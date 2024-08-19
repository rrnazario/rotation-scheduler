namespace Rotation.API.SeedWork;

public interface IEntity
{
    public Guid Id { get; protected set; }
    public bool Excluded { get; protected set; }

    public void RaiseEvent(IApplicationEvent e);// => _events.Add(e);
    public IReadOnlyCollection<IApplicationEvent> GetEventsList();// => _events.ToList();
    public void ClearEvents();
}
