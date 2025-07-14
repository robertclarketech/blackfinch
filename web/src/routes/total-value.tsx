import { Column, Grid, Stack } from "@carbon/react";
import { useBlackfinchApiContext } from "@data/BlackfinchApiContext";
import { DataStateView } from "@data/useGetData";
import { createFileRoute } from "@tanstack/react-router";
import type { FunctionComponent } from "react";

const TotalValue: FunctionComponent = () => {
	const context = useBlackfinchApiContext();
	return (
		<DataStateView fn={() => context.loanTotalValueGet()}>
			{(response) => (
				<Grid>
					<Column lg={16} md={8} sm={4}>
						<Stack gap={6}>
							<h1>Total Loan Value</h1>
							<p>The total value of all approved loans is: {response}</p>
						</Stack>
					</Column>
				</Grid>
			)}
		</DataStateView>
	);
};

export const Route = createFileRoute("/total-value")({
	component: TotalValue,
});
