import type { NextConfig } from "next";
import withFlowbiteReact from "flowbite-react/plugin/nextjs";

const nextConfig: NextConfig = {
	//this shows logging at the console
	logging: {
		fetches: {
			fullUrl: true,
		},
	},
	images: {
		remotePatterns: [
			{
				protocol: "https",
				hostname: "cdn.pixabay.com",
			},
		],
	},
};

export default withFlowbiteReact(nextConfig);