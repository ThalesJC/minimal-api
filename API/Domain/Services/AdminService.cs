using MinimalApi.Domain.Dtos;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Infrastructure.DB;

namespace MinimalApi.Domain.Services;

class AdminService(DBContext context) : IAdminService
{
  private readonly DBContext _context = context;

  public Admin? GetById(int id)
  {
    return _context.Administrators.SingleOrDefault(adm => adm.Id == id);
  }

  public Admin? Login(LoginDTO loginDTO)
  {
    return _context.Administrators.Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password).FirstOrDefault();
  }

  public void Update(Admin admin)
  {
    _context.Administrators.Update(admin);
    _context.SaveChanges();
  }

  void IAdminService.Add(Admin admin)
  {
    _context.Administrators.Add(admin);
    _context.SaveChanges();
  }

  void IAdminService.Delete(int id)
  {
    var person = _context.Administrators.SingleOrDefault(v => v.Id == id);
    if (person != null)
    {
      _context.Administrators.Remove(person);
      _context.SaveChanges();
    }
  }


  List<Admin> IAdminService.GetAll(int? page)
  {
    var query = _context.Administrators.AsQueryable();

    int pageSize = 10;
    if (page != null)
    {
      query = query.Skip(((int)page - 1) * pageSize).Take(pageSize);
    }

    return query.ToList();
  }

}