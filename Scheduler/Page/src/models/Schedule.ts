import Reservation from "./Reservation";
import Room from "./Room";

interface Schedule{
    id: number | null;
    name: string | null;
    rooms: Room[] | null;
    reservations: Reservation[] | null;
}

export default Schedule;