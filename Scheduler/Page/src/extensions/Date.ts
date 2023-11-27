// Purpose: 
// isSameDay: compare two date elements to see if they are the same day
// oldest: returns the latest date of the two
// youngest: returns the earliest date of the two

export function isSameDay(date1: Date, date2: Date): boolean {
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