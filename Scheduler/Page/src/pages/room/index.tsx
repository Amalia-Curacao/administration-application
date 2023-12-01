import React, { ReactElement, createRef, useState } from "react";
import "../../scss/room.table.scss";
import { MdBedroomParent } from "react-icons/md";
import Schedule from "../../models/Schedule";
import PageLink from "../../types/PageLink";
import PageState from "../../types/PageState";
import Room from "../../models/Room";
import RoomType from "../../models/RoomType";
import { useParams } from "react-router-dom";
import Reservation from "../../models/Reservation";
import { isSameDay, oldest } from "../../extensions/Date";
import { FaLongArrowAltLeft, FaLongArrowAltRight } from "react-icons/fa";
import { default as ReservationPage} from "../reservation/page";
import Modal from "react-bootstrap/Modal";
import BookingSource from "../../models/BookingSource";

const _info = {name: "Rooms", icon: <MdBedroomParent/>};

function RoomIndexBody(): ReactElement {
    const { id } = useParams();
    if(!id) throw new Error("Schedule ID is undefined.");
    const [monthYear, setMonthYear] = useState(new Date()); // [1, 12]
    const [rooms, setRooms] = useState(getRooms(parseInt(id)));
    const [state, setState] = useState(PageState.Default);
    const [createModal, setModal] = useState<ReactElement>(<></>);
    let groupedRooms = groupByRoomType(rooms);

    function onCreateReservation(element: ReactElement, handleSave: () => boolean){
        if(state !== PageState.Default) return;  
        setModal(<Modal onHide={handleClose} show={true}>
            <Modal.Body className="bg-primary d-flex">
                {element}
            </Modal.Body>
            <Modal.Footer className="bg-primary">
                <button className="btn btn-secondary hover-danger" onClick={handleClose}>Cancel</button>
                <button className="btn btn-secondary hover-success" onClick={() => {if(handleSave()) handleClose()}}>Save</button>
            </Modal.Footer>
        </Modal>);
        setState(PageState.Create);

        function handleClose(): void {
            setState(PageState.Default);
            setModal(<></>);
        }
    }

    function onMonthYearSelected(monthYear: Date): void {
        setMonthYear(monthYear);
    }

    return(<>
        {createModal}
        <div style={{borderRadius:"5px"}} className="p-3 m-3 mb-2 bg-primary d-flex flex-fill flex-row">
            <MonthYearSelector monthYear={monthYear} onChange={onMonthYearSelected}/>
        </div>
        <div className="p-3 pb-0 d-flex flex-column flex-fill">
            {Object.keys(groupedRooms).map((key, index) => 
                <Tables key={index} monthYear={monthYear} showDates={index === 0} rooms={groupedRooms[parseInt(key)]} 
                onCreateReservation={onCreateReservation}/>)}
            <div className="table-end" style={{borderRadius:"0 0 5px 5px"}}></div>
        </div>
    </>);
}

// #region Elements

function Tables({rooms, monthYear, showDates, onCreateReservation}: {rooms: Room[], monthYear: Date, showDates: boolean, onCreateReservation: (e :ReactElement, f: () => boolean) => void}): ReactElement{
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
                    <RoomAvailibilty key={index} monthYear={monthYear} reservations={r.reservations ?? []} room={r} onCreateReservation={onCreateReservation}/>
                </tr>)}
            </tbody>
        </table>
    </>);
}

function RoomAvailibilty({monthYear, reservations, room, onCreateReservation}: {monthYear: Date, reservations: Reservation[], room: Room, onCreateReservation: (e : ReactElement, f: () => boolean) => void}): ReactElement {
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
                    <CheckOutCell reservation={checkOut!} onClick={onCreateReservation}/>
                    <CheckInCell reservation={checkIn!} onClick={onCreateReservation}/>
                </>);
                case checkIn !== undefined: return(<>
                    <EmptyCell room={room} currentDate={currentDate} shape="L" onClick={onCreateReservation}/>
                    <CheckInCell reservation={checkIn!} onClick={onCreateReservation}/>
                </>);
                case checkOut !== undefined: return(<>
                    <CheckOutCell reservation={checkOut!} onClick={onCreateReservation}/>
                    <EmptyCell room={room} currentDate={currentDate} shape="R" onClick={onCreateReservation}/>
                </>);
                case occupied !== undefined: return(
                    <OccupiedCell reservation={occupied!} currentDate={currentDate} onClick={onCreateReservation}/>
                );
                // TODO add the add reservation button
                default: return(<EmptyCell room={room} currentDate={currentDate} shape="" onClick={onCreateReservation}/>);
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

function CheckInCell({onClick, reservation}: {onClick: (e : ReactElement, f: () => boolean) => void, reservation: Reservation}): ReactElement{
    const editReservationPage = ReservationPage(reservation);
    return(<button onClick={() => onClick(editReservationPage.body, editReservationPage.action)} style={{marginLeft: "-50%"}} className="flex-fill check-in no-decoration"></button>);
}

