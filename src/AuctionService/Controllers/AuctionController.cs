using System;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public AuctionController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }



    [HttpGet]
    public async Task<IActionResult> GetAllAuctions(string? date)
    {
        var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();
        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        var auctions = await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        return Ok(auctions);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuctionById(Guid id)
    {
        var auction = await _context.Auctions.Include(x => x.Item)
           .FirstOrDefaultAsync(x => x.Id == id);
        if (auction is null)
            return NotFound($"Item with id {id} was not found");

        return Ok(_mapper.Map<AuctionDto>(auction));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuction(CreateAuctionDto auctionDto)
    {
        var auction = _mapper.Map<Auction>(auctionDto);
        //Todo: add current user as seller
        auction.Seller = "test";
        _context.Auctions.Add(auction);
        var newAuction = _mapper.Map<AuctionDto>(auction);

        await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return BadRequest("Could not save changes to Db");


        return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, newAuction);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
        if (auction is null)
        {
            return NotFound($"Auction with id {id} was not found");
        }

        //TODO: check seller == username before updating
        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;


        var updateAuction = new AuctionUpdated
        {
            Id = id.ToString(),
            Make = auction.Item.Make,
            Model = auction.Item.Model,
            Year = auction.Item.Year,
            Color = auction.Item.Color,
            Mileage = auction.Item.Mileage
        };
        await _publishEndpoint.Publish(updateAuction);

        var result = _context.SaveChanges() > 0;
        if (result)
        {
            return Ok();

        }
        return BadRequest("Problem saving changes");
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions.FindAsync(id);

        if (auction is null)
        {
            return NotFound($"Auction with id {id} was not found");
        }

        //Todo: check seller == username
        var auctionDeleted = new AuctionDeleted
        {
            Id = id.ToString()
        };

        await _publishEndpoint.Publish(auctionDeleted);

        _context.Auctions.Remove(auction);
        var result = await _context.SaveChangesAsync() > 0;
        if (result)
        {
            return Ok();
        }

        return BadRequest($"Auction with id {id} could not delete");
    }


}
