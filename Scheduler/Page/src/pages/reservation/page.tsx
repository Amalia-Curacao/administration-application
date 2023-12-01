import { Fragment, ReactElement, RefObject, createRef } from "react";
import Reservation from "../../models/Reservation";
import BookingSource from "../../models/BookingSource";
import RoomType from "../../models/RoomType";
import "../../scss/reservation.create.scss";
import { toDateOnlyString, toTimeOnlyString } from "../../extensions/Date";

const checkInRef = createRef<HTMLInputElement>();
const checkOutRef = createRef<HTMLInputElement>();
const flightArrivalNumberRef = createRef<HTMLInputElement>();
const flightDepartureNumberRef = createRef<HTMLInputElement>();
const flightArrivalTimeRef = createRef<HTMLInputElement>();
const flightDepartureTimeRef = createRef<HTMLInputElement>();
const bookingSourceRef = createRef<HTMLSelectElement>();
const remarksRef = createRef<HTMLTextAreaElement>();

const references: {[key: string]: RefObject<undefined>} = { };

function References(key: string): RefObject<undefined> {
    return (references[key] = references[key] || createRef());
}

function Body({reservation}: {reservation: Reservation}): ReactElement {
    return (<>
        <table>
            <thead hidden={true}>
                <th/>
                <th/>
            </thead>
            <tbody >
                <tr>
                    <td colSpan={2} className="bg-primary text-secondary">
                        <label className="d-flex flex-column">Booking Source
                            <select ref={References("booking-source") as unknown as RefObject<HTMLSelectElement>} defaultValue={reservation.bookingSource ? reservation.bookingSource : BookingSource.None } className="form-control">
                                {Object.values(BookingSource).map((value, index) => Number(value) === 0 || Number(value)
                                ? <Fragment key={index}/> 
                                : <option key={index} value={value}>{value}</option>)}
                            </select>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Check In
                            <input ref={References("check-in") as unknown as RefObject<HTMLInputElement>} defaultValue={reservation.checkIn ? toDateOnlyString(reservation.checkIn) : ""} type="date" className="form-control"/>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label>Check Out
                            <input ref={References("check-out") as unknown as RefObject<HTMLInputElement>} defaultValue={reservation.checkOut ? toDateOnlyString(reservation.checkOut) : ""} type="date" className="form-control"/>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Flight Arrival Number
                            <input defaultValue={reservation.flightArrivalNumber ? reservation.flightArrivalNumber : ""} ref={References("flight-arrival-number") as unknown as RefObject<HTMLInputElement>} type="text" className="form-control"/>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary text-nowrap">
                        <label>Flight Departure Number
                            <input defaultValue={reservation.flightDepartureNumber ? reservation.flightDepartureNumber : ""} ref={References("flight-departure-number") as unknown as RefObject<HTMLInputElement>} type="text" className="form-control"/>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Flight Arrival Time
                            <input defaultValue={reservation.flightArrivalTime ? toTimeOnlyString(reservation.flightArrivalTime) : ""} ref={References("flight-arrival-time") as unknown as RefObject<HTMLInputElement>} type="time" className="form-control"/>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label>Flight Departure Time
                            <input defaultValue={reservation.flightDepartureTime ? toTimeOnlyString(reservation.flightDepartureTime) : ""} ref={References("flight-departure-time") as unknown as RefObject<HTMLInputElement>} type="time" className="form-control"/>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td colSpan={2} className="bg-primary text-secondary">
                        <label className="d-flex flex-column">Remarks
                            <textarea defaultValue={reservation.remarks ? reservation.remarks : ""} className="form-control" ref={References("remarks") as unknown as RefObject<HTMLTextAreaElement>} />
                        </label>
                    </td>
                </tr>
            </tbody>
        </table>
    </>);
}

function Action(scheduleId: number, roomNumber: number, roomType: RoomType): boolean {
    const reservationToAdd: Reservation = {
        id: -1,
        checkIn: new Date(checkInRef.current?.value!),
        checkOut: new Date(checkOutRef.current?.value!),
        flightArrivalNumber: flightArrivalNumberRef.current?.value!,
        flightDepartureNumber: flightDepartureNumberRef.current?.value!,
        flightArrivalTime: new Date(flightArrivalTimeRef.current?.value!),
        flightDepartureTime: new Date(flightDepartureTimeRef.current?.value!),
        bookingSource: Number(bookingSourceRef.current?.value!),
        remarks: remarksRef.current?.value!,

        scheduleId: scheduleId,
        roomNumber: roomNumber,
        schedule: undefined,

        roomType: roomType,
        roomScheduleId: scheduleId,
        room: undefined,
        persons: [],
        personIds: []
    };

    return (Validate(reservationToAdd));
}

function Validate(reservation: Reservation): boolean {
    console.log((References("check-in") as unknown as RefObject<HTMLInputElement>).current!.labels);
    // #region undefined/null validation
    
    return (true);
}

export default function Page(reservation: Reservation): {body: ReactElement, action: () => boolean} {
    if(!reservation.scheduleId) throw new Error("Reservation scheduleId is undefined");
    if(!reservation.roomNumber) throw new Error("Reservation roomNumber is undefined");
    if(!reservation.roomType) throw new Error("Reservation roomType is undefined");

    return ({body: <Body reservation={reservation}/>, action: () => Action(reservation.scheduleId!, reservation.roomNumber!, reservation.roomType!)});
}

