using MediatR;
using Rotation.Domain.SeedWork;

namespace Rotation.API.SeedWork;

public interface IApplicationEvent : IDomainEvent, IRequest;
