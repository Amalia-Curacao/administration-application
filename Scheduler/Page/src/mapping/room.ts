import Room from "../models/Room";
import { MapAll as MapReservations } from "./reservation";

export default function Map(room: Room): Room
{
    return {
        ...room,
        reservations: MapReservations(room.reservations ?? [])
    }
}

export function MapAll(rooms: Room[]): Room[]
{
    return rooms.map(Map);
}