"use client";

import { useParamsStore } from "@/hooks/useParamsStore";
import React, { ChangeEvent, useEffect, useState } from "react";
import { FaSearch } from "react-icons/fa";

export default function Search() {
	const setParams = useParamsStore((state) => state.setParams);
	const [value, setValue] = useState("");

	const searchTerm = useParamsStore((state) => state.searchTerm);

	useEffect(() => {
		if (searchTerm === "") {
			setValue("");
		}
	}, [searchTerm]);

	function handleChange(event: ChangeEvent<HTMLInputElement>) {
		setValue(event.target.value);
	}

	function handleSearch() {
		setParams({ searchTerm: value });
		//console.log("Searching for:", value);
	}
	return (
		<div className="flex w-[50%] items-center border-2 border-grey-300 rounded-full py-2 shadow-md">
			<input
				onKeyDown={(e) => e.key === "Enter" && handleSearch()}
				onChange={handleChange}
				type="text"
				value={value}
				placeholder="Search for car by make, model or color"
				className="flex-grow 
                pl-5 bg-transparent 
                focus:outline-none 
                border-transparent 
                focus:border-transparent 
                focus:ring-0 
                text-sm 
                text-grey-600"
			/>
			<button onClick={handleSearch}>
				<FaSearch
					size={34}
					className="bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2"
				/>
			</button>
		</div>
	);
}
