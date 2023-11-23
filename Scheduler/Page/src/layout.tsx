import { ReactElement } from "react";
import Sidebar from "./components/sidebar";
import './scss/layout.scss';
import { routes } from "./routes";

export default function Layout({children}: {children: ReactElement}): ReactElement{
    return (
    <div className="d-flex flex-fill">
        <Sidebar links={Object.values(routes)}/>
        <main className="bg-secondary w-100">
            {children}
        </main>
    </div>
    );
}