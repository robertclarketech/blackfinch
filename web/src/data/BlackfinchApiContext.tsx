import {
	type FunctionComponent,
	type ReactNode,
	createContext,
	useContext,
} from "react";
import { BlackfinchApiApi, Configuration } from "./client";

const BlackfinchApiContext = createContext<BlackfinchApiApi | null>(null);

const api = new BlackfinchApiApi(
	new Configuration({ basePath: import.meta.env["VITE_BLACKFINCH_BASE_URL"] }),
);

export const BlackfinchApiContextProvider: FunctionComponent<{
	children: ReactNode;
}> = (props) => {
	return (
		<BlackfinchApiContext.Provider value={api}>
			{props.children}
		</BlackfinchApiContext.Provider>
	);
};

export const useBlackfinchApiContext = (): BlackfinchApiApi => {
	const context = useContext(BlackfinchApiContext);
	if (!context) {
		throw new Error(
			"useBlackfinchApiContext must be used within BlackfinchApiContextProvider",
		);
	}
	return context;
};
