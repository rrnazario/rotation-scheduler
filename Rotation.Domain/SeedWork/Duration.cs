namespace Rotation.Domain.SeedWork;

public record Duration
{
    public DurationType DurationType { get; }
    public int Amount { get; }
    public DateTime Begin { get; }
    public DateTime CurrentBegin { get; private set; }

    private Duration() { }

    public Duration(int amount, DurationType durationType, DateTime begin)
    {
        Amount = amount;
        DurationType = durationType;
        Begin = CurrentBegin = begin;
    }

    public DateTime[] GetCurrentInterval()
    {
        var result = new List<DateTime>();
        
        var end = DurationType switch
        {
            DurationType.Days => Amount,
            DurationType.Weeks => Amount * 7,
            _ => throw new NotImplementedException(),
        };

        for (var i = 0; i < end; i++)
        {
            result.Add(CurrentBegin.AddDays(i));
        }

        return result.ToArray();
    }
    
    public void SetNextBegin()
    {
        var next = CurrentEnd();

        CurrentBegin = next;
    }

    public bool IsValid()
    {
        if (Amount <= 0) return false;

        if (Begin < DateTime.Now.Date) return false;

        return true;
    }
    
    public DateTime CurrentEnd() => DurationType switch
    {
        DurationType.Days => CurrentBegin.AddDays(Amount),
        DurationType.Weeks => CurrentBegin.AddDays(Amount * 7),
        _ => throw new NotImplementedException(),
    };
}

public enum DurationType
{
    Days,
    Weeks
}
