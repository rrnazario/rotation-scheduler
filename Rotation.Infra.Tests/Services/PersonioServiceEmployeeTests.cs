using FluentAssertions;
using static Rotation.Infra.Services.Personio.Models.PersonioEmployeeModels;
using static Rotation.Infra.Services.Personio.Models.PersonioModels;

namespace Rotation.Infra.Tests.Services;

public class PersonioServiceEmployeeTests
{
    [Fact]
    public void PersonioEmployeeResponseParse_ShouldWork()
    {
        var personioResponse = new PersonioResponse<PersonioEmployeeAttribute>
        {
            success = true,
            data =
            [
                new PersonioResponseData<PersonioEmployeeAttribute>
                {
                    attributes = new Dictionary<string, PersonioEmployeeAttribute>
                    {
                        {
                            "id",
                            new PersonioEmployeeAttribute
                            {
                                label = "ID",
                                type = "standard",
                                value = "1"
                            }
                        },
                        {
                            "email",
                            new PersonioEmployeeAttribute
                            {
                                label = "email",
                                type = "standard",
                                value = "r@r.com"
                            }
                        },
                        {
                            "first_name",
                            new PersonioEmployeeAttribute
                            {
                                label = "email",
                                type = "standard",
                                value = "r@r.com"
                            }
                        },
                        {
                            "last_name",
                            new PersonioEmployeeAttribute
                            {
                                label = "email",
                                type = "standard",
                                value = "r@r.com"
                            }   
                        }
                    }
                }
            ]
        };

        var result = PersonioEmployeeResponse.Parse(personioResponse);

        result.Should().NotBeNull();
        result.id.Should().Be(personioResponse.data[0].attributes["id"].value.ToString());
        result.email.Should().Be(personioResponse.data[0].attributes["email"].value.ToString());
    }
}