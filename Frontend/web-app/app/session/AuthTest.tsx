"use client";

import { Button, Spinner } from "flowbite-react";
import { useState } from "react";
import { updatAuctionTest } from "../actions/auctionActions";

export default function AuthTest() {
	const [loading, setLoading] = useState(false);
	const [result, setResult] = useState<{
		status: number;
		message: string;
	} | null>(null);

	const handleUpdate = () => {
		setResult(null);
		setLoading(true);
		updatAuctionTest()
			.then((res) => setResult(res))
			.catch((err) => setResult(err))
			.finally(() => setLoading(false));
	};

	return (
		<div className="flex item-center gap-4">
			<Button onClick={handleUpdate}>
				{loading && (
					<Spinner
						size="sm"
						className="me-3"
						light
					/>
				)}
				Test auth
			</Button>
			<div>{JSON.stringify(result)}</div>
		</div>
	);
}
