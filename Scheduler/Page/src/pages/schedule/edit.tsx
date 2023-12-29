import { ReactElement, RefObject, createRef } from "react";
import Schedule from "../../models/Schedule";

let _idField: RefObject<HTMLInputElement> = createRef() ;
let _nameField: RefObject<HTMLInputElement> = createRef();

function EditSchedule({schedule} : {schedule: Schedule}) : ReactElement {
    const { id, name } = schedule;

    return(<>
        <td colSpan={1} className="bg-secondary">
            <span ref={_idField} className="fw-bold bg-secondary">
                {id}
            </span>
        </td>
        <td colSpan={1} className="bg-secondary">
            <input ref={_nameField} defaultValue={name ?? ""} type="text" className="form-control bg-secondary border-primary"/>
        </td>
    </>);
}

function Action(schedule : Schedule) : Schedule | undefined {
    return { 
        id: schedule.id, 
        name: _nameField.current?.value ?? "", 
        rooms: [], 
        reservations: [] 
    };
}

export default function ScheduleEdit(schedule: Schedule) : {body: ReactElement, action: () => Schedule | undefined} 
{
    if(schedule.id === undefined) throw new Error("Schedule id is not defined");
    return({body: <EditSchedule schedule={schedule}/>, action: () => Action(schedule)});
}