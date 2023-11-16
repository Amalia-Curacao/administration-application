import PageLink from "./types/PageLink";
import { link as SchedulesLink } from "./pages/schedule/main";
import { RouteObject } from "react-router-dom";
import Logo from "./svgs/logo";
import colors from "./scss/colors.module.scss";

export default function RouteObjects(): RouteObject[] {
    return (routes.map((route: PageLink) => {
        return {
            path: route.path,
            element: route.element
        }
    }));
}

export const defaultPage: PageLink = {
        path: "/",
        name: SchedulesLink.name,
        element: SchedulesLink.element,
        icon: <Logo primaryColor={colors.secondary} secondaryColor={colors.secondary} borderColor="none"/>
    }


export const routes: PageLink[] = [
    defaultPage,
    SchedulesLink,
];