export const link: string = "/schedule"; 

export default function ScheduleIndex(): JSX.Element{
    return (
        <>
            <Header/>
            <Index/>
        </>
    )
}

function Header(): JSX.Element{
    return (
        <header>
            <h1>Schedule</h1>
        </header>
    )
}

function Index() : JSX.Element{
    return (
        <section>
        </section>
    )
}