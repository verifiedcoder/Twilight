namespace Twilight.Samples.Common.Data.Entities;

public class UserViewEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Forename { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateTimeOffset? RegistrationDate { get; set; }
}
