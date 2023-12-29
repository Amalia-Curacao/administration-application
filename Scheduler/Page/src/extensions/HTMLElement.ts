// Purpose: 
// Adds a toggleHidden method to HTMLElements.
// Adds a setHidden method to HTMLElements.
export default HTMLElement;

declare global {
    interface HTMLElement {
        toggleHidden:() => boolean;
        setHidden:(hidden: boolean) => void;
    }
}

HTMLElement.prototype.toggleHidden = function () {
    this.hidden = !this.hidden;
    return this.hidden;
}

HTMLElement.prototype.setHidden = function (hidden: boolean) {
    this.hidden = hidden;
}