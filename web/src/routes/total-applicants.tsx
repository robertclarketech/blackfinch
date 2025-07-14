import {
	Column,
	DataTable,
	Grid,
	Stack,
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from "@carbon/react";
import { useBlackfinchApiContext } from "@data/BlackfinchApiContext";
import { DataStateView } from "@data/useGetData";
import { createFileRoute } from "@tanstack/react-router";
import type { FunctionComponent } from "react";

const headers = [
	{
		key: "approval",
		header: "Approval Status",
	},
	{
		key: "amount",
		header: "Amount",
	},
];

const TotalApplicants: FunctionComponent = () => {
	const context = useBlackfinchApiContext();
	return (
		<DataStateView fn={() => context.loanTotalApplicantsGet()}>
			{(response) => {
				const rows = Object.entries(response).map(([key, value]) => ({
					id: key,
					approval: key,
					amount: value ?? 0,
				}));
				return (
					<Grid>
						<Column lg={16} md={8} sm={4}>
							<Stack gap={6}>
								<h1>Total Applicants</h1>
								<DataTable rows={rows} headers={headers}>
									{({
										rows,
										headers,
										getTableProps,
										getHeaderProps,
										getRowProps,
									}) => (
										<Table {...getTableProps()}>
											<TableHead>
												<TableRow>
													{headers.map((header) => (
														// biome-ignore lint/correctness/useJsxKeyInIterable: <explanation>
														<TableHeader {...getHeaderProps({ header })}>
															{header.header}
														</TableHeader>
													))}
												</TableRow>
											</TableHead>
											<TableBody>
												{rows.map((row) => (
													// biome-ignore lint/correctness/useJsxKeyInIterable: <explanation>
													<TableRow {...getRowProps({ row })}>
														{row.cells.map((cell) => (
															<TableCell key={cell.id}>{cell.value}</TableCell>
														))}
													</TableRow>
												))}
											</TableBody>
										</Table>
									)}
								</DataTable>
							</Stack>
						</Column>
					</Grid>
				);
			}}
		</DataStateView>
	);
};

export const Route = createFileRoute("/total-applicants")({
	component: TotalApplicants,
});
