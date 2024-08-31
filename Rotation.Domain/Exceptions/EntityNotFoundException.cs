namespace Rotation.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entity) 
        : base($"{entity} not found") { }
}
