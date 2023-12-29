import BookingSource from "../models/BookingSource";
import Guest from "../models/Guest";
import PersonPrefix from "../models/PersonPrefix";
import Reservation from "../models/Reservation";
import RoomType from "../models/RoomType";
import { toDateOnlyString } from "./Date";
// Purpose
// ToJson: converts an object to a json string
// ToJsonReservation: converts a reservation object to a json string
// ToJsonRoomType: converts a room type to a json string
// ToJsonBookingSource: converts a booking source to a json string

// Cant figure out how to typeguard interfaces
/* export function ToJson(object: any): any{
    if(object as Reservation) return ToJsonReservation(object as Reservation);
    if(object as RoomType) return ToJsonRoomType(object);
    if(object as BookingSource) return ToJsonBookingSource(object);
    if(object as Guest[]) return object.map((guest: Guest) => ToJsonGuest(guest));
    if(object as Guest) return ToJsonGuest(object);
    throw new Error("Object is not implemented");
} */

export function ToJsonReservation(reservation: Reservation): any{
    return {
        id: reservation.id,
        guests: undefined,
        checkIn: !reservation.checkIn ? undefined : toDateOnlyString(reservation.checkIn), 
        checkOut: !reservation.checkOut ? undefined : toDateOnlyString(reservation.checkOut),
        room: reservation.room,
        roomNumber: reservation.roomNumber,
        roomType: ToJsonRoomType(reservation.roomType!), 
        bookingSource: ToJsonBookingSource(reservation.bookingSource!),
        flightArrivalNumber: reservation.flightArrivalNumber,
        flightArrivalTime: reservation.flightArrivalTime,
        flightDepartureNumber: reservation.flightDepartureNumber,
        flightDepartureTime: reservation.flightDepartureTime,
        schedule: reservation.schedule,
        scheduleId: reservation.scheduleId,
        remarks: reservation.remarks,
    };
}

export function ToJsonRoomType(roomType: RoomType): number{
    switch(roomType){
        case RoomType.None:
            return 0;
        case RoomType.Apartment:
            return 1;
        case RoomType.Room:
            return 2;
        default:
            console.log(roomType);
            throw new Error("RoomType is not implemented");
    }
}

export function ToJsonBookingSource(bookingSource: BookingSource): number{
    switch(bookingSource){
        case BookingSource.None:
            return 0;
        case BookingSource.Tui:
            return 1;
        case BookingSource.BookingDotCom:
            return 2;
        case BookingSource.Expedia:
            return 3;
        case BookingSource.Airbnb:
            return 4;
        case BookingSource.Direct:
            return 5;
        case BookingSource.Despegar:
            return 6;
        default:
            throw new Error("BookingSource is not implemented");
    }
}

export function ToJsonGuest(guest: Guest): any {
    return{
        id: guest.id,
        prefix: ToJsonPrefix(guest.prefix ?? PersonPrefix.Unknown),
        firstName: guest.firstName,
        lastName: guest.lastName,
        age: guest.age,
        note: guest.note,

        reservationId: guest.reservationId,
        reservation: undefined,
    }
}

export function ToJsonPrefix(prefix: PersonPrefix): number{
    switch(prefix){
        case PersonPrefix.Unknown:
            return 0;
        case PersonPrefix.Mr:
            return 1;
        case PersonPrefix.Mrs:
            return 2;
        case PersonPrefix.Ms:
            return 3;
        case PersonPrefix.Other:
            return 4;
        default:
            throw new Error("Prefix is not implemented");
    }
}
