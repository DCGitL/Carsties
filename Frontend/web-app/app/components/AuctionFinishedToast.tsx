import { Auction, AuctionFinished } from "@/types";
import Link from "next/link";
import Image from "next/image";
import React from "react";
import { numberWithCommas } from "@/lib/numberWithComma";

type Props = {
	finishedAuction: AuctionFinished;
	auction: Auction;
};
export default function AuctionFinishedToast({
	finishedAuction,
	auction,
}: Props) {
	return (
		<Link
			href={`/auctions/details/${auction.id}`}
			className="flex flex-col items-center">
			<Image
				src={auction.imageUrl}
				alt="Image of car"
				height={80}
				width={80}
				className="rounded-lg w-auto h-auto"
			/>
			<span>
				New Auction! {auction.make} {auction.model} has been added
			</span>
			{finishedAuction.itemSold && finishedAuction.amount ? (
				<p>
					Congrats to {finishedAuction.winner} who has won this auction for $$
					{numberWithCommas(finishedAuction.amount)}
				</p>
			) : (
				<p>This item did not sell</p>
			)}
		</Link>
	);
}
