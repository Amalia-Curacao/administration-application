import { ReactElement } from "react";
import Schedule from "../../models/Schedule";

function Body({schedule} : {schedule: Schedule}): ReactElement {
    return(<>
        <th colSpan={1} scope="col" className="bg-secondary">
            {schedule.id} 
        </th>
        <th colSpan={1} scope="col" className="bg-secondary">
            <input type="text" readOnly className="bg-transparent form-control shadow-none border-0" value={!schedule.name ? "" : schedule.name} />
        </th>
    </>);
}

function Action(): ReactElement {return(<></>)}


export default function ScheduleIndex(schedule: Schedule): {body: ReactElement, action: ReactElement} {
    return({body: Body({schedule: schedule}), action: Action()});
};