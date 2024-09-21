namespace Rotation.Domain.SeedWork;

public record Calendar
{
    public IEnumerable<CalendarDay> Days { get; private set; } = [];

    public void FillDays(IEnumerable<CalendarDay> days) => Days = days;

    public CalendarAvailability GetAvailability(Duration duration)
    {
        var result = new List<CalendarDay>();
        var durationDays = duration.GetCurrentInterval(); //01/09 a 14/09

        foreach (var durationDay in durationDays)
        {
            var day = Days.FirstOrDefault(d => d.Date.Date == durationDay.Date);

            result.Add(day ?? new CalendarDay(durationDay.Date, true));
        }

        return new CalendarAvailability(result);
    }
}

public record CalendarDay(DateTime Date, bool Available);

public record CalendarAvailability(List<CalendarDay> Availability)
{
    public int AvailabilityPercentage => Availability.Count(v => v.Available) * 100 / Availability.Count;
}
