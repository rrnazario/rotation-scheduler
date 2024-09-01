using FluentAssertions;
using static Rotation.Infra.Services.Personio.PersonioServiceModels;

namespace Rotation.Infra.Tests.Services;

public class PersonioServiceTests
{
    [Fact]
    public void PersonioEmployeeResponseParse_ShouldWork()
    {
        var personioResponse = new PersonioResponse
        {
            Success = true,
            Data = new[]
            {
                new PersonioResponseData
                {
                    Attributes = new Dictionary<string, PersonioResponseAttribute>
                    {
                        {
                            "id",
                            new PersonioResponseAttribute
                            {
                                Label = "ID",
                                Type = "standard",
                                Value = "1"
                            }
                        },
                        {
                            "email",
                            new PersonioResponseAttribute
                            {
                                Label = "email",
                                Type = "standard",
                                Value = "r@r.com"
                            }
                        },
                        {
                            "first_name",
                            new PersonioResponseAttribute
                            {
                                Label = "email",
                                Type = "standard",
                                Value = "r@r.com"
                            }
                        },
                        {
                            "last_name",
                            new PersonioResponseAttribute
                            {
                                Label = "email",
                                Type = "standard",
                                Value = "r@r.com"
                            }
                        }
                    }
                }
            }
        };

        var result = PersonioEmployeeResponse.Parse(personioResponse);

        result.Should().NotBeNull();
        result.Id.Should().Be(personioResponse.Data[0].Attributes["id"].Value);
        result.Email.Should().Be(personioResponse.Data[0].Attributes["email"].Value);
    }
}