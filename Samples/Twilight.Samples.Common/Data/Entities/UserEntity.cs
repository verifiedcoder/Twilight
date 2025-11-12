using System.ComponentModel.DataAnnotations;

namespace Twilight.Samples.Common.Data.Entities;

public class UserEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(256)]
    public string Forename { get; set; } = null!;

    [Required]
    [MaxLength(256)]
    public string Surname { get; set; } = null!;
}
