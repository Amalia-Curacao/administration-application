import { ReactElement } from "react";
import { routes } from "./routes";
import Sidebar from "./components/sidebar";

export default function Layout({children}: {children: ReactElement}): ReactElement{
    return (
    <div className="d-flex">
        <Sidebar links={routes}/>
        <main className="d-flex bg-secondary vw-100">
            {children}
        </main>
    </div>
    );
}