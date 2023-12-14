import { ReactElement, useState } from "react";
import "../../scss/room.table.scss";
import { MdBedroomParent } from "react-icons/md";
import Schedule from "../../models/Schedule";
import PageLink from "../../types/PageLink";
import Room from "../../models/Room";
import RoomType from "../../models/RoomType";
import { useParams } from "react-router-dom";
import Reservation from "../../models/Reservation";
import { isSameDay, oldest } from "../../extensions/Date";
import { FaLongArrowAltLeft, FaLongArrowAltRight } from "react-icons/fa";
import Guest from "../../models/Guest";
import { Modal } from "react-bootstrap";
import {default as GuestPage} from "../guest/page";
import {default as ReservationPage} from "../reservation/page";
import BookingSource from "../../models/BookingSource";
import PersonPrefix from "../../models/PersonPrefix";

const _info = {name: "Rooms", icon: <MdBedroomParent/>};
let onChange: { room(room: Room): void, reservation(reservation: Reservation): void, guest(guest: Guest): void };
let onDelete: { room(room: Room): void, reservation(reservation: Reservation): void, guest(guest: Guest): void };

function RoomIndexBody(): ReactElement {
    const { id } = useParams();
    if(!id) throw new Error("Schedule ID is undefined.");
    const [monthYear, setMonthYear] = useState(new Date()); // [1, 12]
    const [rooms, setRooms] = useState(getRooms(parseInt(id)));
    let groupedRooms = groupByRoomType(rooms);
    initOnChange(rooms, (r: Room[]) => setRooms(r));
    initOnDelete(rooms, (r: Room[]) => setRooms(r));
    
    
    return(<>
        <div style={{borderRadius:"5px"}} className="p-3 m-3 mb-2 bg-primary d-flex flex-fill flex-row">
            <MonthYearSelector monthYear={monthYear} onChange={onMonthYearSelected}/>
        </div>
        <div className="p-3 pb-0 d-flex flex-column flex-fill">
            {Object.keys(groupedRooms).map((key, index) => <Tables key={index} monthYear={monthYear} showDates={index === 0} rooms={groupedRooms[parseInt(key)]}/>)}
            <div className="table-end" style={{borderRadius:"0 0 5px 5px"}}></div>
        </div>
    </>);
    
    function onMonthYearSelected(monthYear: Date): void {
        setMonthYear(monthYear);
    }
}

// #region Elements
function MonthYearSelector({monthYear, onChange}: {monthYear: Date, onChange: (monthYear: Date) => void}): ReactElement {
    const arrowClass = "me-2 justify-content-center align-content-middle no-decoration bg-primary";
    return(<>
        <div className="d-flex flex-fill flex-row justify-content-center align-items-center">
            <button className={arrowClass} onClick={() => onChange(new Date(monthYear.getFullYear(), monthYear.getMonth() - 1, 1))}>
                <FaLongArrowAltLeft size={32} className="text-secondary"/>
            </button>
            <h5 className="text-center text-secondary me-2 ms-2">{monthYear.toLocaleString('default', { month: 'long' }) + " " + monthYear.getFullYear()}</h5>
            <button className={arrowClass} onClick={() => onChange(new Date(monthYear.getFullYear(), monthYear.getMonth() + 1, 1))}>
                <FaLongArrowAltRight size={32} className="text-secondary i-l"/>
            </button>
        </div>
    </>);
}

