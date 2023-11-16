import { ReactElement, createRef } from "react";
import Schedule from "../../models/Schedule";

// TODO Remove
let id = 4;


const refNameInput = createRef<HTMLInputElement>();
// TODO | when the create action is called from the api show the error given by the api on screen as a tooltip on the input field with a red border, also return false.
// in the case that it is successfully added return true;
function Action() : Schedule | null {
    return({id: id++, name: refNameInput.current?.value ?? "", reservations: [], rooms: []});
}

function Body(): ReactElement {
    return(<>
        <div className="bg-secondary p-0">
            <input ref={refNameInput} placeholder="Name" type="text" className="form-control bg-secondary border-primary"/>
        </div>
    </>);
}

export default function ScheduleCreate(): {body: ReactElement, action: () => Schedule | null} {
    return({body: <Body/>, action: Action});
}