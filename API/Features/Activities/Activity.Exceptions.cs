namespace Rotation.API.Features.Activities;

public static class ActivityExceptions
{
    public class UserAlreadyAddedException : Exception
    {
        public UserAlreadyAddedException(Guid userId, Guid activityId)
            : base($"User {userId} already added to Activity {activityId}") { }
    }
}
