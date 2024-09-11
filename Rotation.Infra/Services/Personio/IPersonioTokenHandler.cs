namespace Rotation.Infra.Services.Personio;

public interface IPersonioTokenHandler
{
    public string GetToken();
    public void SetToken(string value);
}

public class PersonioTokenHandler 
    : IPersonioTokenHandler
{
    private static string _token = "";

    public string GetToken() => _token;

    public void SetToken(string value) => _token = value;
}