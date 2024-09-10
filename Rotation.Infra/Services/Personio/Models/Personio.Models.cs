namespace Rotation.Infra.Services.Personio.Models;

/// <summary>
/// General models for Personio
/// </summary>
public static class PersonioModels
{
    public class PersonioResponse<TAttribute>
        where TAttribute : class
    {
        public bool Success { get; set; }
        public PersonioResponseData<TAttribute>[] Data { get; set; }
    }

    public class PersonioResponseData<TAttribute>
        where TAttribute : class
    {
        public string Type { get; set; }
        public Dictionary<string, TAttribute> Attributes { get; set; }
        //public TAttribute Attribute { get; set; }
    }

}
