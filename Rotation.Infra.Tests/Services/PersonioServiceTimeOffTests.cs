using FluentAssertions;
using Rotation.Infra.Services.Personio.Models;
using System.Text.Json;
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
            success = true,
            data =
            [
                new PersonioResponseData<dynamic>
                {
                    attributes = new Dictionary<string, dynamic>
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
                            JsonSerializer.Serialize(
                            new PersonioResponseData<PersonioEmployeeModels.PersonioEmployeeAttribute>
                                    {
                                        attributes =
                                            new Dictionary<string, PersonioEmployeeModels.PersonioEmployeeAttribute>
                                            {
                                                {
                                                    "id",
                                                    new PersonioEmployeeModels.PersonioEmployeeAttribute
                                                    {
                                                        label = "ID",
                                                        type = "standard",
                                                        value = "1"
                                                    }
                                                },
                                                {
                                                    "email",
                                                    new PersonioEmployeeModels.PersonioEmployeeAttribute
                                                    {
                                                        label = "email",
                                                        type = "standard",
                                                        value = "r@r.com"
                                                    }
                                                },
                                                {
                                                    "first_name",
                                                    new PersonioEmployeeModels.PersonioEmployeeAttribute
                                                    {
                                                        label = "first_name",
                                                        type = "standard",
                                                        value = "first_name"
                                                    }
                                                },
                                                {
                                                    "last_name",
                                                    new PersonioEmployeeModels.PersonioEmployeeAttribute
                                                    {
                                                        label = "last_name",
                                                        type = "standard",
                                                        value = "last_name"
                                                    }
                                                }
                                            }
                                    })
                        }
                    }
                }
            ]
        };

        var result = PersonioTimeOffResponse.Parse(personioResponse);

        result.Should().HaveCount(4);
        result.First().Id.Should().Be(personioResponse.data[0].attributes["id"]);
    }
}