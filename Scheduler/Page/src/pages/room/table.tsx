import { ReactElement } from "react";
import { compareTo, isSameDay } from "../../extensions/Date";
import Reservation from "../../models/Reservation";
import Room from "../../models/Room";
import RoomType from "../../models/RoomType";
import { MdBedroomParent } from "react-icons/md";
import { CheckOutCell, CheckInCell, EmptyCell, OccupiedCell } from "./cells";

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
                <span className="d-flex justify-content-center">
                    {day}
                </span>
                <span style={{fontSize: "12px"}} className="d-flex justify-content-center">
                    {["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"][date.getDay()]}
                </span>
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
        const occupied: Reservation | undefined =  reservations.find(r => compareTo(r.checkIn!, currentDate) === -1 && compareTo(r.checkOut!, currentDate) === 1);
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

