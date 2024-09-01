using System.Reflection;
using System.Text.Json.Serialization;

namespace Rotation.Infra.Services.Personio;

public static class PersonioServiceModels
{
    public class PersonioResponse
    {
        public bool Success { get; set; }
        public PersonioResponseData[] Data { get; set; }
    }

    public class PersonioResponseData
    {
        public Dictionary<string, PersonioResponseAttribute> Attributes { get; set; }
    }

    public class PersonioResponseAttribute
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        
        [JsonPropertyName("universal_id")]
        public string UniversalId { get; set; }
    }

    public class PersonioEmployeeResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public PersonioEmployeeResponse() { }

        public static PersonioEmployeeResponse Parse(PersonioResponse personioResponse)
        {
            var type = typeof(PersonioEmployeeResponse);
            var ctor = type.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public, 
            null,
            CallingConventions.HasThis,
            [],
            null);

            var instance = (PersonioEmployeeResponse)ctor.Invoke([]);

            foreach (var personioResponseData in personioResponse.Data)
            {
                instance.Id = personioResponseData.Attributes[nameof(instance.Id).ToLower()].Value;
                instance.Email = personioResponseData.Attributes[nameof(instance.Email).ToLower()].Value;
                instance.FirstName = personioResponseData.Attributes["first_name"].Value;
                instance.LastName = personioResponseData.Attributes["last_name"].Value;
            }

            return instance;
        }
    }

}
