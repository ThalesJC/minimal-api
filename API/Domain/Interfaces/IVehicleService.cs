using MinimalApi.Domain.Entities;

interface IVehicleService
{
  void Add(Vehicle vehicle);

  void Delete(int id);

  List<Vehicle> GetAll(int? page = 1, string? name = "", string? brand = "");

  Vehicle? GetById(int id);

  void Update(Vehicle vehicle);
}