function Tables({rooms, monthYear, showDates}: {rooms: Room[], monthYear: Date, showDates: boolean}): ReactElement{
    if(rooms.length === 0) return(<></>);

    return(<>
        <table className="table table-borderless mt-0 mb-0">
            <thead >
                <tr hidden={!showDates}>
                    <td  className="flipped" style={{fontSize:"16px", borderRadius: "5px 0px 0px 0px"}}>
                        {_info.icon}
                    </td>                            
                    <Dates monthYear={monthYear}/>   
                </tr> 
                <tr>
                    <th colSpan={2} className="pb-0 pt-0 align-middle">
                        {RoomType[Number(rooms[0].type)]}
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
        // TODO: change to occupied.guest?.name ?? ""
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
        guests: [],
        guestIds: []
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
        console.log(reservation)
        onChange.reservation(reservation);
        setReservation(reservation);
        setShow(false);
    }
    const onRemove = (): void => {
        onDelete.reservation(reservation);
        setShow(false);
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
                                onClick={() => toGuestModal(reservation.guestIds!.length)}>Add guest</button>) 
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
            show={showGuest[reservation.guestIds!.length] ?? (showGuest[reservation.guestIds!.length] = false)}/>
    </>);

    function GuestModal({show, guest, onClose}: {show: boolean, guest: Guest, onClose: VoidFunction}): ReactElement {
        const guestPage = GuestPage(guest);
        const onSave = (): void => {
            const guest = guestPage.action(); 
            if(guest) {
                const tempReservation = {...reservation};
                const guestIndex = tempReservation.guests!.findIndex(p => p.id === guest.id);
                if(guestIndex !== -1) tempReservation.guests![guestIndex] = guest;
                else {
                    tempReservation.guests!.push(guest);
                    tempReservation.guestIds!.push(guest.id!);
                };
                console.log(tempReservation);
                setReservation(tempReservation);
                onClose();
            }
        };
        const onRemove = (): void => {
            onDelete.guest(guest);
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
            firstName: "",
            lastName: "",
            prefix: PersonPrefix.Unknown,
            reservationId: reservation.id,
            reservation: reservation,
            note: "",
            age: undefined,
        };
        return(<GuestModal guest={blankGuest} onClose={onClose} show={show}/>);
    }
}
// #endregion

// #region Functions
function groupByRoomType(rooms: Room[]): {[type: number]: Room[]} {
    let groupedRooms: {[type: number]: Room[]} = [];
    rooms.forEach(r => {
        if(!r.type) r.type = RoomType.None;
        if(groupedRooms[r.type] === undefined) groupedRooms[r.type] = [];
        groupedRooms[r.type].push(r);
    });
    return groupedRooms;
}

function getRooms(scheduleId: number): Room[] {
    const rooms: Room[] = [
        {number: 1, floor: 1, 
            reservations: [{
                id: 1,
                checkOut: new Date(2023, 10, 10),
                checkIn: new Date(2023, 10, 1),
                bookingSource: undefined,
                flightArrivalNumber: undefined,
                flightArrivalTime: undefined,
                flightDepartureNumber: undefined,
                flightDepartureTime: undefined,
                remarks: undefined,
                room: undefined,
                roomNumber: 1,
                roomScheduleId: 1,
                roomType: RoomType.Room,
                schedule: undefined,
                scheduleId: 1,
                guestIds: [],
                guests: []
            },
            {
                id: 2,
                checkOut: new Date(2023, 10, 15),
                checkIn: new Date(2023, 10, 10),
                bookingSource: undefined,
                flightArrivalNumber: undefined,
                flightArrivalTime: undefined,
                flightDepartureNumber: undefined,
                flightDepartureTime: undefined,
                remarks: undefined,
                room: undefined,
                roomNumber: 1,
                roomScheduleId: 1,
                roomType: RoomType.Room,
                schedule: undefined,
                scheduleId: 1,
                guestIds: [],
                guests: []
            },
        ], 
        schedule: undefined, scheduleId: scheduleId, type: RoomType.Room},
        {number: 2, floor: 1, reservations: [], schedule: undefined, scheduleId: scheduleId, type: RoomType.Room},
        {number: 3, floor: 1, reservations: [], schedule: undefined, scheduleId: scheduleId, type: RoomType.Room},
        {number: 4, floor: 1, reservations: [], schedule: undefined, scheduleId: scheduleId, type: RoomType.Room},
        {number: 5, floor: 1, reservations: [], schedule: undefined, scheduleId: scheduleId, type: RoomType.Room},
        
        {number: 11, floor: 1, reservations: [], schedule: undefined, scheduleId: scheduleId, type: RoomType.Apartment},
        {number: 12, floor: 1, reservations: [], schedule: undefined, scheduleId: scheduleId, type: RoomType.Apartment},
        {number: 13, floor: 1, reservations: [], schedule: undefined, scheduleId: scheduleId, type: RoomType.Apartment},
        {number: 14, floor: 1, reservations: [], schedule: undefined, scheduleId: scheduleId, type: RoomType.Apartment},
        {number: 15, floor: 1, reservations: [], schedule: undefined, scheduleId: scheduleId, type: RoomType.Apartment},
    ];

    const schedules: Schedule[] = [
        {id: 1, name: "Schedule 1", reservations: [], rooms: rooms},
        {id: 2, name: "Schedule 2", reservations: [], rooms: []},
        {id: 3, name: "Schedule 3", reservations: [], rooms: []}];
    
    rooms.forEach(r => r.schedule = schedules.find(s => s.id === r.scheduleId) ?? undefined);

    return(schedules.find((s) => s.id === scheduleId)?.rooms ?? []);
}

