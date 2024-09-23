using static Rotation.Infra.Services.Personio.Models.PersonioModels;

namespace Rotation.Infra.Services.Personio.Models;

public static class PersonioTimeOffModels
{
    public record GetTimeOffAsyncRequest(DateTime Start, DateTime End, string[] EmployeeIds)
    {
        public string ToParams()
            => $"?start_date={Start:yyyy-MM-dd}&end_date={End:yyyy-MM-dd}&employees[]={string.Join("&employees[]=", EmployeeIds)}";
    }

    public class PersonioTimeOffResponse()
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeEmail { get; set; }

        public static PersonioTimeOffResponse[] Parse(PersonioResponse<dynamic> personioResponse)
        {
            var response = new List<PersonioTimeOffResponse>();

            foreach (var personioResponseData in personioResponse.data)
            {

                int.TryParse(personioResponseData.attributes["id"].ToString(), out int id);
                DateTime.TryParse(personioResponseData.attributes["start_date"].ToString(), out DateTime startDate);
                DateTime.TryParse(personioResponseData.attributes["end_date"].ToString(), out DateTime endDate);

                PersonioEmployeeModels.PersonioEmployeeResponse employee =
                    PersonioEmployeeModels.PersonioEmployeeResponse.Parse(personioResponseData.attributes["employee"].ToString());

                var days = (endDate.Date - startDate.Date).TotalDays;

                for (int i = 0; i <= days; i++)
                {
                    var instance = PersonioResponseHelper.CreateInstance<PersonioTimeOffResponse>();

                    instance.Id = id;
                    instance.StartDate = startDate.AddDays(i);
                    instance.EndDate = instance.StartDate;
                    instance.EmployeeEmail = employee.email;
                    instance.EmployeeId = employee.id;

                    response.Add(instance);
                }
            }

            return response.ToArray();
        }
    }
}