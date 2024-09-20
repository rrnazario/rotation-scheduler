using static Rotation.Infra.Services.Personio.Models.PersonioModels;

namespace Rotation.Infra.Services.Personio.Models;

public static class PersonioTimeOffModels
{
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
            var instance = PersonioResponseHelper.CreateInstance<PersonioTimeOffResponse>();

            foreach (var personioResponseData in personioResponse.data)
            {
                instance.Id = personioResponseData.attributes["id"];
                instance.StartDate = personioResponseData.attributes["start_date"];
                instance.EndDate = personioResponseData.attributes["end_date"];

                PersonioEmployeeModels.PersonioEmployeeResponse employee =
                    PersonioEmployeeModels.PersonioEmployeeResponse.Parse(personioResponseData.attributes["employee"]);

                instance.EmployeeEmail = employee.email;
                instance.EmployeeId = employee.id;
            }

            return instance;
        }
    }
}