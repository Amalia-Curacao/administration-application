import BookingSource from "./BookingSource";
import Room from "./Room";
import RoomType from "./RoomType";
import Schedule from "./Schedule";

export default interface Reservation{
    id: number | null;
    checkIn: Date | null;
    checkOut: Date | null;
    flightArrivalNumber: string | null;
    flightDepartureNumber: string | null;
    flightArrivalTime: Date | null;
    flightDepartureTime: Date | null;
    bookingSource: BookingSource | null;
    remarks: string | null;

    roomNumber: number | null;
    roomType: RoomType | null;
    roomScheduleId: number | null;
    room: Room | null;
    scheduleId: number | null;
    schedule: Schedule | null;
}