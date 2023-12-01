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

function Action() : Schedule | undefined {
    return { 
        id: Number(_idField.current?.innerText) ?? -1, 
        name: _nameField.current?.value ?? "", 
        rooms: [], 
        reservations: [] 
    };
}

export default function ScheduleEdit(schedule: Schedule) : {body: ReactElement, action: () => Schedule | undefined} {
    return({body: <EditSchedule schedule={schedule}/>, action: Action});
}