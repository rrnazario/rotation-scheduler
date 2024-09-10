using Rotation.Application.Features.Users;
using Rotation.Domain.Users;

namespace Rotation.Application;

public static class TestData
{
    public static readonly List<IUser> Users = new()
    {
        new User("Rogerson","rogim"),
        new User("Thuane","thuane"),
        new User("Ronorgs","ronorgs"),
    };
}
