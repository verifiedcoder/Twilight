namespace Twilight.Samples.CQRS.Features.RegisterUser;

public sealed record UserRegisteredEventParameters(int UserId, string Forename, string Surname);