function darken(day: number): string{
    if(day % 2 === 0) return(" darken ");
    return "";
}

function initOnChange(rooms: Room[], setRooms: (r: Room[]) => void): void {
    onChange = {
        room: (room: Room): void => {
            // TODO make call to the backend and fill the rooms with the new data
            setRooms(rooms.map(r => r.number === room.number && r.type === room.type ? room : r));
        },

        reservation: (reservation: Reservation): void => {
            const room = rooms.find(r => r.number === reservation.roomNumber && r.type === reservation.roomType);
            if(!room) return;
            const reservationIndex = room.reservations!.findIndex(r => r.id === reservation.id);
            reservationIndex !== -1 ? room.reservations![reservationIndex] = reservation : room.reservations!.push(reservation);
            onChange.room({...room, reservations: room.reservations});
        },

        guest: (guest: Guest): void => {
            const reservation = rooms.flatMap(r => r.reservations).find(res => res!.id === guest.reservationId);
            if(!reservation) throw new Error("reservation could not be found");
            const personToChangeIndex = reservation!.guests!.findIndex(p => p.id === guest.id);
            personToChangeIndex !== -1 ? reservation!.guests![personToChangeIndex] = guest : reservation!.guests!.push(guest);
            onChange.reservation(reservation!);
        },
    };
}

function initOnDelete(rooms: Room[], setRooms: (r: Room[]) => void): void {
    onDelete = {
        room: (room: Room): void => {
            // TODO make call to the backend and fill the rooms with the new data
            setRooms(rooms.filter(r => r.number !== room.number || r.type !== room.type));
        },

        reservation: (reservation: Reservation): void => {
            const room = rooms.find(r => r.number === reservation.roomNumber && r.type === reservation.roomType);
            if(!room) return;
            const reservationIndex = room.reservations!.findIndex(r => r.id === reservation.id);
            if(reservationIndex === -1) return;
            room.reservations!.splice(reservationIndex, 1);
            onChange.room({...room, reservations: room.reservations});
        },

        guest: (guest: Guest): void => {
            const reservation = rooms.flatMap(r => r.reservations).find(res => res!.id === guest.reservationId);
            if(!reservation) throw new Error("reservation could not be found");
            const personToDeleteIndex = reservation!.guests!.findIndex(p => p.id === guest.id);
            if(personToDeleteIndex === -1) return;
            reservation!.guests!.splice(personToDeleteIndex, 1);
            reservation!.guestIds!.splice(personToDeleteIndex, 1);
            onChange.reservation(reservation!);
        },
    };
}

// #endregion

export const link: PageLink = {
    icon: _info.icon,
    route: '/room',
    element: <RoomIndexBody/>,
    params: '/:id'
};
