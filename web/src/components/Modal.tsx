import { ComposedModal } from "@carbon/react";
import type { FunctionComponent, ReactNode } from "react";
import { createPortal } from "react-dom";

export const HandledModal: FunctionComponent<{
	children: ReactNode;
	onClose: () => void;
}> = (props) => {
	return (
		<>
			{typeof document === "undefined"
				? null
				: createPortal(
						<ComposedModal open={true} onClose={props.onClose}>
							{props.children}
						</ComposedModal>,
						document.body,
					)}
		</>
	);
};
