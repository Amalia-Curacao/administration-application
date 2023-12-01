import Reservation from "./Reservation";
import RoomType from "./RoomType";
import Schedule from "./Schedule";

export default interface Room{
    number: number | undefined;
    type: RoomType | undefined;
    floor: number | undefined;
    scheduleId: number | undefined;
    schedule: Schedule | undefined;
    reservations: Reservation[] | undefined;
}