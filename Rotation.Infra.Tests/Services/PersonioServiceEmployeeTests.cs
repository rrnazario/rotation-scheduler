using FluentAssertions;
using static Rotation.Infra.Services.Personio.PersonioEmployeeModels;
using static Rotation.Infra.Services.Personio.PersonioModels;

namespace Rotation.Infra.Tests.Services;

public class PersonioServiceEmployeeTests
{
    [Fact]
    public void PersonioEmployeeResponseParse_ShouldWork()
    {
        var personioResponse = new PersonioResponse<PersonioEmployeeAttribute>
        {
            Success = true,
            Data =
            [
                new PersonioResponseData<PersonioEmployeeAttribute>
                {
                    Attributes = new Dictionary<string, PersonioEmployeeAttribute>
                    {
                        {
                            "id",
                            new PersonioEmployeeAttribute
                            {
                                Label = "ID",
                                Type = "standard",
                                Value = "1"
                            }
                        },
                        {
                            "email",
                            new PersonioEmployeeAttribute
                            {
                                Label = "email",
                                Type = "standard",
                                Value = "r@r.com"
                            }
                        },
                        {
                            "first_name",
                            new PersonioEmployeeAttribute
                            {
                                Label = "email",
                                Type = "standard",
                                Value = "r@r.com"
                            }
                        },
                        {
                            "last_name",
                            new PersonioEmployeeAttribute
                            {
                                Label = "email",
                                Type = "standard",
                                Value = "r@r.com"
                            }   
                        }
                    }
                }
            ]
        };

        var result = PersonioEmployeeResponse.Parse(personioResponse);

        result.Should().NotBeNull();
        result.Id.Should().Be(personioResponse.Data[0].Attributes["id"].Value);
        result.Email.Should().Be(personioResponse.Data[0].Attributes["email"].Value);
    }
}