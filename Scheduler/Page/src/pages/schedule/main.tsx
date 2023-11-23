import { ReactElement, useState } from "react";
import { GrSchedules } from "react-icons/gr";
import SaveButton from "../../components/saveButton";
import "../../extensions/HTMLElement";
import Schedule from "../../models/Schedule";
import PageLink from "../../types/PageLink";
import PageState from "../../types/PageState";
import CreateSchedule from "./create";
import ScheduleRow from "./row";
import PageTitle from "../../components/pageTitle";

const _info = {name: "Schedules", icon: <GrSchedules/>};
function ScheduleMain(): ReactElement {   
    let _state: PageState = PageState.Default;
    const testSchedules: Schedule[] = [
        {id: 1, name: "test1", reservations: [], rooms: []}, 
        {id: 2, name: "test2", reservations: [], rooms: []}, 
        {id: 3, name: "test3", reservations: [], rooms: []}];
    const [schedules, setSchedules] = useState<Schedule[]>(testSchedules);
        
    function Table(): ReactElement {
        const [creating, setCreating] = useState<boolean>(false);
        
        // #region Actions
        function createState(){
            if(_state !== PageState.Default) return;
            _state = PageState.Create;
            setCreating(true);
        }

        function onEdit(toEdit: Schedule) {
            if(_state !== PageState.Default) return;
            _state = PageState.Edit;
            setSchedules(schedules.map(s => s.id === toEdit.id ? toEdit : s));
        }

        function onDelete(toDelete: Schedule) {
            setSchedules(schedules.filter(s => s.id !== toDelete.id));
        }

        function onAdd(schedule: Schedule) {
            setSchedules([...schedules, schedule]);
        }

        function onReturn() {
            _state = PageState.Default; 
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

    return(<>
        <div className="p-3 pe-3 d-flex flex-column flex-fill">
            <PageTitle name={_info.name} icon={_info.icon}/>
            <Table/>
        </div>
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

export default ScheduleMain;
export const link: PageLink = {route: "/schedule", element: <ScheduleMain/>, icon: _info.icon};