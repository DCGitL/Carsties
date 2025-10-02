"use client";
import { Pagination } from "flowbite-react";
import EmptyFilter from "./EmptyFilter";

type Props = {
	currentPage: number;
	pageCount: number;
	pageChanged: (page: number) => void;
};

export default function AppPagination({
	currentPage,
	pageCount,
	pageChanged,
}: Props) {
	const onPageChange = (page: number) => pageChanged(page);

	return (
		<div className="flex overflow-x-auto sm:justify-center">
			<Pagination
				currentPage={currentPage}
				totalPages={pageCount}
				onPageChange={onPageChange}
				showIcons
			/>
		</div>
	);
}
