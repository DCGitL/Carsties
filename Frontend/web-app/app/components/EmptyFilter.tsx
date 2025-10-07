"use client";
import { useParamsStore } from "@/hooks/useParamsStore";
import React from "react";
import Heading from "./Heading";
import { Button } from "flowbite-react";
import { signIn } from "next-auth/react";
type Props = {
	title?: string;
	subtitle?: string;
	showReset?: boolean;
	showLogin?: boolean;
	callbackUrl?: string;
};

export default function EmptyFilter({
	title = "No matches found for your search or filter criteria",
	subtitle = "Try adjusting your search or filter to find what you are looking for.",
	showReset,
	showLogin,
	callbackUrl,
}: Props) {
	const reset = useParamsStore((state) => state.reset);

	return (
		<div className="flex flex-col gap-2 items-center justify-center h-[40vh] shadow-lg">
			<Heading
				title={title}
				subtitle={subtitle}
				center
			/>
			<div className="w-48 mt-4">
				{showReset && (
					<Button
						onClick={reset}
						outline>
						Reset Filters
					</Button>
				)}
				{showLogin && (
					<Button
						onClick={() => signIn("id-server", { redirectTo: callbackUrl })}
						outline>
						Login
					</Button>
				)}
			</div>
		</div>
	);
}
