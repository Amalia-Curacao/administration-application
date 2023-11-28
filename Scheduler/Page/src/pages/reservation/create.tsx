import { ReactElement, createRef } from "react";
import Reservation from "../../models/Reservation";
import BookingSource from "../../models/BookingSource";
import RoomType from "../../models/RoomType";
import "../../scss/reservation.create.scss";

const checkInRef = createRef<HTMLInputElement>();
const checkOutRef = createRef<HTMLInputElement>();
const flightArrivalNumberRef = createRef<HTMLInputElement>();
const flightDepartureNumberRef = createRef<HTMLInputElement>();
const flightArrivalTimeRef = createRef<HTMLInputElement>();
const flightDepartureTimeRef = createRef<HTMLInputElement>();
const bookingSourceRef = createRef<HTMLSelectElement>();
const remarksRef = createRef<HTMLInputElement>();

function Body({checkIn}: {checkIn: Date | undefined}): ReactElement {
    if(!checkIn) checkIn = new Date();
    return (<>
        <table className="d-flex flex-fill">
            <tbody >
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Check In</label>
                        <input ref={checkInRef} defaultValue={checkIn.toDateString()} type="date" className="form-control"/>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label>Check Out</label>
                        <input ref={checkOutRef} type="date" className="form-control"/>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Flight Arrival Number</label>
                        <input ref={flightArrivalNumberRef} type="text" className="form-control"/>
                    </td>
                    <td className="bg-primary text-secondary text-nowrap">
                        <label>Flight Departure Number</label>
                        <input ref={flightDepartureNumberRef} type="text" className="form-control"/>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Flight Arrival Time</label>
                        <input ref={flightArrivalTimeRef} type="time" className="form-control"/>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label>Flight Departure Time</label>
                        <input ref={flightDepartureTimeRef} type="time" className="form-control"/>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Remarks</label>
                        <input type="text" className="form-control"/>
                    </td>
                    <td className="bg-primary text-secondary">
                    <label>Booking Source</label>
                    <select ref={bookingSourceRef} className="form-control">
                        {Object.values(BookingSource).map((value, index) => Number(value) ?
                        <></> 
                        : <option key={index} value={value} selected={index === 0}>{value}</option>)}
                    </select>
                    </td>
                </tr>
            </tbody>
        </table>
    </>);
}

function Action(scheduleId: number, roomNumber: number, roomType: RoomType): string | Reservation {
    console.log("request");
    console.log(bookingSourceRef.current?.value);
    return ({
        id: -1,
        checkIn: new Date(checkInRef.current?.value ?? ""),
        checkOut: new Date(checkOutRef.current?.value ?? ""),
        flightArrivalNumber: flightArrivalNumberRef.current?.value ?? "",
        flightDepartureNumber: flightDepartureNumberRef.current?.value ?? "",
        flightArrivalTime: new Date(flightArrivalTimeRef.current?.value ?? ""),
        flightDepartureTime: new Date(flightDepartureTimeRef.current?.value ?? ""),
        bookingSource: null,
        remarks: remarksRef.current?.value ?? "",

        scheduleId: scheduleId,
        roomNumber: roomNumber,
        schedule: null,

        roomType: roomType,
        roomScheduleId: scheduleId,
        room: null,
      });
}

export default function ReservationCreate(scheduleId: number, roomNumber: number, roomType: number, checkInDate: Date | undefined): {body: ReactElement, action: () => string | Reservation} {
    return({body: <Body checkIn={checkInDate}/>, action: () => Action(scheduleId, roomNumber, roomType)});
}