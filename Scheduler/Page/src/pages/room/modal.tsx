import axios from "axios";
import { ReactElement, useState } from "react";
import { Modal } from "react-bootstrap";
import { ToJson } from "../../extensions/ToJson";
import BookingSource from "../../models/BookingSource";
import Guest from "../../models/Guest";
import PersonPrefix from "../../models/PersonPrefix";
import Reservation from "../../models/Reservation";
import Room from "../../models/Room";
import {default as GuestPage} from "../guest/page";
import {default as ReservationPage} from "../reservation/page";

export function CreateReservationModal({checkIn, room, show, setShow}: {checkIn: Date, room: Room, show: boolean, setShow: (b: boolean) => void}): ReactElement {
    const blankReservation: Reservation = {
        id: -1,
        checkIn: checkIn,
        checkOut: undefined,
        flightArrivalNumber: undefined,
        flightDepartureNumber: undefined,
        flightArrivalTime: undefined,
        flightDepartureTime: undefined,
        bookingSource: BookingSource.None,
        remarks: "",
        roomNumber: room.number,
        roomType: room.type,
        roomScheduleId: room.scheduleId,
        room: room,
        scheduleId: room.scheduleId,
        schedule: room.schedule,
        guests: []
    }
    const [reservation, setReservation] = useState<Reservation>(blankReservation);
    return(<>
        <ReservationModal show={show} setShow={setShow} reservation={reservation} setReservation={(r: Reservation) => setReservation(r)}/>
    </>);
}

export default function ReservationModal({reservation, show, setShow, setReservation}: {reservation: Reservation, setReservation: (r: Reservation) => void, show: boolean, setShow: (s: boolean) => void}): ReactElement {
    const [showGuest, setShowGuest] = useState<{[index: number]: boolean}>({});
    const reservationPage = ReservationPage(reservation);
    const toReservationModal = (): void => { 
        Object.keys(showGuest).forEach(key => setShowGuest(showGuest => ({...showGuest, [key]: false})));
        setShow(true);
    };
    const toGuestModal = (index: number): void => {
        setShow(false);
        setShowGuest(showGuest => ({...showGuest, [index]: true}));
    }
    const onSave = (): void => {
        const reservation = reservationPage.action();
        if(!reservation) return;
        axios.post(process.env.REACT_APP_API_URL + "/Reservations/" + (reservation.id! < 0 ? "Create" : "Edit"), ToJson(reservation))
            .then(response => {
                setReservation(response.data as Reservation);
                setShow(false);
            })
            .catch(error => console.log(error)); 
    }
    const onRemove = (): void => {
        axios.delete(process.env.REACT_APP_API_URL + "/Reservations/Delete/" + reservation.id)
        .then( response =>
            {
                if(!(response.data as boolean)) return;
                setShow(false);
            }
        )
        .catch(error => console.log(error));
    }
    return(<>
        <Modal show={show} onHide={() => setShow(false)}>
            <Modal.Body style={{borderRadius: "5px 5px 0px 0px"}} className="bg-primary">
                {reservationPage.body}
            </Modal.Body>
            <Modal.Footer style={{borderRadius: "0px 0px 5px 5px"}} className="bg-primary">
                <div className="flex flex-fill pe-3 ps-3">
                    <div className="float-start btn-group">
                        {reservation.guests?.map((p, index) => 
                            <button key={index} className="btn btn-secondary hover-success" onClick={() => toGuestModal(index)}>{p.firstName}</button>)}
                        {reservation.guests!.length < 2 
                            ? (<button className="btn btn-secondary hover-success float-start" 
                                onClick={() => toGuestModal(reservation.guests!.length)}>Add guest</button>) 
                            : (<></>)}
                    </div>
                    <div className="float-end btn-group">
                        {reservation.id! < 0 
                            ? <button className="btn btn-secondary hover-danger" onClick={() => setShow(false)}>Cancel</button> 
                            : <button className="btn btn-secondary hover-danger" onClick={onRemove}>Delete</button>}
                        <button className="btn btn-secondary hover-success" onClick={onSave}>Save</button>
                    </div>
                </div>
            </Modal.Footer>
        </Modal>
        {reservation.guests?.map((p, index) => 
            <GuestModal key={index} show={showGuest[index] ?? (showGuest[index] = false)} guest={p} onClose={toReservationModal}/>
        )}
        <CreateGuestModal reservation={reservation} onClose={toReservationModal}
            show={showGuest[reservation.guests!.length] ?? (showGuest[reservation.guests!.length] = false)}/>
    </>);

    function GuestModal({show, guest, onClose}: {show: boolean, guest: Guest, onClose: VoidFunction}): ReactElement {
        const guestPage = GuestPage(guest);
        const onSave = (): void => {
            const guest = guestPage.action(); 
            if(guest) {
                const tempReservation = {...reservation};
                const guestIndex = tempReservation.guests!.findIndex(p => p.id === guest.id);
                guestIndex !== -1 
                ? tempReservation.guests![guestIndex] = guest
                : tempReservation.guests!.push(guest);
                setReservation(tempReservation);
                onClose();
            }
        };
        const onRemove = (): void => {
            if(reservation.id! >= 0) axios.delete(process.env.REACT_APP_API_URL + "/Guests/Delete/" + guest.id);
            setReservation({...reservation, guests: reservation.guests!.filter(p => p.id !== guest.id)});
            onClose();
        };
        return(<>
            <Modal show={show} onHide={onClose}>
                <Modal.Body style={{borderRadius: "5px 5px 0px 0px"}} className="bg-primary">
                    {guestPage.body}
                </Modal.Body>
                <Modal.Footer style={{borderRadius: "0px 0px 5px 5px"}} className="bg-primary">
                    <div className="btn-group">
                        <button className="btn btn-secondary hover-danger" onClick={onRemove}>Delete</button>
                        <button className="btn btn-secondary hover-success" onClick={onSave}>Save</button>
                    </div>
                </Modal.Footer>
            </Modal>
        </>);
    }

    function CreateGuestModal({show, reservation, onClose}: {show: boolean, reservation: Reservation, onClose: VoidFunction}): ReactElement {
        const blankGuest: Guest = {
            id: -1,
            firstName: undefined,
            lastName: undefined,
            prefix: PersonPrefix.Unknown,
            reservationId: reservation.id,
            reservation: reservation,
            note: "",
            age: undefined,
        };
        return(<GuestModal guest={blankGuest} onClose={onClose} show={show}/>);
    }
}