import { ReactElement, Fragment } from "react";
import Person from "../../models/Person";
import References from "../../tools/References";
import PersonPrefix from "../../models/PersonPrefix";
import InputField from "../../components/inputField";

const references: References = new References();
// Temp
let id: number = 1;
// TODO gets new id from server
function getId(): number{
    return(++id);
}

function Action(reservationId: number, guestId: number): Person | undefined{
    const guest: Person = {
        id: guestId <= 0 ? getId() : guestId,
        prefix: PersonPrefix[references.GetSelect("prefix").current!.value! as keyof typeof PersonPrefix],
        firstName: references.GetInput("first-name").current?.value,
        lastName: references.GetInput("last-name").current?.value,
        age: Number(references.GetInput("age").current?.value),
        note: references.GetTextArea("note").current?.value,
        reservationId: reservationId,
        reservation: undefined
    };
    
    return(Validate(guest) ? guest : undefined);
}

function Validate(guest: Person): boolean{
    let isValid: boolean = true;
    
    const nullErrorMessage: string = "Required";
    if(!guest.firstName || guest.firstName === "") {
        references.GetSpan("first-name-validation")!.current!.innerText = nullErrorMessage;
        references.GetDiv("first-name-wrapper")!.current!.classList.add("border-danger");
        isValid = false;
    };
    if(!guest.lastName || guest.lastName === "") {
        references.GetSpan("last-name-validation")!.current!.innerText = nullErrorMessage;
        references.GetDiv("last-name-wrapper")!.current!.classList.add("border-danger");
        isValid = false;
    };

    if(guest.age! <= 0) {
        references.GetSpan("age-validation")!.current!.innerText = "Age cannot be negative or empty";
        references.GetDiv("age-wrapper")!.current!.classList.add("border-danger");
        isValid = false;
    }

    return (isValid);
}

function Body({guest}: {guest: Person}): ReactElement{
    return(<>
        <table className="w-100">
            <tbody>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label className="w-100">Prefix
                            <InputField refKey="prefix" references={references}>
                                <select ref={references.GetSelect("prefix")} defaultValue={guest.prefix ? guest.prefix : PersonPrefix.Unknown} className="form-control">
                                    {Object.values(PersonPrefix).map((value, index) => <option key={index} value={value}>{value}</option>)}
                                </select>
                            </InputField>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label>First Name
                            <InputField refKey="first-name" references={references}>
                                <input defaultValue={guest.firstName ? guest.firstName : ""} ref={references.GetInput("first-name")} type="text" className="form-control"/>
                            </InputField>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td className="bg-primary text-secondary">
                        <label>Last Name
                            <InputField refKey="last-name" references={references}>
                                <input defaultValue={guest.lastName ? guest.lastName : ""} ref={references.GetInput("last-name")} type="text" className="form-control"/>
                            </InputField>
                        </label>
                    </td>
                    <td className="bg-primary text-secondary">
                        <label>Age
                            <InputField refKey="age" references={references}>
                                <input defaultValue={guest.age ? guest.age.toString() : ""} ref={references.GetInput("age")} type="number" className="form-control"/>
                            </InputField>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td colSpan={2} className="bg-primary text-secondary">
                        <label className="d-flex flex-column">Note
                            <InputField refKey="note" references={references}>
                                <textarea defaultValue={guest.note ? guest.note : ""} className="form-control" ref={references.GetTextArea("note")} />
                            </InputField>
                        </label>
                    </td>
                </tr>
            </tbody>
        </table>
    </>);
}

export default function Page(guest: Person): {action: () => Person | undefined, body: ReactElement}{
    if(!guest.reservationId) throw new Error("Guest does not have a reservation");
    if(!guest.id) throw new Error("Guest does not have an id");

    return({
        action: () => Action(guest.reservationId!, guest.id!),
        body: <Body guest={guest}/>
    });
}