import { ReactElement } from "react";
import { IconType } from "react-icons";

interface PageLink {
    route: string;
    element: ReactElement;
    icon: ReactElement<IconType>;
    params?: string;
}


export default PageLink; 

