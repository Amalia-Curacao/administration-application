import { ReactElement, useState } from "react";
import SaveButton from "../../components/saveButton";
import Schedule from "../../models/Schedule";
import ScheduleEdit from "./edit";
import ScheduleIndex from ".";
import {PageState as State} from "../../types/PageState";
import { routes } from "../../routes";
import { useNavigate } from "react-router-dom";

function ScheduleRow({schedule, onDelete, onEdit}: {schedule: Schedule, onEdit: (schedule: Schedule) => void, onDelete: (schedule: Schedule) => void}): ReactElement {    
    const scheduleRowIndex = <ScheduleRowIndex 
        schedule={schedule} 
        onDelete={onDelete}
        onEdit={() => {updateRow(State.Edit)}}/>;

    const scheduleRowEdit = <ScheduleRowEdit 
        schedule={schedule} 
        onReturn={() => updateRow(State.Default)}
        onSuccess={onEdit} 
        onFailure={() => console.log("error")}/>;

    const [row, setRow] = useState<ReactElement>(scheduleRowIndex);

    function updateRow(changeTo: State){
        switch(changeTo) {
            case State.Edit:
                setRow(scheduleRowEdit);
                break;
            default:
                setRow(scheduleRowIndex);
                break;
        }
    }
    return row;
}

function ScheduleRowIndex({schedule, onEdit, onDelete}: {schedule: Schedule, onEdit: VoidFunction, onDelete: (toDelete: Schedule) => void}): ReactElement {
    return(<tr>
        {ScheduleIndex(schedule).body}
        <td colSpan={1} className="bg-secondary"> 
            <div className="btn-group float-end">
                <ActionGroup onDelete={() => onDelete(schedule)} schedule={schedule} onEdit={onEdit}/>
            </div>
        </td>
    </tr>);
}

function ScheduleRowEdit({schedule, onReturn, onFailure, onSuccess}: {schedule: Schedule, onReturn: VoidFunction, onFailure: VoidFunction, onSuccess: (toEdit: Schedule) => void}): ReactElement {
    function onSave(toEdit: Schedule): Schedule | undefined {
        const schedule = ScheduleEdit(toEdit).action();
        if(!schedule) return undefined;
        onSuccess(schedule);
        return schedule;
    } 
    
    return(<tr>
        {ScheduleEdit(schedule).body}
        <td colSpan={1} className="bg-secondary"> 
            <div className="btn-group float-end">
                <SaveButton onSave={() => onSave(schedule)} onFailure={onFailure} onReturn={onReturn} />
            </div>
        </td>
    </tr>);

}

function ActionGroup({schedule, onDelete, onEdit}: {schedule: Schedule, onDelete: VoidFunction, onEdit: VoidFunction}): ReactElement {
    const navigate = useNavigate(); 

    return(
        <div className="btn-group">
            <button onClick={() => navigate(routes["room index"].route + "/" + schedule.id, {replace: true})} className="btn btn-outline-primary">Details</button>
            <button onClick={onEdit} className="btn btn-outline-warning">Edit</button>
            <button onClick={onDelete} className="btn btn-outline-danger">Delete</button>
        </div>);
}

export default ScheduleRow;