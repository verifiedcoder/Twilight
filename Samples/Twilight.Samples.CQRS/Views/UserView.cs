namespace Twilight.Samples.CQRS.Views;

public record UserView(int ViewId, int UserId, string Forename, string Surname, string FullName, DateTimeOffset RegistrationDate);
