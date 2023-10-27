import { ReactElement } from "react";

export default function Layout({children}: {children: ReactElement}): ReactElement{
    return (
        <>
            <Navbar>
                <a href="/">Home</a>
            </Navbar>
            {children}
        </>
    )
}

function Navbar({children} : {children: ReactElement}): ReactElement{
    return(
        <nav className="navbar">
            {children}
        </nav>
    );
}