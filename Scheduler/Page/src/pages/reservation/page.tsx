import { ReactElement } from "react";
import Reservation from "../../models/Reservation";
import BookingSource from "../../models/BookingSource";
import RoomType from "../../models/RoomType";
import "../../scss/reservation.create.scss";
import { toDateOnlyString, toTimeOnlyString } from "../../extensions/Date";
import References from "../../tools/References";
import InputField from "../../components/inputField";
import Guest from "../../models/Guest";

const references: References = new References();

function Body({reservation}: {reservation: Reservation}): ReactElement {
    return (<>
        <table className="w-100"    >
            <tbody>
                <tr>
                    <td colSpan={2} className="bg-primary text-secondary">
                        <label className="d-flex flex-column w-100">Booking Source
                                <InputField refKey="booking-source" references={references}>
                                    <select onChange={updateBookingSource} ref={references.GetSelect("booking-source")} defaultValue={reservation.bookingSource ? reservation.bookingSource : BookingSource.None } className="form-control">
                                        {Object.values(BookingSource).map((value, index) => <option key={index} value={value}>{value}</option>)}
                                    </select>
                                </InputField>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label className="w-100">Check In
                            <InputField refKey="check-in" references={references}>
                                <input onChange={updateCheckIn} ref={references.GetInput("check-in")} defaultValue={!reservation.checkIn ? "" : toDateOnlyString(reservation.checkIn!)} type="date" className="form-control"/>
                            </InputField>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label className="w-100">Check Out
                            <InputField refKey="check-out" references={references}>
                                <input onChange={updateCheckOut} ref={references.GetInput("check-out")} defaultValue={!reservation.checkOut ? "" : toDateOnlyString(reservation.checkOut!)} type="date" className="form-control"/>
                            </InputField>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label className="w-100">Flight Arrival Number
                            <InputField refKey="flight-arrival-number" references={references}>
                                <input onChange={updateFlightArrivalNumber} defaultValue={reservation.flightArrivalNumber ? reservation.flightArrivalNumber : ""} ref={references.GetInput("flight-arrival-number")} type="text" className="form-control"/>
                            </InputField>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary text-nowrap">
                        <label className="w-100">Flight Departure Number
                            <InputField refKey="flight-departure-number" references={references}>
                                <input onChange={updateFlightDepartureNumber} defaultValue={reservation.flightDepartureNumber ? reservation.flightDepartureNumber : ""} ref={references.GetInput("flight-departure-number")} type="text" className="form-control"/>
                            </InputField>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label className="w-100">Flight Arrival Time
                            <InputField refKey="flight-arrival-time" references={references}>
                                <input onChange={updateFlightArrivalTime} defaultValue={!reservation.flightArrivalTime ? "" : toTimeOnlyString(reservation.flightArrivalTime!)} ref={references.GetInput("flight-arrival-time")} type="time" className="form-control"/>
                            </InputField>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label className="w-100">Flight Departure Time
                            <InputField refKey="flight-departure-time" references={references}>
                                <input onChange={updateFlightDepartureTime} defaultValue={!reservation.flightDepartureTime ? "" : toTimeOnlyString(reservation.flightDepartureTime!)} ref={references.GetInput("flight-departure-time")} type="time" className="form-control"/>
                            </InputField>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td colSpan={2} className="bg-primary text-secondary">
                        <label className="d-flex flex-column w-100">Remarks
                            <InputField refKey="remarks" references={references}>
                                <textarea onChange={updateRemarks} defaultValue={reservation.remarks ? reservation.remarks : ""} className="form-control" ref={references.GetTextArea("remarks")} />
                            </InputField>
                        </label>
                    </td>
                </tr>
            </tbody>
        </table>
    </>);

    // #region update functions
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
    // #endregion
}

function Action(scheduleId: number, roomNumber: number, roomType: RoomType, reservationId: number, people: Guest[]): Reservation | undefined {
    const reservationToAdd: Reservation = {
        id: reservationId,
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
        guests: people ?? []
    };

    return (Validate(reservationToAdd) ? reservationToAdd : undefined);
}

function Validate(reservation: Reservation): boolean {
    let isValid: boolean = true;

    const nullErrorMessage: string = "Required";
    if(!reservation.checkIn || isNaN(reservation.checkIn!.valueOf())) {
        references.GetSpan("check-in-validation")!.current!.innerText = nullErrorMessage;
        references.GetDiv("check-in-wrapper")!.current!.classList.add("border-danger");
        isValid = false;
    };
    if(!reservation.checkOut || isNaN(reservation.checkOut!.valueOf())) {
        references.GetSpan("check-out-validation")!.current!.innerText = nullErrorMessage;
        references.GetDiv("check-out-wrapper")!.current!.classList.add("border-danger");
        isValid = false;
    };
    
    if(reservation.bookingSource === BookingSource.None) {
        references.GetSpan("booking-source-validation")!.current!.innerText = "Bookingsource cannot be none.";
        references.GetDiv("booking-source-wrapper")!.current!.classList.add("border-danger");
        isValid = false;
    }

    if(reservation.checkIn! > reservation.checkOut!) {
        references.GetSpan("check-in-validation")!.current!.innerText = "Check in cannot be after check out.";
        references.GetDiv("check-in-wrapper")!.current!.classList.add("border-danger");
        isValid = false;
    }

    return (isValid);
}

export default function Page(reservation: Reservation): {body: ReactElement, action: () => Reservation | undefined} {
    if(!reservation.scheduleId) throw new Error("Reservation scheduleId is undefined");
    if(!reservation.roomNumber) throw new Error("Reservation roomNumber is undefined");
    if(!reservation.roomType) throw new Error("Reservation roomType is undefined");
    return ({
        body: <Body reservation={reservation}/>, 
        action: () => Action(reservation.scheduleId!, reservation.roomNumber!, reservation.roomType!, reservation.id!, reservation.guests!)
    });
}

