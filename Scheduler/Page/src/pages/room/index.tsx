import { ReactElement, useEffect, useState } from "react";
import "../../scss/room.table.scss";
import { MdBedroomParent } from "react-icons/md";
import PageLink from "../../types/PageLink";
import Room from "../../models/Room";
import RoomType from "../../models/RoomType";
import { useParams } from "react-router-dom";
import { FaLongArrowAltLeft, FaLongArrowAltRight } from "react-icons/fa";
import axios from "axios";
import {default as Rooms} from "./table";
import {MapAll as MapRooms} from "../../mapping/room";

const _info = {name: "Rooms", icon: <MdBedroomParent/>};

function RoomIndexBody(): ReactElement {
    const { id } = useParams();
    if(!id) throw new Error("Schedule ID is undefined.");
    const [monthYear, setMonthYear] = useState(new Date()); // [1, 12]
    const [rooms, setRooms] = useState<Room[]>([]);
    useEffect(() => {
        axios.get(process.env.REACT_APP_API_URL + "/Rooms/Get/" + id)
        .then(async response => {
            setRooms(MapRooms(response.data as Room[]));
        })
        .catch(error => console.log(error));
    }, [id]);

    const groupedRooms = groupByRoomType(rooms);
    return(<>
        <div style={{borderRadius:"5px"}} className="p-3 m-3 mb-2 bg-primary d-flex flex-fill flex-row">
            <MonthYearSelector monthYear={monthYear} onChange={onMonthYearSelected}/>
        </div>
        <div className="p-3 pb-0 d-flex flex-column flex-fill">
            {Object.keys(groupedRooms).map((key, index) => 
                <Rooms key={index} monthYear={monthYear} showDates={index === 0} rooms={groupedRooms[key]}/>)}
            <div className="table-end" style={{borderRadius:"0 0 5px 5px"}}/>
        </div>
    </>);
    
    function onMonthYearSelected(monthYear: Date): void {
        setMonthYear(monthYear);
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

// #region Functions
function groupByRoomType(rooms: Room[]): {[type: string]: Room[]} {
    let groupedRooms: {[type: string]: Room[]} = {};
    rooms.forEach(r => {
        if(!r.type) r.type = RoomType.None;
        if(groupedRooms[r.type] === undefined) groupedRooms[r.type] = [];
        groupedRooms[r.type].push(r);
    });
    return groupedRooms;
}

// #endregion

export const link: PageLink = {
    icon: _info.icon,
    route: '/room',
    element: <RoomIndexBody/>,
    params: '/:id'
};
