import { ReactElement, createRef } from "react";
import Schedule from "../../models/Schedule";

const refNameInput = createRef<HTMLInputElement>();
function Action() : Schedule | undefined {
    return({id: -1, name: refNameInput.current?.value ?? "", reservations: [], rooms: []});
}

function Body(): ReactElement {
    return(<>
        <div className="bg-secondary p-0">
            <input ref={refNameInput} placeholder="Name" type="text" className="form-control bg-secondary border-primary"/>
        </div>
    </>);
}

export default function ScheduleCreate(): {body: ReactElement, action: () => Schedule | undefined} {
    return({body: <Body/>, action: Action});
}