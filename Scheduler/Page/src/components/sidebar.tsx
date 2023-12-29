import { ReactElement } from "react";
import { FaSmileBeam } from "react-icons/fa";
import PageLink from "../types/PageLink";

export default function Sidebar({links} : {links: PageLink[]}): ReactElement{
    const baseClass= "p-3 text-secondary bg-primary min-vh-100";
    const fixedPosition = "position-fixed top-0 start-0";
    return (<>
    <div className={baseClass}>
        <Link icon={<FaSmileBeam />} path="#"/>
    </div>
    <div className={baseClass + ' ' + fixedPosition}>
        {links.map(link => <Link key={link.route} icon={link.icon} path={link.route}/>)}
    </div>
    </>);
}

function Link({path, icon}: {path: string, icon: ReactElement}): ReactElement {
    return (<>
        <a className="d-flex p-1 pb-2" href={path}>
            <div style={{fontSize: "20px"}}>
                {icon}
            </div>
        </a>
    </>);
}
