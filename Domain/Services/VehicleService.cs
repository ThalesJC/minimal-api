using MinimalApi.Domain.Entities;
using MinimalApi.Infrastructure.DB;

namespace minimalApi.Domain.Services

{
    class VehicleService(DBContext context) : IVehicleService
    {
        private readonly DBContext _context = context;

    void IVehicleService.Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
        }

        void IVehicleService.Delete(int id)
        {
            var vehicle = _context.Vehicles.SingleOrDefault(v => v.Id == id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                _context.SaveChanges();
            }
        }

        List<Vehicle> IVehicleService.GetAll(int? page, string? name, string? brand)
        {
            var query = _context.Vehicles.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(v => v.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(v => v.Brand.Contains(brand));
            }

            int pageSize = 10;
            if (page != null)
            {
            query = query.Skip(((int)page - 1) * pageSize).Take(pageSize);
            }

            return query.ToList();
        }

        Vehicle? IVehicleService.GetById(int id)
        {
            return _context.Vehicles.SingleOrDefault(v => v.Id == id);
        }

        void IVehicleService.Update(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
        }
    }
}