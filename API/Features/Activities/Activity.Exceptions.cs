namespace Rotation.API.Features.Activities;

public static class ActivityExceptions
{
    public class UserAlreadyAddedException : Exception
    {
        public UserAlreadyAddedException(string userEmail, Guid activityId)
            : base($"User {userEmail} already added to Activity {activityId}") { }
    }
}
