import { ReactElement, RefObject, createRef } from "react";
import Schedule from "../../models/Schedule";

let refIdField: RefObject<HTMLInputElement> = createRef() ;
let refNameField: RefObject<HTMLInputElement> = createRef();

function EditSchedule({schedule} : {schedule: Schedule}) : ReactElement {
    const { id, name } = schedule;

    return(<>
        <td colSpan={1} className="bg-secondary">
            <span ref={refIdField} className="fw-bold bg-secondary">
                {id}
            </span>
        </td>
        <td colSpan={1} className="bg-secondary">
            <input ref={refNameField} defaultValue={name ?? ""} type="text" className="form-control bg-secondary border-primary"/>
        </td>
    </>);
}

function Action() : Schedule | null {
    return { 
        id: Number(refIdField.current?.innerText) ?? -1, 
        name: refNameField.current?.value ?? "", 
        rooms: [], 
        reservations: [] 
    };
}

export default function ScheduleEdit(schedule: Schedule) : {body: ReactElement, action: () => Schedule | null} {
    return({body: <EditSchedule schedule={schedule}/>, action: Action});
}