import { ReactElement } from "react";
import PageLink from "../../types/PageLink";
import { GrSchedules } from "react-icons/gr";
import { AiOutlinePlus } from "react-icons/ai";

export const link: PageLink = {name: "Schedule index", path: "/schedule", element: <ScheduleIndex/>, icon: <GrSchedules/>};

export default function ScheduleIndex(): ReactElement{
    return (
        <div className="p-3">
            <PageTitle/>
            <div className="pt-3">
                <Index/>
            </div>
        </div>
    )
}

function PageTitle(): ReactElement{
    return (
        <div className="d-flex">
            <h1 className="pe-3">{link.name}</h1>
            <div className="btn-group">
                <a type="button" className="btn btn-outline-success" href="#">
                    <AiOutlinePlus/>
                </a>
            </div>
        </div>
    )
}

function Index() : ReactElement{
    return (
        <table className="table table-hover">
            <thead className="h4">
                <tr>
                    <th scope="col" className="pe-3 table-bordered bg-secondary">
                        Id
                    </th>
                    <th scope="col" className="pe-3 table-bordered bg-secondary">
                        Name
                    </th>
                    <th scope="col" className="pe-3 bg-secondary"/>
                </tr>
            </thead>
            <tbody>
                {Schedule()}
            </tbody>
        </table>
    ) 
}

function Schedule(): ReactElement{
    return (
        <tr>
            <td className="table-bordered bg-secondary">
                1
            </td>
            <td className="table-bordered bg-secondary">
                Amalia
            </td>
            <td className="btn-group bg-secondary rounded-0">
                <a className="btn btn-outline-warning" href="#">Details</a>
                <a className="btn btn-outline-danger" href="#">Delete</a>
            </td>
        </tr>
    );
}