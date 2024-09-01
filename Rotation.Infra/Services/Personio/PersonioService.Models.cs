using System.Text.Json.Serialization;

namespace Rotation.Infra.Services.Personio;

public static class PersonioServiceModels
{
    public class PersonioResponse
    {
        public bool success { get; set; }
        public PersonioResponseData[] data { get; set; }
    }

    public class PersonioResponseData
    {
        Dictionary<string, PersonioResponseAttribute> attributes { get; set; }
    }

    public class PersonioResponseAttribute
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        
        [JsonPropertyName("universal_id")]
        public string UniversalId { get; set; }
    }

}
