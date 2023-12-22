import BookingSource from "../models/BookingSource";
import Reservation from "../models/Reservation";
import RoomType from "../models/RoomType";
import { toDateOnlyString } from "./Date";
// Purpose
// ToJson: converts an object to a json string
// ToJsonReservation: converts a reservation object to a json string
// ToJsonRoomType: converts a room type to a json string
// ToJsonBookingSource: converts a booking source to a json string


export function ToJson(object: any): any{
    if(object as Reservation) return ToJsonReservation(object);
    if(object as RoomType) return ToJsonRoomType(object);
    if(object as BookingSource) return ToJsonBookingSource(object);
    throw new Error("Object is not implemented");
}

function ToJsonReservation(reservation: Reservation): any{
    return {
        id: reservation.id,
        guests: reservation.guests,
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

function ToJsonRoomType(roomType: RoomType): number{
    switch(roomType){
        case RoomType.None:
            return 0;
        case RoomType.Apartment:
            return 1;
        case RoomType.Room:
            return 2;
        default:
            throw new Error("RoomType is not implemented");
    }
}

function ToJsonBookingSource(bookingSource: BookingSource): number{
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