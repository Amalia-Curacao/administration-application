namespace System;

public static class StringExtensions
{
	/// <summary>
	/// Parses TUI's string date time format to <see cref="DateTime"/>.
	/// </summary>
	public static DateTime ParseToDateTime(this string dateTime)
	{
		var splitDateTime = dateTime.Split('-', ' ', ':');
		var days = int.Parse(splitDateTime[0]);
		var months = int.Parse(splitDateTime[1]);
		var years = int.Parse(splitDateTime[2]);
		var hours = int.Parse(splitDateTime[3]);
		var minutes = int.Parse(splitDateTime[4]);
		var seconds = int.Parse(splitDateTime[5]);
		return new DateTime(years, months, days, hours, minutes, seconds);
	}
}