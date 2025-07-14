import { Column, Grid, Stack } from "@carbon/react";
import { useBlackfinchApiContext } from "@data/BlackfinchApiContext.tsx";
import { DataStateView } from "@data/useGetData";
import { createFileRoute } from "@tanstack/react-router";
import type { FunctionComponent } from "react";

const MeanLtv: FunctionComponent = () => {
	const context = useBlackfinchApiContext();
	return (
		<DataStateView fn={() => context.loanMeanLtvGet()}>
			{(response) => (
				<Grid>
					<Column lg={16} md={8} sm={4}>
						<Stack gap={6}>
							<h1>Average Loan to Value</h1>
							<p>The current average loan to value is: {response}</p>
						</Stack>
					</Column>
				</Grid>
			)}
		</DataStateView>
	);
};

export const Route = createFileRoute("/mean-ltv")({
	component: MeanLtv,
});
