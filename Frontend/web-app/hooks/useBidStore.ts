import { Bid } from "@/types";
import { create } from "zustand";

type State = {
	bids: Bid[];
	open: boolean;
};

type Action = {
	setBids: (bids: Bid[]) => void;
	addBid: (bid: Bid) => void;
	setOpen: (value: boolean) => void;
};

export const useBidStore = create<State & Action>((set) => ({
	bids: [],
	open: true,
	setOpen: (value: boolean) => {
		set(() => ({
			open: value,
		}));
	},
	setBids: (bids: Bid[]) => {
		set(() => ({
			bids,
		}));
	},
	addBid: (bid: Bid) => {
		set((state) => ({
			bids: !state.bids.find((x) => x.id === bid.id)
				? [bid, ...state.bids]
				: [...state.bids],
		}));
	},
	//	addBid: (bid) => set((state) => ({ bids: [...state.bids, bid] })), // immutable push
}));
