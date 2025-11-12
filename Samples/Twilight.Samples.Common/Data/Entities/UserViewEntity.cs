using System.ComponentModel.DataAnnotations;

namespace Twilight.Samples.Common.Data.Entities;

public class UserViewEntity
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(256)]
    public string Forename { get; set; } = null!;

    [Required]
    [MaxLength(256)]
    public string Surname { get; set; } = null!;

    [Required]
    [MaxLength(513)]
    public string FullName { get; set; } = null!;

    [Required]
    public DateTimeOffset? RegistrationDate { get; set; }
}
