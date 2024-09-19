namespace Rotation.Infra.Services.Personio.Models;

/// <summary>
/// General models for Personio
/// </summary>
public static class PersonioModels
{
    public class PersonioResponse<TAttribute>
        where TAttribute : class
    {
        public bool success { get; set; }
        public PersonioResponseData<TAttribute>[] data { get; set; }
    }

    public class PersonioResponseData<TAttribute>
        where TAttribute : class
    {
        public string type { get; set; }
        public Dictionary<string, TAttribute> attributes { get; set; }
    }
}