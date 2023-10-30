import { ReactElement } from "react";
import "./layout.css"
import { routes } from "./routes";
import Sidebar from "./components/sidebar";

export default function Layout({children}: {children: ReactElement}): ReactElement{
    return (
    <div className="d-flex vh-100">
        <Sidebar links={routes}/>
        <main className="bg-secondary vw-100 p-3">
            {children}
        </main>
    </div>
    );
}