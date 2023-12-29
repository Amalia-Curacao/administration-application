import { ReactElement } from "react";

function SaveButton({onSave, onReturn, onFailure}: {onSave: () => object | undefined, onReturn: VoidFunction, onFailure: VoidFunction}): ReactElement {
    return(
        <div className="btn-group float-end">
            <button onClick={onReturn} className="btn btn-outline-danger">Back</button>
            <button onClick={() => {onSave() ? onReturn() : onFailure()}} className="btn btn-outline-success">
                Save
            </button>
        </div>);
}

export default SaveButton;