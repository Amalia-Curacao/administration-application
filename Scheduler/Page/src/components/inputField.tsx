import { ReactElement } from "react";
import References from "../tools/References";

// This component is used to wrap input fields and add a validation span.
// To access the input field, use references.GetInput(refKey).
// To access the validation span, use references.GetSpan(refKey + "-validation").
// To access the wrapper div, use references.GetDiv(refKey + "-wrapper").
export default function InputField({refKey, children, references}: {refKey: string, children: ReactElement, references: References}): ReactElement {
    return(<div ref={references.GetDiv(refKey + "-wrapper")} className="bg-secondary border border-2" style={{borderRadius: "5px"}}>
        {children}
        <span key={refKey} className="text-danger bg-secondary" ref={references.GetSpan(refKey + "-validation")}/>
    </div>);
}