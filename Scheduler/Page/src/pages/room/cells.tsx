import { ReactElement, useState, Fragment } from "react";
import { isSameDay, oldest } from "../../extensions/Date";
import Reservation from "../../models/Reservation";
import Room from "../../models/Room";
import ReservationModal, { CreateReservationModal} from "./modal";
import axios from "axios";
import { ToJsonGuest, ToJsonReservation } from "../../extensions/ToJson";
import { useNavigate } from "react-router-dom";

export default function Cells({room, monthYear}: {room: Room, monthYear: Date}): ReactElement{
    const navigation = useNavigate();
    const amount = new Date(monthYear.getFullYear(), monthYear.getMonth() + 1, 0).getDate();
    const reservations = room.reservations ?? [];
    let days: ReactElement[] = [];

    for(let day = 1; day <= amount; day++) {
        const current: Date = new Date(monthYear.getFullYear(), monthYear.getMonth(), day);
        const occupy: Reservation[] =  reservations.filter(r => (r.checkIn! <= current) && (r.checkOut! >= current));
        const addReservationCell = (shape: string): ReactElement => {return(<EmptyCell room={room} currentDate={current} shape={shape} onAdd={onAdd}/>)};

        days.push(<td style={{overflow:"hidden"}} key={day} className={"p-0 d-flex flex-fill cell" + (day % 2 === 0 ? " darken " : "")}>
                {occupy.length === 0
                    ? addReservationCell("")
                    : <Cell occupy={occupy} current={current} onDelete={onDelete} onUpdate={onUpdate} emptyCell={addReservationCell}/>}
            </td>);
    }

    return(<tr className="d-flex flex-fill bg-secondary p-0">
        {days}
    </tr>);

    function onUpdate(reservation: Reservation): void {
        reservation = {...reservation, roomNumber: room.number, roomType: room.type, roomScheduleId: room.scheduleId, scheduleId: room.scheduleId }
        axios.post(process.env.REACT_APP_API_URL + "/Reservations/Edit", ToJsonReservation(reservation)).then(() => {
            axios.put(process.env.REACT_APP_API_URL + "/Guest/Set/" + reservation.id, reservation.guests!.map(ToJsonGuest))
            .catch(err => {console.log(err)});
        })
        .then(() => navigation(0))
        .catch(err => {console.log(err)});
    }
    function onDelete(reservation: Reservation): void {
        axios.delete(process.env.REACT_APP_API_URL + "/Reservations/Delete/" + reservation.id)
        .then(() => navigation(0))
        .catch(err => {console.log(err)});
    }
    function onAdd(reservation: Reservation): void {
        axios.post(process.env.REACT_APP_API_URL + "/Reservations/Create", ToJsonReservation(reservation))
        .then(response => {
            reservation.guests!.forEach(guest => {
                axios.post(process.env.REACT_APP_API_URL + "/Guest/Create", ToJsonGuest({...guest, reservationId: (response.data as Reservation).id}))
                .catch(err => {console.log(err)});
            });
        })
        .then(() => navigation(0))
        .catch(err => {console.log(err)});
    }
}

// occupy should not is not empty
export function Cell({occupy, current, onUpdate, onDelete, emptyCell}: {occupy: Reservation[], current: Date, onUpdate: (r: Reservation) => void, onDelete: (r: Reservation) => void, emptyCell: (shape: string) => ReactElement}): ReactElement {
    const [checkIn, setCheckIn] = useState<Reservation | undefined>(occupy.find(r => isSameDay(r.checkIn!, current)));
    const [checkOut, setCheckOut] = useState<Reservation | undefined>(occupy.find(r => isSameDay(r.checkOut!, current)));
    switch(true) {
        case checkIn !== undefined && checkOut !== undefined: return(<>
            <CheckOutCell onSave={updateCheckOut} reservation={checkOut!}/>
            <CheckInCell onSave={updateCheckIn} reservation={checkIn!}/>
        </>);
        case checkIn !== undefined: return(<>
            {emptyCell("L")}
            <CheckInCell onSave={updateCheckIn} reservation={checkIn!}/>
        </>);
        case checkOut !== undefined: return(<>
            <CheckOutCell onSave={updateCheckOut} reservation={checkOut!}/>
            {emptyCell("R")}
        </>);
        default: return <OccupiedCell onSave={updateOccupy} reservation={occupy[0]} currentDate={current}/>;
    }    

    function updateCheckIn(reservation: Reservation | undefined): void {
        setCheckIn(reservation);
        if(reservation) onUpdate(reservation);
        else onDelete(checkIn!);
    }
    function updateCheckOut(reservation: Reservation | undefined): void {
        setCheckOut(reservation);
        if(reservation) onUpdate(reservation);
        else onDelete(checkOut!);
    }
    function updateOccupy(reservation: Reservation | undefined): void {
        reservation !== undefined 
            ? onUpdate(reservation!) 
            : onDelete(occupy[0]);
    }
}

export function CheckInCell({reservation, onSave}: {reservation: Reservation, onSave: (r: Reservation | undefined) => void}): ReactElement{
    // TODO migrate on level lower
    const [modal, setModal] = useState<boolean>(false);
    const button: ReactElement = <button onClick={() => setModal(!modal)} style={{marginLeft: "-50%"}} className="flex-fill check-in no-decoration"></button>;
    
    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservation} onSave={onSave}/>
    </>);
}

export function CheckOutCell({reservation, onSave}: {reservation: Reservation, onSave: (r: Reservation | undefined) => void}): ReactElement{
    const [modal, setModal] = useState<boolean>(false);
    const button: ReactElement = <button onClick={() => setModal(!modal)} style={{marginRight: "-50%"}} className="flex-fill check-out no-decoration"></button>;
    
    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservation} onSave={onSave}/>
    </>);
}

export function OccupiedCell({reservation, currentDate, onSave}: {reservation: Reservation, currentDate: Date, onSave: (r: Reservation | undefined) => void}): ReactElement{
    const [modal, setModal] = useState<boolean>(false);
    const button: ReactElement = <button onClick={() => setModal(!modal)} className="flex-fill occupied no-decoration">{
        GuestName(reservation!, currentDate)}</button>;

    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservation} onSave={onSave}/>
    </>);

    function GuestName(occupied: Reservation, currentDate: Date) : ReactElement {
        if(!occupied) return(<Fragment/>);
        const beginingOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
        const dayAfterCheckIn = new Date(occupied.checkIn!.getFullYear(), occupied.checkIn!.getMonth(), occupied.checkIn!.getDate() + 1);
        if(isSameDay(currentDate, oldest(beginingOfMonth, dayAfterCheckIn)))
            {   
                return(<span className="guest-name">
                    {occupied.guests!.length > 0
                        ? occupied.guests![0].lastName!
                        : "" }
                </span>);}
        return <Fragment/>;
    } 
}

export function EmptyCell({shape, room, currentDate, onAdd}: {shape: string, room: Room, currentDate: Date, onAdd: (r: Reservation) => void}): ReactElement{
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
        <CreateReservationModal onSave={onAdd} setShow={setShow} show={modal} checkIn={currentDate} room={room} />
    </>);
}