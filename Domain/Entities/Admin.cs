using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.Domain.Entities;

class Admin
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [Required]
  [StringLength(255)]
  public string Email { get; set; } = default!;

  [StringLength(20)]
  public string Password { get; set; } = default!;

  [StringLength(10)]
  public string Role { get; set; } = default!;

}