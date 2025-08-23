using MinimalApi.Domain.Enuns;

namespace MinimalApi.Domain.ModelViews

{
  public record AdminModelView
  {
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public Roles Role { get; set; } = default!;
  }
}