"use client";

import { placeBidForAuction } from "@/app/actions/auctionActions";
import { useBidStore } from "@/hooks/useBidStore";
import { numberWithCommas } from "@/lib/numberWithComma";
import { FieldValues, useForm } from "react-hook-form";
import toast from "react-hot-toast";

type Props = {
	auctionId: string;
	highBid: number;
};
export default function BidForm({ auctionId, highBid }: Props) {
	const { register, handleSubmit, reset } = useForm();
	const addBid = useBidStore((state) => state.addBid);
	// const setBids = useBidStore((state) => state.setBids);
	// const bids = useBidStore((state) => state.bids);
	const onSubmit = async (data: FieldValues) => {
		if (data.amount <= highBid) {
			toast.error(
				`Your bid must be at least $${numberWithCommas(highBid + 1)}`
			);
			reset();
			return;
		}
		placeBidForAuction(auctionId, +data.amount)
			.then((bid) => {
				if (bid.error) {
					reset();
					throw bid.error;
				}
				addBid(bid);
				reset();
			})
			.catch((error) => toast.error(error.message));
		// try {
		// 	const bid = await placeBidForAuction(auctionId, +data.amount);
		// 	// debug
		// 	console.log("placeBid response:", bid);
		// 	if (!bid || (bid as any).error) {
		// 		reset();
		// 		throw (bid as any).error ?? new Error("Invalid bid response");
		// 	}
		// 	// safer: replace whole list (ensures re-render) or use addBid (immutable)
		// 	setBids([...bids, bid]);
		// 	// or: addBid(bid);
		// 	reset();
		// } catch (error: any) {
		// 	toast.error(error?.message ?? "Failed to place bid");
		// }
	};

	return (
		<form
			onSubmit={handleSubmit(onSubmit)}
			className="flex items-center border-2 rounded-lg py-2">
			<input
				type="number"
				{...register("amount")}
				placeholder={`Enter your bid (minimum bid is $${numberWithCommas(
					highBid + 1
				)})`}
				className="flex-grow 
                pl-5 bg-transparent 
                focus:outline-none 
                border-transparent 
                focus:border-transparent 
                focus:ring-0 
                text-sm 
                text-grey-600"
			/>
		</form>
	);
}
