import { ReactElement } from "react";
import PageLink from "../../types/PageLink";
import { GrSchedules } from "react-icons/gr";

export const link: PageLink = {name: "Schedule Index", path: "/schedule", element: <ScheduleIndex/>, icon: <GrSchedules/>};

export default function ScheduleIndex(): ReactElement{
    return (
        <>
            <PageTitle/>
            <Index/>
        </>
    )
}

function PageTitle(): ReactElement{
    return (
        <h1>Schedule</h1>
    )
}

function Index() : ReactElement{
    return (
        <section>
        </section>
    )
}