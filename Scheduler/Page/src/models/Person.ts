import Reservation from "./Reservation";

export default interface Person{
    id: number | undefined;
    prefix: PersonPrefix | undefined; 
    firstName: string | undefined;
    lastName: string | undefined;
    age: number | undefined;
    note: string | undefined;


    reservationId: number | undefined;
    reservation: Reservation | undefined;
}