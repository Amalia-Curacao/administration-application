import { ReactElement, useEffect, useState } from "react";
import { GrSchedules } from "react-icons/gr";
import SaveButton from "../../components/saveButton";
import "../../extensions/HTMLElement";
import Schedule from "../../models/Schedule";
import PageLink from "../../types/PageLink";
import PageState from "../../types/PageState";
import CreateSchedule from "./create";
import ScheduleRow from "./row";
import PageTitle from "../../components/pageTitle";
import axios from "axios";

const _info = {name: "Schedules", icon: <GrSchedules/>};

function ScheduleMain(): ReactElement {   
    function createSchedule(schedule: Schedule): void {
        axios.get(process.env.REACT_APP_API_URL + "/Schedules/Create/" + schedule.name)
        .then(res => res.status === 200 
            ? setSchedules([...schedules, res.data as Schedule]) 
            : console.log(res));
    }

    function getSchedules(): void {
        console.log(process.env.REACT_APP_API_URL);
        axios.get(process.env.REACT_APP_API_URL + "/Schedules/Get")
        .then(res => res.status === 200 
            ? setSchedules(res.data as Schedule[])
            : console.log(res));
    }

    function deleteSchedule(schedule: Schedule): void {
        axios.delete(process.env.REACT_APP_API_URL + "/Schedules/Delete/" + schedule.id)
        .then(res => {
            res.status === 200 && Boolean(res) ? setSchedules(schedules.filter(s => s.id !== schedule.id)) : console.log(res);
        })
    }

    function editSchedule(schedule: Schedule): void{
        console.log(schedule);
        axios.post(process.env.REACT_APP_API_URL + "/Schedules/Edit/", {id: schedule.id, name: schedule.name})
        .then(res => {
            res.status === 200 
            ? setSchedules(schedules.map(s => s.id === schedule.id ? schedule : s)) 
            : console.log(res);
        })
    }

    let _state: PageState = PageState.Default;
    const [schedules, setSchedules] = useState<Schedule[]>([]);
    useEffect(() => getSchedules(), []);
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
            editSchedule(toEdit);
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
                        <ScheduleRowCreate addSchedule={(schedule) => createSchedule(schedule)} onReturn={onReturn}/>
                    </tr>

                    {schedules.map(schedule => <ScheduleRow key={schedule.id} schedule={schedule} onEdit={onEdit} onDelete={deleteSchedule}/>)}
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
        function onSave(): Schedule | undefined{
            const schedule = CreateSchedule().action();
            if(!schedule) return undefined;
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