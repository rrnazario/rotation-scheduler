using System.Text.Json.Serialization;
using static Rotation.Infra.Services.Personio.Models.PersonioModels;

namespace Rotation.Infra.Services.Personio.Models;

/// <summary>
/// Employee models for Personio
/// </summary>
public static class PersonioEmployeeModels
{
    public class PersonioEmployeeAttribute
    {
        public object value { get; set; }
        public string label { get; set; }
        public string type { get; set; }

        [JsonPropertyName("universal_id")]
        public string universalId { get; set; }
    }

    public class PersonioEmployeeResponse()
    {
        public string id { get; set; }
        public string email { get; set; }

        public static PersonioEmployeeResponse Parse(PersonioResponse<PersonioEmployeeAttribute> personioResponse)
        {
            var instance = PersonioResponseHelper.CreateInstance<PersonioEmployeeResponse>();

            foreach (var personioResponseData in personioResponse.data)
            {
                instance.id = personioResponseData.attributes[nameof(instance.id).ToLower()].value.ToString();
                instance.email = personioResponseData.attributes[nameof(instance.email).ToLower()].value.ToString();
            }

            return instance;
        }
    }
}
