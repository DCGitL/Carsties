"use client";
import React, { useEffect, useState } from "react";
import AuctionCard from "./AuctionCard";
import { Auction } from "@/types";
import Appagination from "../components/AppPagination";
import { getData } from "../actions/auctionActions";
import Filter from "./Filter";
import { useParamsStore } from "@/hooks/useParamsStore";
import { useShallow } from "zustand/shallow";
import qs from "query-string";
import EmptyFilter from "../components/EmptyFilter";
import { useAuctionStore } from "@/hooks/useAuctionStore";

export default function Listings() {
	const [loading, setLoading] = useState(true);
	const params = useParamsStore(
		useShallow((state) => ({
			pageNumber: state.pageNumber,
			pageSize: state.pageSize,
			pageCount: state.pageCount,
			searchTerm: state.searchTerm,
			orderBy: state.orderBy,
			filterBy: state.filterBy,
			seller: state.seller,
			winner: state.winner,
		}))
	);
	const data = useAuctionStore(
		useShallow((state) => ({
			auctions: state.auctions,
			totalCount: state.totalCount,
			pageCount: state.pageCount,
		}))
	);

	const setData = useAuctionStore((state) => state.setData);

	const setParams = useParamsStore((state) => state.setParams);

	const url = qs.stringifyUrl(
		{ url: "", query: params },
		{ skipEmptyString: true }
	);

	function setPageNumber(pageNumber: number) {
		setParams({ pageNumber });
	}

	useEffect(() => {
		getData(url).then((data) => {
			setData(data);
			setLoading(false);
		});
	}, [url, setData]);

	if (loading) return <div>Loading...</div>;

	return (
		<>
			<Filter />
			{data.totalCount === 0 ? (
				<EmptyFilter showReset={true} />
			) : (
				<>
					<div className="grid grid-cols-4 gap-2">
						{data &&
							data.auctions.map((auction: Auction) => (
								<AuctionCard
									key={auction.id}
									auction={auction}
								/>
							))}
					</div>
					<Appagination
						pageChanged={(page) => setPageNumber(page)}
						currentPage={params.pageNumber}
						pageCount={data.pageCount}
					/>
				</>
			)}
		</>
	);
}
