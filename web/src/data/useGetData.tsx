import { Loading } from "@carbon/react";
import { type ReactNode, useEffect, useMemo, useState } from "react";

export type DataState<T> =
	| { type: "Loading" }
	| { type: "Error"; error: string }
	| { type: "Loaded"; result: T };

export const useGetData = <T,>(fn: () => Promise<T>) => {
	const [state, setState] = useState<DataState<T>>({ type: "Loading" });

	useEffect(() => {
		(async () => {
			try {
				setState({ type: "Loaded", result: await fn() });
			} catch (e) {
				// biome-ignore lint/suspicious/noConsole: <explanation>
				console.error(e);
				const message =
					e instanceof Error ? e.message : (e as object).toString();
				setState({ type: "Error", error: message });
			}
		})();
	}, [fn]);

	return state;
};

export const DataStateView = <T,>(props: {
	fn: () => Promise<T>;
	children: (data: T) => ReactNode;
}): ReactNode => {
	const state = useGetData(props.fn);
	const view = useMemo(() => {
		switch (state.type) {
			case "Loading":
				return <Loading active={true} description="Loading" />;
			case "Error":
				return state.error;
			case "Loaded":
				return props.children(state.result);
		}
	}, [state, props.children]);

	return view;
};
