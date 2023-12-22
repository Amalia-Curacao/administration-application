import axios from "axios";
import { ReactElement, useState } from "react";
import { Modal } from "react-bootstrap";
import { isSameDay, oldest } from "../../extensions/Date";
import { ToJson } from "../../extensions/ToJson";
import BookingSource from "../../models/BookingSource";
import Guest from "../../models/Guest";
import PersonPrefix from "../../models/PersonPrefix";
import Reservation from "../../models/Reservation";
import Room from "../../models/Room";
import RoomType from "../../models/RoomType";
import { MdBedroomParent } from "react-icons/md";
import {default as GuestPage} from "../guest/page";
import {default as ReservationPage} from "../reservation/page";

export default function Tables({rooms, monthYear, showDates}: {rooms: Room[], monthYear: Date, showDates: boolean}): ReactElement {
    if(rooms.length === 0) return(<></>);

    return(<>
        <table className="table table-borderless mt-0 mb-0">
            <thead>
                <tr hidden={!showDates}>
                    <td className="flipped" style={{fontSize:"16px", borderRadius: "5px 0px 0px 0px"}}>
                        <MdBedroomParent/>
                    </td>                            
                    <Dates monthYear={monthYear}/>   
                </tr> 
                <tr>
                    <th colSpan={2} className="pb-0 pt-0 align-middle">
                        {RoomType[rooms[0].type!]}
                    </th>
                </tr>
            </thead> 
            <tbody>
                {rooms.map((r, index) => 
                <tr style={{overflow:"visible"}} className="darken-on-hover" key={index}>
                    <td className="flipped text-center">{r.number}</td>
                    <RoomAvailibilty key={index} monthYear={monthYear} reservations={r.reservations ?? []} room={r}/>
                </tr>)}
            </tbody>
        </table>
    </>);
}

function Dates({monthYear}: {monthYear: Date}): ReactElement {
    const totalAmountOfdays = new Date(monthYear.getFullYear(), monthYear.getMonth() + 1, 0).getDate();
    let days: ReactElement[] = [];
    for(let i = 1; i <= totalAmountOfdays; i++) days.push(<Day key={i} day={i}/>);

    return(<tr className="d-flex p-0">{days}</tr>);
    function Day({day}: {day: number}): ReactElement {
        const date = new Date(monthYear.getFullYear(), monthYear.getMonth(), day);
        const cellClass = "d-flex flex-fill justify-content-center flex-column darken-on-hover p-2 bg-primary text-secondary";
        const colorClass = isSameDay(date, new Date()) ? " " : (date < new Date() ? " past" : " ") ;
        const borderRadius = totalAmountOfdays === day ? "0px 5px 0px 0px" : "0px";

        return(
            <td style={{borderRadius: borderRadius}} className={cellClass + colorClass}>
                <div className="d-flex justify-content-center">
                    {day}
                </div>
                <div style={{fontSize: "12px"}} className="d-flex justify-content-center">
                    {["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"][date.getDay()]}
                </div>
            </td>
        );
    }
}

function RoomAvailibilty({monthYear, reservations, room}: {monthYear: Date, reservations: Reservation[], room: Room}): ReactElement {
    const amount = new Date(monthYear.getFullYear(), monthYear.getMonth() + 1, 0).getDate();

    let days: ReactElement[] = [];
    for(let day = 1; day <= amount; day++) {
        const currentDate = new Date(monthYear.getFullYear(), monthYear.getMonth(), day);
        const checkIn: Reservation | undefined = reservations.find(r => isSameDay(currentDate, r.checkIn!));
        const checkOut: Reservation | undefined = reservations.find(r => isSameDay(currentDate, r.checkOut!));
        const occupied: Reservation | undefined =  reservations.find(r => (r.checkIn! < currentDate) && (r.checkOut! > currentDate));
        // TODO check if occupied is undefined and if so check if there is a reservation that starts on this day
        const element = (): ReactElement => {
            
            switch(true) {
                case checkIn !== undefined && checkOut !== undefined: return(<>
                    <CheckOutCell reservation={checkOut!}/>
                    <CheckInCell reservation={checkIn!}/>
                </>);
                case checkIn !== undefined: return(<>
                    <EmptyCell room={room} currentDate={currentDate} shape="L"/>
                    <CheckInCell reservation={checkIn!}/>
                </>);
                case checkOut !== undefined: return(<>
                    <CheckOutCell reservation={checkOut!}/>
                    <EmptyCell room={room} currentDate={currentDate} shape="R"/>
                </>);
                case occupied !== undefined: return(
                    <OccupiedCell reservation={occupied!} currentDate={currentDate}/>
                );
                default: return(<EmptyCell room={room} currentDate={currentDate} shape=""/>);
            }
        }

        days.push(<td style={{overflow:"hidden"}} key={day} className={"p-0 d-flex flex-fill cell" + darken(day)}>
            {element()}
        </td>); 
    }

    return(<tr className="d-flex flex-fill bg-secondary p-0">
        {days}
    </tr>);
}

