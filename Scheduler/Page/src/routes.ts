import PageLink from "./types/PageLink";
import { link as ScheduleIndexLink } from "./pages/schedule/index";
import { RouteObject } from "react-router-dom";

export default function RouteObjects(): RouteObject[] {
    return (routes.map((route: PageLink) => {
        return {
            path: route.path,
            element: route.element
        }
    }));
}

const defaultPage: PageLink = {
        path: "/",
        name: ScheduleIndexLink.name,
        element: ScheduleIndexLink.element
    }


export const routes: PageLink[] = [
    defaultPage,
    ScheduleIndexLink
];