import { ReactElement } from "react";
import { MdBedroomParent } from "react-icons/md";
import Schedule from "../../models/Schedule";
import PageLink from "../../types/PageLink";
import Room from "../../models/Room";

function RoomIndexBody({rooms}: {rooms: Room[]}): ReactElement {
    return(<></>);
}

function getRooms(schedule: Schedule): Room[] {
    const schedules: Schedule[] = [
        {id: 1, name: "Schedule 1", reservations: [], rooms: []},
        {id: 2, name: "Schedule 2", reservations: [], rooms: []},
        {id: 3, name: "Schedule 3", reservations: [], rooms: []}];
    return(schedules.find((s) => s.id === schedule.id)?.rooms ?? []);
}

export default function RoomIndex(schedule: Schedule): {body: ReactElement, link: PageLink }{
    return({
        body: <RoomIndexBody rooms={getRooms(schedule)}/>,
        link: {
            icon: <MdBedroomParent />,
            path: '/room',
            element: <></>,
            name: 'Rooms'
        }
    });
}