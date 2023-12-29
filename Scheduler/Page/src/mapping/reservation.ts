import { DateOnlyToDateTime } from "../extensions/Date";
import Reservation from "../models/Reservation";

export default function Map(reservation: Reservation): Reservation{
    return {
        ...reservation,
        checkIn: new Date(DateOnlyToDateTime(reservation.checkIn!)),
        checkOut: new Date(DateOnlyToDateTime(reservation.checkOut!))
    }
}

export function MapAll(reservations: Reservation[]): Reservation[] {
    return reservations.map(Map);
}