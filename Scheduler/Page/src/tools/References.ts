import { RefObject, createRef } from "react";

export default class References {
    constructor() { 
        this.references = {};
    }
    private references: { [key: string]: RefObject<undefined> };

    public Get(key: string): RefObject<undefined> {
        return this.references[key] = this.references[key] || createRef();
    }

    public GetInput(key: string): RefObject<HTMLInputElement> {
        return (this.references[key] = this.references[key] || createRef()) as unknown as RefObject<HTMLInputElement>;
    }

    public GetTextArea(key: string): RefObject<HTMLTextAreaElement> {
        return (this.references[key] = this.references[key] || createRef()) as unknown as RefObject<HTMLTextAreaElement>;
    }

    public GetSelect(key: string): RefObject<HTMLSelectElement> {
        return (this.references[key] = this.references[key] || createRef()) as unknown as RefObject<HTMLSelectElement>;
    }
}