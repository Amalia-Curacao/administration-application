import BookingSource from "./BookingSource";
import Room from "./Room";
import RoomType from "./RoomType";
import Schedule from "./Schedule";
import Person from "./Person";

export default interface Reservation{
    id: number | undefined;
    checkIn: Date | undefined;
    checkOut: Date | undefined;
    flightArrivalNumber: string | undefined;
    flightDepartureNumber: string | undefined;
    flightArrivalTime: Date | undefined;
    flightDepartureTime: Date | undefined;
    bookingSource: BookingSource | undefined;
    remarks: string | undefined;

    roomNumber: number | undefined;
    roomType: RoomType | undefined;
    roomScheduleId: number | undefined;
    room: Room | undefined;
    scheduleId: number | undefined;
    schedule: Schedule | undefined;
    persons: Person[] | undefined;
    personIds: number[] | undefined;
}