import { ReactElement, useState } from "react";
import PageLink from "../../types/PageLink";
import { GrSchedules } from "react-icons/gr";
import Schedule from "../../models/Schedule";
import "../../extensions/HTMLElement";
import CreateSchedule from "./create";
import ScheduleIndex from ".";
import ScheduleEdit from "./edit";

// #region used types within this module
// Types are written in PascalCasing and start with a capital letter.
enum State {
    Default = "default",
    Create = "create",
    Edit = "edit",
}
// #endregion

// #region Variables
// Variables are written are camelCasing and start with an underscore.
let _state: State = State.Default;
// #endregion

// #region Elements
// Elements are written in PascalCasing and are always functions.

function ScheduleMain(): ReactElement {    
    return(<>
        <div className="p-3 pe-3 d-flex flex-column flex-fill">
            <PageTitle/>
            <Table/>
        </div>
    </>);
}

function PageTitle(): ReactElement { 
    return(
    <div className="d-flex justify-content-between">
        <h1>Schedules</h1>
        <div className="h1">{link.icon}</div>
    </div>);
}
    
function Table(): ReactElement {
    const [creating, setCreating] = useState<boolean>(false);
    const testSchedules: Schedule[] = [
        {id: 1, name: "test1", reservations: [], rooms: []}, 
        {id: 2, name: "test2", reservations: [], rooms: []}, 
        {id: 3, name: "test3", reservations: [], rooms: []}];
    const [schedules, setSchedules] = useState<Schedule[]>(testSchedules);

    function onEdit(toEdit: Schedule) {
        setSchedules(schedules.map(s => s.id === toEdit.id ? toEdit : s));
        console.log(schedules);
    }

    function onDelete(toDelete: Schedule) {
        setSchedules(schedules.filter(s => s.id !== toDelete.id));
        console.log(schedules);
    }

    function onAdd(schedule: Schedule) {
        setSchedules([...schedules, schedule]);
        console.log(schedule);
        console.log(schedules);
    }

    function createState(){
        if(_state !== State.Default) return;
        _state = State.Create;
        setCreating(true);
    }

    return(<>
        <table className="table table-hover table-secondary">
            <thead className="h4">
                <tr className="bg-secondary">
                    <th className="table-bordered bg-secondary">
                        <span>
                            Id
                        </span>
                    </th>
                    <th className="table-bordered bg-secondary">
                        <span>
                            Name
                        </span>
                    </th>
                    <th className="bg-secondary">
                        <button hidden={creating} onClick={createState} className="btn btn-outline-success float-end">
                            Add
                        </button>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr hidden={!creating}>
                    <ScheduleRowCreate addSchedule={onAdd} onReturn={() => {_state = State.Default; setCreating(false)}}/>
                </tr>

                <ScheduleRows schedules={schedules} onEdit={onEdit} onDelete={onDelete}/>

                <tr hidden={creating}>
                    <td colSpan={3} className="bg-secondary rounded-0">
                        <div onClick={createState} className="d-block text-center btn btn-outline-success">
                            Add
                        </div>
                    </td>
                </tr>
            </tbody>        
        </table>
    </>);
}

function ScheduleRows({schedules, onEdit, onDelete}: {schedules: Schedule[], onEdit: (toEdit: Schedule) => void, onDelete: (toDelete: Schedule) => void}): ReactElement {
    return(<>
        {schedules.map(schedule => <ScheduleRow key={schedule.id} schedule={schedule} onEdit={onEdit} onDelete={onDelete}/>)}
    </>);
}

function ScheduleRowCreate({onReturn, addSchedule}: {onReturn: VoidFunction, addSchedule: (schedule: Schedule) => void}): ReactElement {
    function onSave(): Schedule | null{
        const schedule = CreateSchedule().action();
        if(schedule === null) return null;
        addSchedule(schedule);
        return schedule;
    }

    return(
        <>
            <td colSpan={1} className="bg-secondary"/>
            <td colSpan={1} className="bg-secondary rounded-0">
                {CreateSchedule().body}
            </td>
            <td colSpan={1} className="bg-secondary">
                <SaveButton onSave={onSave} onFailure={() => console.log("error")} onReturn={onReturn}/>
            </td>
        </>
    )
}

function ScheduleRow({schedule, onEdit, onDelete}: {schedule: Schedule, onEdit: (toEdit: Schedule) => void, onDelete: (toDelete: Schedule) => void}): ReactElement {    
    
    const scheduleRowIndex = <ScheduleRowIndex 
        schedule={schedule} 
        onDelete={onDelete} 
        onDetails={() => console.log("details")} 
        onEdit={() => {if(_state === State.Default) updateRow(State.Edit)}}/>;
    const scheduleRowEdit = <ScheduleRowEdit 
        schedule={schedule} 
        onReturn={() => updateRow(State.Default)}
        onSuccess={onEdit} 
        onFailure={() => console.log("error")}/>;

    const [row, setRow] = useState<ReactElement>(scheduleRowIndex);

    function updateRow(changeTo: State){
        _state = changeTo;
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

function SaveButton({onSave, onReturn, onFailure}: {onSave: () => Schedule | null, onReturn: VoidFunction, onFailure: VoidFunction}): ReactElement {
    return(
        <div className="btn-group float-end">
            <button onClick={onReturn} className="btn btn-outline-danger">Back</button>
            <button onClick={() => {onSave() !== null ? onReturn() : onFailure()}} className="btn btn-outline-success">
                Save
            </button>
        </div>);
}
// #endregion

export default ScheduleMain;
export const link: PageLink = {name: "Schedules", path: "/schedule", element: <ScheduleMain/>, icon: <GrSchedules/>};