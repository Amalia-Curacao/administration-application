import { ReactElement } from "react";
import Reservation from "../../models/Reservation";
import BookingSource from "../../models/BookingSource";
import RoomType from "../../models/RoomType";
import "../../scss/reservation.create.scss";
import { toDateOnlyString, toTimeOnlyString } from "../../extensions/Date";
import References from "../../tools/References";

const references: References = new References();

function Body({reservation}: {reservation: Reservation}): ReactElement {
    return (<>
        <table>
            <tbody>
                <tr>
                    <td colSpan={2} className="bg-primary text-secondary">
                        <label className="d-flex flex-column">Booking Source
                            <select onChange={updateBookingSource} ref={references.GetSelect("booking-source")} defaultValue={reservation.bookingSource ? reservation.bookingSource : BookingSource.None } className="form-control">
                                {Object.values(BookingSource).map((value, index) => <option key={index} value={value}>{value}</option>)}
                            </select>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Check In
                            <input onChange={updateCheckIn} ref={references.GetInput("check-in")} defaultValue={reservation.checkIn ? toDateOnlyString(reservation.checkIn) : ""} type="date" className="form-control"/>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label>Check Out
                            <input onChange={updateCheckOut} ref={references.GetInput("check-out")} defaultValue={reservation.checkOut ? toDateOnlyString(reservation.checkOut) : ""} type="date" className="form-control"/>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Flight Arrival Number
                            <input onChange={updateFlightArrivalNumber} defaultValue={reservation.flightArrivalNumber ? reservation.flightArrivalNumber : ""} ref={references.GetInput("flight-arrival-number")} type="text" className="form-control"/>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary text-nowrap">
                        <label>Flight Departure Number
                            <input onChange={updateFlightDepartureNumber} defaultValue={reservation.flightDepartureNumber ? reservation.flightDepartureNumber : ""} ref={references.GetInput("flight-departure-number")} type="text" className="form-control"/>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Flight Arrival Time
                            <input onChange={updateFlightArrivalTime} defaultValue={reservation.flightArrivalTime ? toTimeOnlyString(reservation.flightArrivalTime) : ""} ref={references.GetInput("flight-arrival-time")} type="time" className="form-control"/>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label>Flight Departure Time
                            <input onChange={updateFlightDepartureTime} defaultValue={reservation.flightDepartureTime ? toTimeOnlyString(reservation.flightDepartureTime) : ""} ref={references.GetInput("flight-departure-time")} type="time" className="form-control"/>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td colSpan={2} className="bg-primary text-secondary">
                        <label className="d-flex flex-column">Remarks
                            <textarea onChange={updateRemarks} defaultValue={reservation.remarks ? reservation.remarks : ""} className="form-control" ref={references.GetTextArea("remarks")} />
                        </label>
                    </td>
                </tr>
            </tbody>
        </table>
    </>);

    function updateBookingSource(): void {
        reservation.bookingSource = BookingSource[references.GetSelect("booking-source")!.current?.value! as keyof typeof BookingSource];
    }

    function updateCheckIn(): void {
        reservation.checkIn = new Date(references.GetInput("check-in")!.current?.value!);
    }

    function updateCheckOut(): void {
        reservation.checkOut = new Date(references.GetInput("check-out")!.current?.value!);
    }

    function updateFlightArrivalNumber(): void {
        reservation.flightArrivalNumber = references.GetInput("flight-arrival-number")!.current?.value!;
    }

    function updateFlightDepartureNumber(): void {
        reservation.flightDepartureNumber = references.GetInput("flight-departure-number")!.current?.value!;
    }

    function updateFlightArrivalTime(): void {
        reservation.flightArrivalTime = new Date(references.GetInput("flight-arrival-time")!.current?.value!);
    }

    function updateFlightDepartureTime(): void {
        reservation.flightDepartureTime = new Date(references.GetInput("flight-departure-time")!.current?.value!);
    }

    function updateRemarks(): void {
        reservation.remarks = references.GetTextArea("remarks")!.current?.value!;
    }
}

function Action(scheduleId: number, roomNumber: number, roomType: RoomType): boolean {
    const reservationToAdd: Reservation = {
        id: -1,
        checkIn: new Date(references.GetInput("check-in")!.current?.value!),
        checkOut: new Date(references.GetInput("check-out")!.current?.value!),
        flightArrivalNumber: references.GetInput("flight-arrival-number")!.current?.value!,
        flightDepartureNumber: references.GetInput("flight-departure-number")!.current?.value!,
        flightArrivalTime: new Date(references.GetInput("flight-arrival-time")!.current?.value!),
        flightDepartureTime: new Date(references.GetInput("flight-departure-time")!.current?.value!),
        bookingSource: BookingSource[references.GetSelect("booking-source")!.current?.value! as keyof typeof BookingSource],
        remarks: references.GetInput("remarks")!.current?.value!,

        scheduleId: scheduleId,
        roomNumber: roomNumber,
        schedule: undefined,

        roomType: roomType,
        roomScheduleId: scheduleId,
        room: undefined,
        people: [],
        peopleIds: []
    };

    return (Validate(reservationToAdd));
}

function Validate(reservation: Reservation): boolean {
    console.log(references.GetInput("check-in").current!.labels);
    // #region undefined/null validation
    return (true);
}

export default function Page(reservation: Reservation): {body: ReactElement, action: () => boolean} {
    if(!reservation.scheduleId) throw new Error("Reservation scheduleId is undefined");
    if(!reservation.roomNumber) throw new Error("Reservation roomNumber is undefined");
    if(!reservation.roomType) throw new Error("Reservation roomType is undefined");

    return ({body: <Body reservation={reservation}/>, action: () => Action(reservation.scheduleId!, reservation.roomNumber!, reservation.roomType!)});
}

