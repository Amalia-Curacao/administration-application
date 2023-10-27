import { ReactElement } from "react";

export default interface PageLink {
    name: string;
    path: string;
    element: ReactElement;
}