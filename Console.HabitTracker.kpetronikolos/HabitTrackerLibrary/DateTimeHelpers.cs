using System.Globalization;

namespace HabitTrackerLibrary;

public static class DateTimeHelpers
{
    public static List<string> NextWeeksDates() =>
        Enumerable.Range(-10, 10).Select(index =>
            DateTime.Now.AddDays(index).ToString("dd-MM-yy", new CultureInfo("en-US"))).ToList();
}
