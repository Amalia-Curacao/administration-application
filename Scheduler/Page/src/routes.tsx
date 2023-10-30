import PageLink from "./types/PageLink";
import { link as ScheduleIndexLink } from "./pages/schedule/index";
import { RouteObject } from "react-router-dom";
import { AiFillHome } from "react-icons/ai";

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
        icon: <AiFillHome/>
    }


export const routes: PageLink[] = [
    defaultPage,
    ScheduleIndexLink
];