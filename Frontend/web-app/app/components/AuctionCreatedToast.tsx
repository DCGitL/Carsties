import { Auction } from "@/types";
import Link from "next/link";
import Image from "next/image";
import React from "react";

type Props = {
	auction: Auction;
};
export default function AuctionCreatedToast({ auction }: Props) {
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
		</Link>
	);
}
