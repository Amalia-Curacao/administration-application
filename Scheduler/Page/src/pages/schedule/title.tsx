import { ReactElement } from "react";

function PageTitle({name, icon}: {name: string, icon: ReactElement}): ReactElement { 
    return(
    <div className="d-flex justify-content-between">
        <h1>{name}</h1>
        <div className="h1">{icon}</div>
    </div>);
}

export default PageTitle;