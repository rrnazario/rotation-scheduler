using FluentAssertions;
using static Rotation.Infra.Services.Personio.PersonioModels;
using static Rotation.Infra.Services.Personio.PersonioTimeOffModels;

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
                        }
                    }
                }
            ]
        };

        var result = PersonioTimeOffResponse.Parse(personioResponse);

        result.Should().NotBeNull();
        //result.Id.Should().Be(personioResponse.Data[0].Attributes["id"].Value);
        //result..Should().Be(personioResponse.Data[0].Attributes["email"].Value);
    }
}