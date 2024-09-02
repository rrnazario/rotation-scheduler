using System.Reflection;
using System.Text.Json.Serialization;
using static Rotation.Infra.Services.Personio.PersonioEmployeeModels;
using static Rotation.Infra.Services.Personio.PersonioModels;

namespace Rotation.Infra.Services.Personio;

public static class PersonioTimeOffModels
{
    public class PersonioTimeOffPeriodAttribute
    {
        //public int Id { get; set; }
        //public string Status { get; set; }

        //[JsonPropertyName("start_date")]
        //public DateTime StartDate { get; set; }

        //[JsonPropertyName("end_date")]
        //public DateTime EndDate { get; set; }

        //public PersonioResponseData<PersonioEmployeeAttribute> Employee { get; set; }

        //public bool IsApproved() => Status == "approved";
    }


    public record GetTimeOffAsyncRequest(DateTime Start, DateTime End, string[] EmployeeIds)
    {
        public string ToParams()
            => $"?start_date={Start}&end_date={End}&employees[]={string.Join("&employees[]=", EmployeeIds)}";
    }

    public class PersonioTimeOffResponse()
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeEmail { get; set; }

        public static PersonioTimeOffResponse Parse(PersonioResponse<dynamic> personioResponse)
        {
            var type = typeof(PersonioTimeOffResponse);
            var ctor = type.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public,
            null,
            CallingConventions.HasThis,
            [],
            null);

            var instance = (PersonioTimeOffResponse)ctor.Invoke([]);

            foreach (var personioResponseData in personioResponse.Data)
            {
                instance.Id = personioResponseData.Attributes["id"];
                //instance.EmployeeId = personioResponseData.Attributes["employee"];
                //instance.EmployeeEmail = personioResponseData.Attributes["employee"];
                instance.StartDate = personioResponseData.Attributes["start_date"];
                instance.EndDate = personioResponseData.Attributes["end_date"];
            }

            return instance;
        }
    }
}
