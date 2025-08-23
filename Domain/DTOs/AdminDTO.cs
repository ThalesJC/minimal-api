using MinimalApi.Domain.Enuns;

namespace MinimalApi.Domain.Dtos

{
    public record AdminDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}