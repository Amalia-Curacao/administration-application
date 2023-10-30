import { ReactElement } from "react";
import PageLink from "../types/PageLink";

export default function Sidebar({links} : {links: PageLink[]}): ReactElement{
    return (
        <nav className="p-3 start-0 text-info bg-primary vh-100">
        {links.map(link => 
        <a key={link.path} className="d-flex p-1" href={link.path}>
            <div className="p2">
                {link.icon}
            </div> 
        </a>)}
    </nav>
    );
}
