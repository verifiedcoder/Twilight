namespace Twilight.Sample.CQRS
{
    public record UserRegisteredEventParameters(int UserId, string Forename, string Surname);
}
