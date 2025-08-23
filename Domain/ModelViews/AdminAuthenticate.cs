
using MinimalApi.Domain.Enuns;

namespace minimal_api.Domain.ModelViews
{
    public record AdminAuthenticate
    {
        public string Email { get; set; } = default!;
        public Roles Role { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}