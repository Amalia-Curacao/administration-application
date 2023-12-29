import { Fragment, ReactElement } from "react";
import { isSameDay } from "../../extensions/Date";
import Room from "../../models/Room";
import RoomType from "../../models/RoomType";
import { MdBedroomParent } from "react-icons/md";
import Cells from "./cells";

export default function Tables({rooms, monthYear, showDates}: {rooms: Room[], monthYear: Date, showDates: boolean}): ReactElement {
    if(rooms.length === 0) return(<Fragment/>);
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
                    <Cells key={index} monthYear={monthYear} room={r}/>
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

        return(<td style={{borderRadius: borderRadius}} className={cellClass + colorClass}>
            <span className="d-flex justify-content-center">
                {day}
            </span>
            <span style={{fontSize: "12px"}} className="d-flex justify-content-center">
                {["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"][date.getDay()]}
            </span>
        </td>);
    }
}


