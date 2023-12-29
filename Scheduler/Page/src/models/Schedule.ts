import Reservation from "./Reservation";
import Room from "./Room";

interface Schedule{
    id: number | undefined;
    name: string | undefined;
    rooms: Room[] | undefined;
    reservations: Reservation[] | undefined;
}

export default Schedule;