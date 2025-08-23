using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MinimalApi.Domain.Enuns;

namespace MinimalApi.Domain.Entities;

public class Admin
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [Required]
  [StringLength(255)]
  public string Email { get; set; } = default!;

  [Required]
  [StringLength(20)]
  public string Password { get; set; } = default!;

  [Required]
  [StringLength(10)]
  public string Role { get; set; } = default!;

}