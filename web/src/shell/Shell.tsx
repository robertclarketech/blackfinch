import {
	Content,
	Header,
	HeaderMenuButton,
	HeaderNavigation,
	HeaderSideNavItems,
	SideNav,
	SideNavItems,
} from "@carbon/react";
import { Outlet } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";
import { type FunctionComponent, useState } from "react";
import { NavItems } from "./NavItems.tsx";

export const Shell: FunctionComponent = () => {
	const [navOpen, setNavOpen] = useState<boolean>(false);
	return (
		<>
			<Header>
				<HeaderMenuButton
					onClick={() => setNavOpen((prev) => !prev)}
					isActive={navOpen}
				/>

				<SideNav
					aria-label="Side navigation1"
					expanded={navOpen}
					onSideNavBlur={() => setNavOpen(false)}
					onOverlayClick={() => setNavOpen(false)}
					isPersistent={false}
				>
					<SideNavItems>
						<HeaderSideNavItems>
							<NavItems />
						</HeaderSideNavItems>
					</SideNavItems>
				</SideNav>
				<HeaderNavigation>
					<NavItems />
				</HeaderNavigation>
			</Header>
			<Content>
				<Outlet />
			</Content>
			<TanStackRouterDevtools />
		</>
	);
};
