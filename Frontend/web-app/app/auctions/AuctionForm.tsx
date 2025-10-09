"use client";
import { Button, Spinner } from "flowbite-react";
import { usePathname, useRouter } from "next/navigation";
import React, { useEffect } from "react";
import { FieldValues, useForm } from "react-hook-form";
import Input from "../components/Input";
import DateInput from "../components/DateInput";
import { createAuction, updateAuction } from "../actions/auctionActions";
import toast from "react-hot-toast";
import { Auction } from "@/types";

type Props = {
	auction?: Auction;
};
export default function AuctionForm({ auction }: Props) {
	const router = useRouter();
	const pathname = usePathname();
	const {
		control,
		handleSubmit,
		setFocus,
		reset,
		formState: { isSubmitting, isValid, isDirty },
	} = useForm({
		mode: "onTouched",
	});

	useEffect(() => {
		if (auction) {
			const { make, model, color, mileage, year } = auction;
			reset({ make, model, color, mileage, year });
		}
		setFocus("make");
	}, [setFocus, auction, reset]);

	const onSubmit = async (data: FieldValues) => {
		try {
			let id = "";
			let res;
			if (pathname === "/auctions/create") {
				res = await createAuction(data);
				id = res.id;
			} else {
				if (auction) {
					id = auction.id;
					res = await updateAuction(data, id);
				}
			}

			if (res.error) {
				throw res.error;
			}
			console.log(data);
			router.push(`/auctions/details/${id}`);
		} catch (error: any) {
			toast.error(error.status + " " + error.message);
		}
	};
	return (
		<form
			className="flex flex-col mt-3"
			onSubmit={handleSubmit(onSubmit)}>
			<Input
				label="Make"
				name="make"
				control={control}
				rules={{ required: "Make is required" }}
			/>
			<Input
				label="Model"
				name="model"
				control={control}
				rules={{ required: "Model is required" }}
			/>
			<Input
				label="Color"
				name="color"
				control={control}
				rules={{ required: "Color is required" }}
			/>
			<div className="grid grid-cols-2 gap-3">
				<Input
					label="Year"
					name="year"
					type="number"
					control={control}
					rules={{ required: "Year is required" }}
				/>
				<Input
					label="Mileage"
					name="mileage"
					type="number"
					control={control}
					rules={{ required: "Mileage is required" }}
				/>
			</div>
			{pathname === "/auctions/create" && (
				<>
					<Input
						label="ImageUrl"
						name="imageUrl"
						control={control}
						rules={{ required: "Image Url is required" }}
					/>
					<div className="grid grid-cols-2 gap-3">
						<Input
							label="Reserve price (enter 0 if no reserve price)"
							name="reservePrice"
							type="number"
							control={control}
							rules={{ required: "Reserve is required" }}
						/>
						<DateInput
							name="auctionEnd"
							label="Auction end date/time"
							control={control}
							showTimeSelect
							dateFormat="dd MMMM yyyy h:mm a"
							rules={{ required: "Auction End date is required" }}
						/>
					</div>
				</>
			)}
			<div className="flex justify-between">
				<Button
					color="alternative"
					onClick={() => router.push("/")}>
					{" "}
					Cancel
				</Button>

				<Button
					outline
					color="green"
					type="submit"
					disabled={!isValid || !isDirty}>
					{isSubmitting && <Spinner size="sm" />}
					Submit
				</Button>
			</div>
		</form>
	);
}
