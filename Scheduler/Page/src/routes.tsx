import PageLink from "./types/PageLink";
import { link as ScheduleIndexLink } from "./pages/schedule/index";
import { link as ScheduleCreateLink } from "./pages/schedule/create";
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
        name: ScheduleIndexLink.name,
        element: ScheduleIndexLink.element,
        icon: <Logo primaryColor={colors.secondary} secondaryColor={colors.secondary} borderColor="none"/>
    }


export const routes: PageLink[] = [
    defaultPage,
    ScheduleIndexLink,
    ScheduleCreateLink
];