import { ReactElement } from "react";
import './scss/layout.scss';

export default function Layout({children}: {children: ReactElement}): ReactElement{
    return (
    <div className="d-flex flex-fill">
        { /* <Sidebar links={Object.values(routes)}/> */ }
        <main className="bg-secondary w-100 vh-100">
            {children}
        </main>
    </div>
    );
}