import Reservation from "./Reservation";
import RoomType from "./RoomType";
import Schedule from "./Schedule";

export default interface Room{
    number: number | null;
    type: RoomType | null;
    floor: number | null;
    scheduleId: number | null;
    schedule: Schedule | null;
    reservations: Reservation[] | null;
}