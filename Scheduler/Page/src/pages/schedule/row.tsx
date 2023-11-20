import { ReactElement, useState } from "react";
import SaveButton from "../../components/saveButton";
import Schedule from "../../models/Schedule";
import ScheduleEdit from "./edit";
import ScheduleIndex from ".";
import {PageState as State} from "../../types/PageState";

function ScheduleRow({schedule, onDelete, onEdit}: {schedule: Schedule, onEdit: (schedule: Schedule) => void, onDelete: (schedule: Schedule) => void}): ReactElement {    
        
    const scheduleRowIndex = <ScheduleRowIndex 
        schedule={schedule} 
        onDelete={onDelete} 
        onDetails={() => console.log("details")} 
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

function ScheduleRowIndex({schedule, onEdit, onDelete, onDetails}: {schedule: Schedule, onEdit: VoidFunction, onDelete: (toDelete: Schedule) => void, onDetails: VoidFunction}): ReactElement {
    
    function removeSchedule(toDelete: Schedule) {
        onDelete(toDelete);
    }
    
    return(<tr>
        {ScheduleIndex(schedule).body}
        <td colSpan={1} className="bg-secondary"> 
            <div className="btn-group float-end">
                <ActionGroup onDelete={() => removeSchedule(schedule)} onDetails={onDetails} onEdit={onEdit}/>
            </div>
        </td>
    </tr>);
}

function ScheduleRowEdit({schedule, onReturn, onFailure, onSuccess}: {schedule: Schedule, onReturn: VoidFunction, onFailure: VoidFunction, onSuccess: (toEdit: Schedule) => void}): ReactElement {
    function onSave(toEdit: Schedule): Schedule | null {
        const schedule = ScheduleEdit(toEdit).action();
        if(schedule === null) return null;
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

function ActionGroup({onDetails, onDelete, onEdit}: {onDetails: VoidFunction, onDelete: VoidFunction, onEdit: VoidFunction}): ReactElement {
    return(
        <div className="btn-group">
            <button onClick={onDetails} className="btn btn-outline-primary">Details</button>
            <button onClick={onEdit} className="btn btn-outline-warning">Edit</button>
            <button onClick={onDelete} className="btn btn-outline-danger">Delete</button>
        </div>);
}

export default ScheduleRow;