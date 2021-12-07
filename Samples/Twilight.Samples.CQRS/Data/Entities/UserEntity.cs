namespace Twilight.Samples.CQRS.Data.Entities;

public class UserEntity
{
    public int Id { get; set; }

    public string Forename { get; set; } = null!;

    public string Surname { get; set; } = null!;
}
