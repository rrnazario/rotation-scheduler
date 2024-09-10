using FluentAssertions;
using Rotation.Infra.Services.Personio.Models;
using static Rotation.Infra.Services.Personio.Models.PersonioModels;
using static Rotation.Infra.Services.Personio.Models.PersonioTimeOffModels;

namespace Rotation.Infra.Tests.Services;

public class PersonioServiceTimeOffTests
{
    [Fact]
    public void PersonioTimeOffResponseParse_ShouldWork()
    {
        var personioResponse = new PersonioResponse<dynamic>
        {
            Success = true,
            Data =
            [
                new PersonioResponseData<dynamic>
                {
                    Attributes = new Dictionary<string, dynamic>
                    {
                        {
                            "id",
                            12345
                        },
                        {
                            "status",
                            "approved"
                        },
                        {
                            "start_date",
                            DateTime.Parse("2017-12-27T00:00:00+0100")
                        },
                        {
                            "end_date",
                            DateTime.Parse("2017-12-30T00:00:00+0100")
                        },
                        {
                            "employee",
                            new PersonioResponse<PersonioEmployeeModels.PersonioEmployeeAttribute>
                            {
                                Data =
                                [
                                    new PersonioResponseData<PersonioEmployeeModels.PersonioEmployeeAttribute>
                                    {
                                        Attributes =
                                            new Dictionary<string, PersonioEmployeeModels.PersonioEmployeeAttribute>
                                            {
                                                {
                                                    "id",
                                                    new PersonioEmployeeModels.PersonioEmployeeAttribute
                                                    {
                                                        Label = "ID",
                                                        Type = "standard",
                                                        Value = "1"
                                                    }
                                                },
                                                {
                                                    "email",
                                                    new PersonioEmployeeModels.PersonioEmployeeAttribute
                                                    {
                                                        Label = "email",
                                                        Type = "standard",
                                                        Value = "r@r.com"
                                                    }
                                                },
                                                {
                                                    "first_name",
                                                    new PersonioEmployeeModels.PersonioEmployeeAttribute
                                                    {
                                                        Label = "email",
                                                        Type = "standard",
                                                        Value = "r@r.com"
                                                    }
                                                },
                                                {
                                                    "last_name",
                                                    new PersonioEmployeeModels.PersonioEmployeeAttribute
                                                    {
                                                        Label = "email",
                                                        Type = "standard",
                                                        Value = "r@r.com"
                                                    }
                                                }
                                            }
                                    }
                                ]
                            }
                        }
                    }
                }
            ]
        };

        var result = PersonioTimeOffResponse.Parse(personioResponse);

        result.Should().NotBeNull();
        result.Id.Should().Be(personioResponse.Data[0].Attributes["id"]);
        result.EmployeeEmail.Should()
            .Be(personioResponse.Data[0].Attributes["employee"].Data[0].Attributes["email"].Value);
    }
}