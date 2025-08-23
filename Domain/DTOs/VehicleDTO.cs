namespace MinimalApi.Domain.Dtos

{
    public record VehicleDTO
    {
        public string Name { get; set; } = default!;
        public string Brand { get; set; } = default!;
        public string Year { get; set; } = default!;
    }
}