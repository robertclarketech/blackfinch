import { Shell } from "@src/shell/Shell";
import { createRootRoute } from "@tanstack/react-router";

export const Route = createRootRoute({
	component: () => <Shell />,
});
