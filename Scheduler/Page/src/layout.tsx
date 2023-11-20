import { ReactElement } from "react";
import { routes } from "./routes";
import Sidebar from "./components/sidebar";
import './scss/layout.scss';

export default function Layout({children}: {children: ReactElement}): ReactElement{
    return (
    <div className="d-flex flex-fill">
        <Sidebar links={routes}/>
        <main className="bg-secondary w-100">
            {children}
        </main>
    </div>
    );
}