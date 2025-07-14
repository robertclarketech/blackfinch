import { HeaderMenuItem } from "@carbon/react";
import { Link } from "@tanstack/react-router";
import type { FunctionComponent } from "react";

export const NavItems: FunctionComponent = () => {
	return (
		<>
			<HeaderMenuItem as={Link} to="/">
				Create Application
			</HeaderMenuItem>
			<HeaderMenuItem as={Link} to="/total-applicants">
				View Total Applicants
			</HeaderMenuItem>
			<HeaderMenuItem as={Link} to="/total-value">
				View Loans
			</HeaderMenuItem>
			<HeaderMenuItem as={Link} to="/mean-ltv">
				View Mean LTV
			</HeaderMenuItem>
		</>
	);
};
