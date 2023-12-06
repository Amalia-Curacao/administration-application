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
import Person from "../../models/Person";
import { Modal } from "react-bootstrap";
import {default as GuestPage} from "../guest/page";
import {default as ReservationPage} from "../reservation/page";
import BookingSource from "../../models/BookingSource";
import PersonPrefix from "../../models/PersonPrefix";

const _info = {name: "Rooms", icon: <MdBedroomParent/>};
let onChange: { room(room: Room): void, reservation(reservation: Reservation): void, guest(guest: Person): void };

function RoomIndexBody(): ReactElement {
    const { id } = useParams();
    if(!id) throw new Error("Schedule ID is undefined.");
    const [monthYear, setMonthYear] = useState(new Date()); // [1, 12]
    const [rooms, setRooms] = useState(getRooms(parseInt(id)));
    let groupedRooms = groupByRoomType(rooms);
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

        guest: (guest: Person): void => {
            const reservation = rooms.flatMap(r => r.reservations).find(res => res!.id === guest.reservationId);
            if(!reservation) throw new Error("reservation could not be found");
            const personToChangeIndex = reservation!.people!.findIndex(p => p.id === guest.id);
            personToChangeIndex !== -1 ? reservation!.people![personToChangeIndex] = guest : reservation!.people!.push(guest);
            onChange.reservation(reservation!);
        },
    };
    
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
    const button: ReactElement = <button onClick={() => setModal(!modal)} style={{marginLeft: "-50%"}} className="flex-fill check-in no-decoration"></button>;
    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservation}/>
    </>);
}

function CheckOutCell({reservation}: {reservation: Reservation}): ReactElement{
    const [modal, setModal] = useState<boolean>(false);
    const button: ReactElement = <button onClick={() => setModal(!modal)} style={{marginRight: "-50%"}} className="flex-fill check-out no-decoration"></button>;
    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservation}/>
    </>);
}

function OccupiedCell({reservation, currentDate}: {reservation: Reservation, currentDate: Date}): ReactElement{
    const [modal, setModal] = useState<boolean>(false);
    const button: ReactElement = <button onClick={() => setModal(!modal)} className="flex-fill occupied no-decoration">{GuestName(reservation!, currentDate)}</button>;
    return(<>
        {button}
        <ReservationModal show={modal} setShow={(b: boolean) => setModal(b)} reservation={reservation}/>
    </>);

    function GuestName(occupied: Reservation, currentDate: Date) : ReactElement {
        if(!occupied) return(<></>);
        const beginingOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
        const dayAfterCheckIn = new Date(occupied.checkIn!.getFullYear(), occupied.checkIn!.getMonth(), occupied.checkIn!.getDate() + 1);
        const dayToShow = oldest(beginingOfMonth, dayAfterCheckIn).getDate();
        // TODO: change to occupied.guest?.name ?? ""
        if(currentDate.getDate() === dayToShow) return(<span className="guest-name">{"PETER van BLANKEN"}</span>);
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
        <CreateReservationModal setShow={setShow} checkIn={currentDate} room={room} show={modal} />
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
        people: [],
        peopleIds: []
    }
    return(<>
        <ReservationModal show={show} setShow={setShow} reservation={blankReservation}/>
    </>);
}

function ReservationModal({reservation, show, setShow}: {reservation: Reservation, show: boolean, setShow: (s: boolean) => void}): ReactElement {
    const [showGuest, setShowGuest] = useState<{[index: number]: boolean}>({});
    const [tempReservation, ] = useState<Reservation>({...reservation});
    const reservationPage = ReservationPage(tempReservation);
    const toReservationModal = (index: number): void => { 
        setShowGuest(showGuest => ({...showGuest, [index]: false}));
        setShow(true);
    };
    const toGuestModal = (index: number): void => {
        setShow(false);
        setShowGuest(showGuest => ({...showGuest, [index]: true}));
    }
    const onSave = (): void => {
        if(!reservationPage.action()) return;
        onChange.reservation(tempReservation);
        setShow(false);
     }
    return(<>
        <Modal show={show} onHide={() => setShow(false)}>
                <Modal.Body className="bg-primary">
                    {reservationPage.body}
                </Modal.Body>
                <Modal.Footer className="bg-primary">
                    {reservation.people!.length < 2 
                        ? (<button className="btn btn-secondary hover-success float-start" 
                            onClick={() => toGuestModal(reservation.peopleIds!.length)}>Add guest</button>) 
                        : (<></>)}
                    <button className="btn btn-secondary hover-success" onClick={onSave}>Save</button>
                </Modal.Footer>
        </Modal>
        {tempReservation.people?.map((p, index) => 
            <GuestModal key={index} show={showGuest[index] ?? (showGuest[index] = false)} guest={p} onClose={() => toReservationModal(index)}/>
        )}
        <CreateGuestModal reservation={tempReservation} onClose={() => toReservationModal(tempReservation.peopleIds!.length)}
            show={showGuest[tempReservation.peopleIds!.length] ?? (showGuest[tempReservation.peopleIds!.length] = false)}/>
    </>);

    function GuestModal({show, guest, onClose}: {show: boolean, guest: Person, onClose: VoidFunction}): ReactElement {
        const guestPage = GuestPage(guest);
        return(<>
            <Modal show={show} onHide={onClose}>
                <Modal.Body className="bg-primary">
                    {guestPage.body}
                </Modal.Body>
                <Modal.Footer className="bg-primary">
                    <button className="btn btn-secondary hover-success" onClick={onClose}>Close</button>
                </Modal.Footer>
            </Modal>
        </>);
    }

    function CreateGuestModal({show, reservation, onClose}: {show: boolean, reservation: Reservation, onClose: VoidFunction}): ReactElement {
        const blankGuest: Person = {
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
                peopleIds: [],
                people: []
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
                peopleIds: [],
                people: []
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

// #endregion

export const link: PageLink = {
    icon: _info.icon,
    route: '/room',
    element: <RoomIndexBody/>,
    params: '/:id'
};
