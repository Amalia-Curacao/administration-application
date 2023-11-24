import { ReactElement, useState } from "react";
import "../../scss/room.table.scss";
import { MdBedroomParent } from "react-icons/md";
import Schedule from "../../models/Schedule";
import PageLink from "../../types/PageLink";
import PageState from "../../types/PageState";
import Room from "../../models/Room";
import RoomType from "../../models/RoomType";
import { useParams } from "react-router-dom";
import Reservation from "../../models/Reservation";
import { isSameDay } from "../../extensions/Date";
import { FaLongArrowAltLeft, FaLongArrowAltRight } from "react-icons/fa";

const _info = {name: "Rooms", icon: <MdBedroomParent/>};

function RoomIndexBody(): ReactElement {
    const { id } = useParams();
    if(!id) throw new Error("Schedule ID is undefined.");
    const [monthYear, setMonthYear] = useState(new Date()); // [1, 12]
    const [rooms, setRooms] = useState(getRooms(parseInt(id)));
    const [state, setState] = useState(PageState.Default);
    let groupedRooms = groupByRoomType(rooms);

    function onMonthYearSelected(monthYear: Date): void {
        setMonthYear(monthYear);
    }

    return(<>
        <div style={{borderRadius:"5px"}} className="p-3 m-3 mb-2 bg-primary d-flex flex-fill flex-row">
            <MonthYearSelector monthYear={monthYear} onChange={onMonthYearSelected}/>
        </div>
        <div className="p-3 d-flex flex-column flex-fill">
            {Object.keys(groupedRooms).map((key, index) => <Tables key={index} monthYear={monthYear} showDates={index === 0} rooms={groupedRooms[parseInt(key)]}/>)}
        </div>
    </>);
}

// #region Elements

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
                    <RoomAvailibilty monthYear={monthYear} reservations={r.reservations ?? []}/>
                </tr>)}
            </tbody>
        </table>
    </>);
}

function RoomAvailibilty({monthYear, reservations}: {monthYear: Date, reservations: Reservation[]}): ReactElement {
    const date = new Date(monthYear.getFullYear(), monthYear.getMonth() + 1, 0).getDate();
    function Day({day}: {day: number}): ReactElement {
        const date = new Date(monthYear.getFullYear(), monthYear.getMonth(), day);
        const checkIn: Reservation | undefined = reservations.find(r => isSameDay(date, r.checkIn!));
        const checkOut: Reservation | undefined = reservations.find(r => isSameDay(date, r.checkOut!));
        const occupied: Reservation | undefined =  reservations.find(r => (r.checkIn! < date) && (r.checkOut! > date));

        function Cells(): ReactElement {
            enum CellType {Occupied, CheckIn, CheckOut, None}
            const {type1, type2} = getTypes();

            function getTypes(): {type1: CellType, type2: CellType} {
                let {type1, type2}: {type1: CellType, type2: CellType} = {type1: CellType.None, type2: CellType.None};
                if(occupied) { type1 = CellType.Occupied; type2 = CellType.Occupied; }
                if(checkIn !== undefined) type2 = CellType.CheckIn;
                if(checkOut !== undefined) type1 = CellType.CheckOut;
                return({type1: type1, type2: type2});
            }

            function Cell({type, left}: {type: CellType, left: boolean}): ReactElement {

                function getStyle(): string {
                    switch(type) {
                        case CellType.CheckIn: return("check-in");
                        case CellType.CheckOut: return("check-out");
                        case CellType.Occupied: return("occupied");
                        case CellType.None: return("");
                    }
                }

                return(<>
                    <div className={"flex-fill " + getStyle() + (left ? " left " : " right ")}></div>
                </>);
            }

            return(<>
                <td style={{overflow:"visible"}} className={"p-0 d-flex flex-fill border-dark border-1"}>
                    <Cell left={true} type={type1}/>
                    <Cell left={false} type={type2}/>
                </td>
            </>);
        }
        return(<>
            <Cells/>
        </>);
    }
    let days: ReactElement[] = [];
    for(let i = 1; i <= date; i++) days.push(<Day key={i} day={i}/>);


    return(<>
        <tr className="d-flex flex-fill bg-secondary p-0">
            {days}
        </tr>
    </>);
    
}

function Dates({monthYear}: {monthYear: Date}): ReactElement {
    const date = new Date(monthYear.getFullYear(), monthYear.getMonth() + 1, 0).getDate();

    function Day({day}: {day: number}): ReactElement {
        const date = new Date(monthYear.getFullYear(), monthYear.getMonth(), day);
        return(
            <td className="d-flex flex-fill justify-content-center flex-column bg-primary text-secondary darken-on-hover p-2">
                <div className="d-flex justify-content-center">
                    {day}
                </div>
                <div style={{fontSize: "12px"}} className="d-flex justify-content-center">
                    {["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"][date.getDay()]}
                </div>
            </td>
        );
    }
        let days: ReactElement[] = [];
        for(let i = 1; i <= date; i++) days.push(<Day key={i} day={i}/>);

    return(<>
        <tr className="d-flex p-0">
            {days}
        </tr>
    </>);
}

function MonthYearSelector({monthYear, onChange}: {monthYear: Date, onChange: (monthYear: Date) => void}): ReactElement {
    const arrowClass = "btn btn-secondary me-2 justify-content-center";
    return(<>
        <div className="d-flex flex-fill flex-row justify-content-center align-items-center">
            <button className={arrowClass} onClick={() => onChange(new Date(monthYear.getFullYear(), monthYear.getMonth() - 1, 1))}>
                <FaLongArrowAltLeft />
            </button>
            <h5 className="text-center text-secondary me-2 ms-2">{monthYear.toLocaleString('default', { month: 'long' }) + " " + monthYear.getFullYear()}</h5>
            <button className={arrowClass} onClick={() => onChange(new Date(monthYear.getFullYear(), monthYear.getMonth() + 1, 1))}>
                <FaLongArrowAltRight />
            </button>
        </div>
    </>);
}

// #endregion

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
                roomtype: RoomType.Room,
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
                roomtype: RoomType.Room,
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

export const link: PageLink = {
    icon: _info.icon,
    route: '/room',
    element: <RoomIndexBody/>,
    params: '/:id'
};
