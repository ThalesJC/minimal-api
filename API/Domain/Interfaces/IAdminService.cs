using MinimalApi.Domain.Dtos;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interfaces;

interface IAdminService
{
  Admin? Login(LoginDTO loginDTO);
  void Add(Admin admin);
  void Delete(int id);
  List<Admin> GetAll(int? page);
  Admin? GetById(int id);

  void Update(Admin admin);
}