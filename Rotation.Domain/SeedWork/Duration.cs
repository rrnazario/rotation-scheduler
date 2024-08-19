namespace Rotation.Domain.SeedWork;

public class Duration
{
    public DurationType DurationType { get; private set; }
    public int Amount { get; private set; }
    public DateTime Begin { get; private set; }

    public DateTime End => DurationType switch
    {
        DurationType.Days => Begin.AddDays(Amount),
        DurationType.Weeks => Begin.AddDays(Amount * 7),
        _ => throw new NotImplementedException(),
    };

    public Duration(int amount, DurationType durationType, DateTime begin)
    {
        Amount = amount;
        DurationType = durationType;
        Begin = begin;
    }

    public bool IsValid()
    {
        if (Amount <= 0) return false;

        if (Begin < DateTime.Now.Date) return false;

        return true;
    }
}

public enum DurationType
{
    Days,
    Weeks
}
