import { forwardRef, useImperativeHandle } from "react";
import PageLink from "../../types/PageLink";
import { GrTableAdd } from "react-icons/gr";

const save = () => {
    console.log("save");
};

export interface handleSave {
    save: VoidFunction;
}

const ScheduleCreate = forwardRef<handleSave, {}>((props, ref) => {
    useImperativeHandle(ref, () => ({
        save
    }));
    
    return(<>
        <table className="table table-hover table-secondary table-borderless mb-0">
            <tbody className="bg-secondary">
                <tr className="bg-secondary">
                    <td className="bg-secondary p-0">
                        <input type="text" className="form-control bg-secondary border-primary"/>
                    </td>
                    
                        { props === undefined 
                        ?   <td className="bg-secondary">
                                <a onClick={save} className="btn btn-outline-success" href="#">Save</a> 
                            </td>
                        : <></> }
                </tr>
            </tbody>
        </table>    
    </>);
});;
export const link: PageLink = { name: "Schedule create", path: "/schedule/create", element: <></>, icon: <GrTableAdd/> };
export default ScheduleCreate;