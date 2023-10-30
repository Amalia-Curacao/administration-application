import { ReactElement } from "react";
import { IconType } from "react-icons";

export default interface PageLink {
    name: string;
    path: string;
    element: ReactElement;
    icon: ReactElement<IconType>;
}