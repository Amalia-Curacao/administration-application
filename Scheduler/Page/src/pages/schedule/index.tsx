import { ForwardedRef, ReactElement, createRef, useRef } from "react";
import PageLink from "../../types/PageLink";
import { GrSchedules } from "react-icons/gr";
import ScheduleCreate, { handleSave } from "./create";

export const link: PageLink = {name: "Schedule index", path: "/schedule", element: <ScheduleIndex/>, icon: <GrSchedules/>};
export default function ScheduleIndex(): ReactElement{
    return (
        <div className="p-3 pe-3 d-flex flex-column flex-fill">
            <PageTitle/>
            <div className="pt-3">
                <table className="table table-hover table-secondary">
                    <TableHeader/>
                    <TableBody/>
                </table>
            </div>
        </div>
    );
}

function PageTitle(): ReactElement{
    return (
        <div className="d-flex justify-content-between">
            <h1>Schedules</h1>
            <div className="h1">{link.icon}</div>
        </div>
    )
}

function TableHeader(): ReactElement{
    return(
    <thead className="h4">
        <tr className="bg-secondary">
            <th className="pe-3 table-bordered bg-secondary">
                Id
            </th>
            <th className="pe-3 table-bordered bg-secondary">
                Name
            </th>
            <th className="bg-secondary">
                <a className="btn btn-outline-success float-end" href="#">
                    Add
                </a>
            </th>
        </tr>
    </thead>);
}

function TableBody(): ReactElement{
    const childRef = useRef<handleSave>(null);
    return (
        <tbody>
            <tr className="bg-secondary" hidden={false}>
                <td className="bg-secondary"></td>
                <td className="bg-secondary">
                <ScheduleCreate ref={childRef} />
                </td>
                <td className="bg-secondary">
                    <button onClick={() => {if(childRef.current) childRef.current.save()}} className="btn btn-outline-success float-end">
                        Save
                    </button>
                </td>
            </tr>

            <tr>
                <th className="bg-secondary">
                    1
                </th>
                <th className="bg-secondary">
                    Amalia
                </th>
                <td className="bg-secondary">
                    <div className="btn-group float-end">
                        <a className="btn btn-outline-primary" href="#">Details</a>
                        <a className="btn btn-outline-warning" href="#">Edit</a>
                        <a className="btn btn-outline-danger" href="#">Delete</a>
                    </div>
                </td>
            </tr>

            <tr>
                <td colSpan={3} className="bg-secondary rounded-0">
                    <a className="d-block text-center btn btn-outline-success" href="#">
                        Add
                    </a>
                </td>
            </tr>           
        </tbody>
    );
}