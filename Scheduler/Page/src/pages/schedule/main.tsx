import { ReactElement, useState } from "react";
import { GrSchedules } from "react-icons/gr";
import SaveButton from "../../components/saveButton";
import "../../extensions/HTMLElement";
import Schedule from "../../models/Schedule";
import PageLink from "../../types/PageLink";
import { PageState as State } from "../../types/PageState";
import CreateSchedule from "./create";
import ScheduleRow from "./row";

function ScheduleMain(): ReactElement {    

    // #region Variables
    // Variables are written are camelCasing and start with an underscore.
    let _state: State = State.Default;
    const testSchedules: Schedule[] = [
        {id: 1, name: "test1", reservations: [], rooms: []}, 
        {id: 2, name: "test2", reservations: [], rooms: []}, 
        {id: 3, name: "test3", reservations: [], rooms: []}];
    const [schedules, setSchedules] = useState<Schedule[]>(testSchedules);
    // #endregion

    return(<>
        <div className="p-3 pe-3 d-flex flex-column flex-fill">
            <PageTitle/>
            <Table/>
        </div>
    </>);
    
    
    // #region Elements
    // Elements are written in PascalCasing and are always functions.
    function PageTitle(): ReactElement { 
        return(
        <div className="d-flex justify-content-between">
            <h1>Schedules</h1>
            <div className="h1">{link.icon}</div>
        </div>);
    }
        
    function Table(): ReactElement {
        const [creating, setCreating] = useState<boolean>(false);
        
        function createState(){
            if(_state !== State.Default) return;
            _state = State.Create;
            setCreating(true);
        }

        // #region Actions
        function onEdit(toEdit: Schedule) {
            if(_state !== State.Default) return;
            _state = State.Edit;
            setSchedules(schedules.map(s => s.id === toEdit.id ? toEdit : s));
        }

        function onDelete(toDelete: Schedule) {
            setSchedules(schedules.filter(s => s.id !== toDelete.id));
        }

        function onAdd(schedule: Schedule) {
            setSchedules([...schedules, schedule]);
        }

        function onReturn() {
            _state = State.Default; 
            setCreating(false);
        }
        // #endregion

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
                        <ScheduleRowCreate addSchedule={onAdd} onReturn={onReturn}/>
                    </tr>

                    {schedules.map(schedule => <ScheduleRow key={schedule.id} schedule={schedule} onEdit={onEdit} onDelete={onDelete}/>)}
                </tbody>        
            </table>
        </>);
    }

    function ScheduleRowCreate({onReturn, addSchedule}: {onReturn: VoidFunction, addSchedule: (schedule: Schedule) => void}): ReactElement {
        function onSave(): Schedule | null{
            const schedule = CreateSchedule().action();
            if(schedule === null) return null;
            addSchedule(schedule);
            return schedule;
        }

        function onFailure() {
            console.log("error");
        }

        return(
            <>
                <td colSpan={1} className="bg-secondary"/>
                <td colSpan={1} className="bg-secondary rounded-0">
                    {CreateSchedule().body}
                </td>
                <td colSpan={1} className="bg-secondary">
                    <SaveButton onSave={onSave} onFailure={onFailure} onReturn={onReturn}/>
                </td>
            </>
        )
    }

    
    // #endregion
}

export default ScheduleMain;
export const link: PageLink = {name: "Schedules", path: "/schedule", element: <ScheduleMain/>, icon: <GrSchedules/>};