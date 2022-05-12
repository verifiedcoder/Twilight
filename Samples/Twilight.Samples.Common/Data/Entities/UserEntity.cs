using System.ComponentModel.DataAnnotations;

namespace Twilight.Samples.Common.Data.Entities;

public class UserEntity
{
    public int Id { get; set; }

    [Required]
    public string Forename { get; set; } = null!;

    [Required]
    public string Surname { get; set; } = null!;
}
