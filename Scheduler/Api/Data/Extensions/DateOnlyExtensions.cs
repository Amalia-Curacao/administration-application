namespace System;

public static class DateOnlyExtensions
{
    /// <summary> Gets the difference in the two <see cref="DateOnly"/> objects. </summary>
    /// <returns> The full days between the two dates. </returns>
    public static int DaysDifference(this DateOnly dateOnly, DateOnly other)
        => (int)Math.Floor((dateOnly.ToDateTime(TimeOnly.MinValue) - other.ToDateTime(TimeOnly.MinValue)).TotalDays);
    
}
