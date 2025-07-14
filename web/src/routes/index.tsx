import { createFileRoute } from "@tanstack/react-router";

import {
	Button,
	CodeSnippet,
	Column,
	Form,
	Grid,
	Loading,
	ModalBody,
	ModalFooter,
	ModalHeader,
	Stack,
	TextInput,
} from "@carbon/react";
import { useBlackfinchApiContext } from "@data/BlackfinchApiContext";
import type { DataState } from "@data/useGetData";
import { HandledModal } from "@src/components/Modal";
import {
	type FormEventHandler,
	type FunctionComponent,
	useCallback,
	useMemo,
	useState,
} from "react";
import {
	type FlatErrors,
	flatten,
	maxValue,
	minValue,
	number,
	object,
	pipe,
	safeParse,
	string,
	transform,
} from "valibot";

const applicationSchema = object({
	loanAmountPence: pipe(
		string(),
		transform(Number),
		number(),
		minValue(10000000),
		maxValue(150000000),
	),
	assetValuePence: pipe(string(), transform(Number), number(), minValue(1)),
	creditScore: pipe(
		string(),
		transform(Number),
		number(),
		minValue(1),
		maxValue(999),
	),
});

export const App: FunctionComponent = () => {
	const [errors, setErrors] = useState<FlatErrors<typeof applicationSchema>>();
	const context = useBlackfinchApiContext();
	const [dataState, setDataState] = useState<DataState<string>>();

	const onFormSubmit: FormEventHandler<HTMLFormElement> = useCallback(
		async (event) => {
			event.preventDefault();
			const objectData = Object.fromEntries(new FormData(event.currentTarget));
			const parsed = safeParse(applicationSchema, objectData);
			if (parsed.issues) {
				setErrors(flatten<typeof applicationSchema>(parsed.issues));
				return;
			}

			setErrors(undefined);
			if (parsed.success) {
				setDataState({ type: "Loading" });
				try {
					const response = await context.loanApplyPost({
						createLoanApplicationRequest: parsed.output,
					});
					setDataState({ type: "Loaded", result: response });
				} catch (e) {
					// biome-ignore lint/suspicious/noConsole: <explanation>
					console.error(e);
					const message =
						e instanceof Error ? e.message : (e as object).toString();
					setDataState({ type: "Error", error: message });
				}
			}
		},
		[context.loanApplyPost],
	);

	const submitView = useMemo(() => {
		if (!dataState) {
			return null;
		}
		switch (dataState.type) {
			case "Loading":
				return <Loading active={true} description="Loading" />;
			case "Error":
				return (
					<HandledModal onClose={() => setDataState(undefined)}>
						<ModalHeader>
							<h2>Error</h2>
						</ModalHeader>
						<ModalBody>
							<Stack gap={6}>
								There was a problem creating the application.
								<CodeSnippet>{dataState.error}</CodeSnippet>
							</Stack>
						</ModalBody>
						<ModalFooter>
							<Button onClick={() => setDataState(undefined)} kind="primary">
								Close
							</Button>
						</ModalFooter>
					</HandledModal>
				);
			case "Loaded":
				return (
					<HandledModal onClose={() => setDataState(undefined)}>
						<ModalHeader>
							<h2>Success</h2>
						</ModalHeader>
						<ModalBody>Application successfully created.</ModalBody>
					</HandledModal>
				);
		}
	}, [dataState]);

	return (
		<Grid>
			{submitView}
			<Column lg={16} md={8} sm={4}>
				<Stack gap={6}>
					<h1>Create Application</h1>
					<Form onSubmit={onFormSubmit}>
						<Stack gap={6}>
							<TextInput
								id="loan-amount"
								name="loanAmountPence"
								labelText="Loan Amount"
								placeholder="Loan Amount in Pence"
								invalid={(errors?.nested?.loanAmountPence?.length ?? 0) > 0}
								invalidText={errors?.nested?.loanAmountPence?.join(", ")}
							/>
							<TextInput
								id="asset-value"
								name="assetValuePence"
								labelText="Asset Value"
								placeholder="Asset Value in Pence"
								invalid={(errors?.nested?.assetValuePence?.length ?? 0) > 0}
								invalidText={errors?.nested?.assetValuePence?.join(", ")}
							/>
							<TextInput
								id="credit-score"
								name="creditScore"
								labelText="Credit Score"
								placeholder="Credit Score (1–999)"
								invalid={(errors?.nested?.creditScore?.length ?? 0) > 0}
								invalidText={errors?.nested?.creditScore?.join(", ")}
							/>
							<Button type="submit">Submit</Button>
						</Stack>
					</Form>
				</Stack>
			</Column>
		</Grid>
	);
};

export const Route = createFileRoute("/")({
	component: App,
});
