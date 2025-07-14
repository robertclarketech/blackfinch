import { RouterProvider, createRouter } from "@tanstack/react-router";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.scss";

import { BlackfinchApiContextProvider } from "@data/BlackfinchApiContext";
// Import the generated route tree
import { routeTree } from "./routeTree.gen";

// Create a new router instance
const router = createRouter({ routeTree });

// Register the router instance for type safety
declare module "@tanstack/react-router" {
	interface Register {
		router: typeof router;
	}
}

const el = document.createElement("div");
document.body.appendChild(el);
createRoot(el).render(
	<StrictMode>
		<BlackfinchApiContextProvider>
			<RouterProvider router={router} />
		</BlackfinchApiContextProvider>
	</StrictMode>,
);
