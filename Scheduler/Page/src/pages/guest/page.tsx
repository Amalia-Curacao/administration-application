import { ReactElement, Fragment } from "react";
import Person from "../../models/Person";
import References from "../../tools/References";
import PersonPrefix from "../../models/PersonPrefix";

const references: References = new References();

function Action(reservationId: number): boolean{
    const guest: Person = {
        id: undefined,
        prefix: PersonPrefix[references.GetSelect("prefix").current!.value! as keyof typeof PersonPrefix],
        firstName: references.GetInput("first-name").current?.value,
        lastName: references.GetInput("last-name").current?.value,
        age: Number(references.GetInput("age").current?.value),
        note: references.GetTextArea("note").current?.value,
        reservationId: reservationId,
        reservation: undefined
    };
    console.log(guest);
    
    return(true);
}

function Body({guest}: {guest: Person}): ReactElement{
    return(<>
        <table className="table table-bordered table-striped table-hover">
            <tbody>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Prefix
                            <select ref={references.GetSelect("prefix")} defaultValue={guest.prefix ? guest.prefix : PersonPrefix.Unknown} className="form-control">
                                {Object.values(PersonPrefix).map((value, index) => <option key={index} value={value}>{value}</option>)}
                            </select>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label>First Name
                            <input defaultValue={guest.firstName ? guest.firstName : ""} ref={references.GetInput("first-name")} type="text" className="form-control"/>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Last Name
                            <input defaultValue={guest.lastName ? guest.lastName : ""} ref={references.GetInput("last-name")} type="text" className="form-control"/>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label>Age
                            <input defaultValue={guest.age ? guest.age.toString() : ""} ref={references.GetInput("age")} type="number" className="form-control"/>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td colSpan={2} className="bg-primary text-secondary">
                        <label className="d-flex flex-column">Note
                            <textarea defaultValue={guest.note ? guest.note : ""} className="form-control" ref={references.GetTextArea("note")} />
                        </label>
                    </td>
                </tr>
            </tbody>
        </table>
    </>);
}

export default function Page(guest: Person): {action: () => boolean, body: ReactElement}{
    if(!guest.reservationId) throw new Error("Guest does not have a reservation");

    return({
        action: () => Action(guest.reservationId!),
        body: <Body guest={guest}/>
    });
}