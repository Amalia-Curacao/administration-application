import { ReactElement } from "react";
import PageLink from "../../types/PageLink";

export const link: PageLink = {name: "Schedule Index", path: "/schedule", element: <ScheduleIndex/>};

export default function ScheduleIndex(): ReactElement{
    return (
        <>
            <Header/>
            <Index/>
        </>
    )
}

function Header(): ReactElement{
    return (
        <header>
            <h1>Schedule</h1>
        </header>
    )
}

function Index() : ReactElement{
    return (
        <section>
        </section>
    )
}