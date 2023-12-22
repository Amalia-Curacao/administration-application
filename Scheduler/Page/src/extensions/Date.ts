// Purpose: 
// isSameDay: compare two date elements to see if they are the same day
// oldest: returns the latest date of the two
// youngest: returns the earliest date of the two
// toDateOnlyString: returns the date element as a string
// toTimeOnlyString: returns the time element as a string
// isValidDate: returns true if the date is a valid date

export function isSameDay(date1: Date, date2: Date): boolean {
    date1 = new Date(date1);
    date2 = new Date(date2);
    return date1.getFullYear() === date2.getFullYear() &&
        date1.getMonth() === date2.getMonth() &&
        date1.getDate() === date2.getDate();
}

export function oldest(date1: Date, date2: Date): Date {
    return date1 > date2 ? date1 : date2;
}

export function youngest(date1: Date, date2: Date): Date {
    return date1 < date2 ? date1 : date2;
}

export function toDateOnlyString(date: Date): string {
    if(date instanceof Date) return date.toISOString().split('T')[0];
    else return date;
}

export function toTimeOnlyString(date: Date): string {
    if(date instanceof Date) return date.toISOString().split('T')[1];
    else return date
}

export function toDateTimeString(date: Date): string {
    return date.toISOString();
}