function darken(day: number): string{
    if(day % 2 === 0) return(" darken ");
    return "";
}

function CheckInCell({reservation}: {reservation: Reservation}): ReactElement{
    const [modal, setModal] = useState<boolean>(false);
    const [reservationState, setReservationState] = useState<Reservation>(reservation);
    const button: ReactElement = <button onClick={() => setModal(!modal)} style={{marginLeft: "-50%"}} className="flex-fill check-in no-decoration"></button>;
    
    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservationState}  setReservation={(r: Reservation) => setReservationState(r)}/>
    </>);
}

function CheckOutCell({reservation}: {reservation: Reservation}): ReactElement{
    const [modal, setModal] = useState<boolean>(false);
    const [reservationState, setReservationState] = useState<Reservation>(reservation);
    const button: ReactElement = <button onClick={() => setModal(!modal)} style={{marginRight: "-50%"}} className="flex-fill check-out no-decoration"></button>;
    
    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservationState} setReservation={(r: Reservation) => setReservationState(r)}/>
    </>);
}

function OccupiedCell({reservation, currentDate}: {reservation: Reservation, currentDate: Date}): ReactElement{
    const [modal, setModal] = useState<boolean>(false);
    const [reservationState, setReservationState] = useState<Reservation>(reservation);
    const button: ReactElement = <button onClick={() => setModal(!modal)} className="flex-fill occupied no-decoration">{
        GuestName(reservation!, currentDate)}</button>;

    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservationState} setReservation={(r: Reservation) => setReservationState(r)}/>
    </>);

    function GuestName(occupied: Reservation, currentDate: Date) : ReactElement {
        if(!occupied) return(<></>);
        const beginingOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
        const dayAfterCheckIn = new Date(occupied.checkIn!.getFullYear(), occupied.checkIn!.getMonth(), occupied.checkIn!.getDate() + 1);
        const dayToShow = oldest(beginingOfMonth, dayAfterCheckIn).getDate();
        if(currentDate.getDate() === dayToShow) 
        return(<span className="guest-name">
            {occupied.guests!.length > 0
                ? occupied.guests![0].lastName!
                : "" }
            </span>);
        return <></>;
    } 
}

function EmptyCell({shape, room, currentDate}: {shape: string, room: Room, currentDate: Date}): ReactElement{
    const [modal, setModal] = useState<boolean>(false);
    function onClick(): void{
        setModal(!modal);
    }

    function setShow(b: boolean){
        setModal(b);
    }

    const button = (): ReactElement => {
        switch(shape){
            case "L":
                return (<button style={{marginLeft: "-50%"}} className="d-flex flex-fill no-decoration" onClick={onClick}/>);
            case "R":
                return(<button style={{marginRight: "-50%"}} className="d-flex flex-fill no-decoration" onClick={onClick}/>);
            default:
                return(<button className="d-flex flex-fill no-decoration" onClick={onClick}/>);
        }
    }

    return(<>
        {button()} 
        <CreateReservationModal setShow={setShow} show={modal} checkIn={currentDate} room={room} />
    </>);
}

function CreateReservationModal({checkIn, room, show, setShow}: {checkIn: Date, room: Room, show: boolean, setShow: (b: boolean) => void}): ReactElement {
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

function ReservationModal({reservation, show, setShow, setReservation}: {reservation: Reservation, setReservation: (r: Reservation) => void, show: boolean, setShow: (s: boolean) => void}): ReactElement {
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