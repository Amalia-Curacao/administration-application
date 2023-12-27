import { ReactElement, useState, Fragment } from "react";
import { oldest } from "../../extensions/Date";
import Reservation from "../../models/Reservation";
import Room from "../../models/Room";
import  ReservationModal, { CreateReservationModal} from "./modal";

export function CheckInCell({reservation}: {reservation: Reservation}): ReactElement{
    const [modal, setModal] = useState<boolean>(false);
    const [reservationState, setReservationState] = useState<Reservation>(reservation);
    const button: ReactElement = <button onClick={() => setModal(!modal)} style={{marginLeft: "-50%"}} className="flex-fill check-in no-decoration"></button>;
    
    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservationState}  setReservation={(r: Reservation) => setReservationState(r)}/>
    </>);
}

export function CheckOutCell({reservation}: {reservation: Reservation}): ReactElement{
    const [modal, setModal] = useState<boolean>(false);
    const [reservationState, setReservationState] = useState<Reservation>(reservation);
    const button: ReactElement = <button onClick={() => setModal(!modal)} style={{marginRight: "-50%"}} className="flex-fill check-out no-decoration"></button>;
    
    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservationState} setReservation={(r: Reservation) => setReservationState(r)}/>
    </>);
}

export function OccupiedCell({reservation, currentDate}: {reservation: Reservation, currentDate: Date}): ReactElement{
    const [modal, setModal] = useState<boolean>(false);
    const [reservationState, setReservationState] = useState<Reservation>(reservation);
    const button: ReactElement = <button onClick={() => setModal(!modal)} className="flex-fill occupied no-decoration">{
        GuestName(reservation!, currentDate)}</button>;

    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservationState} setReservation={(r: Reservation) => setReservationState(r)}/>
    </>);

    function GuestName(occupied: Reservation, currentDate: Date) : ReactElement {
        if(!occupied) return(<Fragment/>);
        const beginingOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
        const dayAfterCheckIn = occupied.checkIn!;
        const dayToShow = oldest(beginingOfMonth, dayAfterCheckIn).getDate();
        if(currentDate.getDate() === dayToShow) 
        return(<span className="guest-name">
            {occupied.guests!.length > 0
                ? occupied.guests![0].lastName!
                : "" }
            </span>);
        return <Fragment/>;
    } 
}

export function EmptyCell({shape, room, currentDate}: {shape: string, room: Room, currentDate: Date}): ReactElement{
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