function CheckOutCell({onClick, reservation}: {onClick: (e : ReactElement, f: () => boolean) => void, reservation: Reservation}): ReactElement{
    const editReservationPage = ReservationPage(reservation);
    return(<button onClick={() => onClick(editReservationPage.body, editReservationPage.action)} style={{marginRight: "-50%"}} className="flex-fill check-out no-decoration"></button>);
}

function OccupiedCell({onClick, reservation, currentDate}: {onClick: (e : ReactElement, f: () => boolean) => void, reservation: Reservation, currentDate: Date}): ReactElement{
    const editReservationPage = ReservationPage(reservation);
    return(<button onClick={() => onClick(editReservationPage.body, editReservationPage.action)} 
    className="flex-fill occupied no-decoration">{GuestName(reservation!, currentDate)}</button>);

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

function EmptyCell({onClick, shape, room, currentDate}: {onClick: (e : ReactElement, f: () => boolean) => void, shape: string, room: Room, currentDate: Date}): ReactElement{
    const reservationPage = ReservationPage({
        scheduleId: room.scheduleId,
        roomNumber: room.number,
        roomType: room.type,
        checkIn: currentDate,
        id: null,
        checkOut: null,
        flightArrivalNumber: null,
        flightDepartureNumber: null,
        flightArrivalTime: null,
        flightDepartureTime: null,
        bookingSource: null,
        remarks: null,
        roomScheduleId: room.scheduleId,
        room: null,
        schedule: null
    });
    switch(shape){
        case "L":
            return (<button style={{marginLeft: "-50%"}} className="d-flex flex-fill no-decoration"
            onClick={() => onClick(reservationPage.body, reservationPage.action)}/>);
        case "R":
            return(<button style={{marginRight: "-50%"}} className="d-flex flex-fill no-decoration"
            onClick={() => onClick(reservationPage.body, reservationPage.action)}/>);
        default:
            return(<button className="d-flex flex-fill no-decoration"
            onClick={() => onClick(reservationPage.body, reservationPage.action)}/>);
    }
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
                bookingSource: null,
                flightArrivalNumber: null,
                flightArrivalTime: null,
                flightDepartureNumber: null,
                flightDepartureTime: null,
                remarks: null,
                room: null,
                roomNumber: 1,
                roomScheduleId: 1,
                roomType: RoomType.Room,
                schedule: null,
                scheduleId: 1,
            },
            {
                id: 2,
                checkOut: new Date(2023, 10, 15),
                checkIn: new Date(2023, 10, 10),
                bookingSource: null,
                flightArrivalNumber: null,
                flightArrivalTime: null,
                flightDepartureNumber: null,
                flightDepartureTime: null,
                remarks: null,
                room: null,
                roomNumber: 1,
                roomScheduleId: 1,
                roomType: RoomType.Room,
                schedule: null,
                scheduleId: 1,
            },
        ], 
        schedule: null, scheduleId: scheduleId, type: RoomType.Room},
        {number: 2, floor: 1, reservations: [], schedule: null, scheduleId: scheduleId, type: RoomType.Room},
        {number: 3, floor: 1, reservations: [], schedule: null, scheduleId: scheduleId, type: RoomType.Room},
        {number: 4, floor: 1, reservations: [], schedule: null, scheduleId: scheduleId, type: RoomType.Room},
        {number: 5, floor: 1, reservations: [], schedule: null, scheduleId: scheduleId, type: RoomType.Room},
        
        {number: 11, floor: 1, reservations: [], schedule: null, scheduleId: scheduleId, type: RoomType.Apartment},
        {number: 12, floor: 1, reservations: [], schedule: null, scheduleId: scheduleId, type: RoomType.Apartment},
        {number: 13, floor: 1, reservations: [], schedule: null, scheduleId: scheduleId, type: RoomType.Apartment},
        {number: 14, floor: 1, reservations: [], schedule: null, scheduleId: scheduleId, type: RoomType.Apartment},
        {number: 15, floor: 1, reservations: [], schedule: null, scheduleId: scheduleId, type: RoomType.Apartment},
    ];

    const schedules: Schedule[] = [
        {id: 1, name: "Schedule 1", reservations: [], rooms: rooms},
        {id: 2, name: "Schedule 2", reservations: [], rooms: []},
        {id: 3, name: "Schedule 3", reservations: [], rooms: []}];
    
    rooms.forEach(r => r.schedule = schedules.find(s => s.id === r.scheduleId) ?? null);

    return(schedules.find((s) => s.id === scheduleId)?.rooms ?? []);
}

function darken(day: number): string{
    if(day % 2 === 0) return(" darken ");
    return "";
}

export const link: PageLink = {
    icon: _info.icon,
    route: '/room',
    element: <RoomIndexBody/>,
    params: '/:id'
};
