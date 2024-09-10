using System.Reflection;

namespace Rotation.Infra.Services.Personio;

public static class PersonioResponseHelper
{
    public static T CreateInstance<T>()
        where T: class
    {
        var type = typeof(T);
        var ctor = type.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public,
            null,
            CallingConventions.HasThis,
            [],
            null);

        return (T)ctor.Invoke([]);
    }
}