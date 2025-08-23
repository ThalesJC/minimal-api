using minimalApi.Domain.ModelViews;
using MinimalApi.Domain.Dtos;

namespace MinimalApi.Domain.Utilities;

public static class Helpers
{
  public static ValidationErrors CarValidation(VehicleDTO vehicleDTO)
  {
    var validation = new ValidationErrors
    {
      Messages = new List<string>()
    };

    if (string.IsNullOrEmpty(vehicleDTO.Name))
    {
      validation.Messages.Add("O campo 'Nome' não pode estar vazio.");
    }

    if (string.IsNullOrEmpty(vehicleDTO.Brand))
    {
      validation.Messages.Add("O campo 'Marca' não pode estar vazio.");
    }

    if (!int.TryParse(vehicleDTO.Year?.ToString(), out int year) || year < 1950)
    {
      validation.Messages.Add("O campo 'Ano' não pode estar vazio ou deve ser maior que 1950.");
    }
    return validation;

  }
  public static ValidationErrors AdminValidation(AdminDTO adminDTO)
  {
    var validation = new ValidationErrors
    {
      Messages = new List<string>()
    };

    if (string.IsNullOrEmpty(adminDTO.Email))
    {
      validation.Messages.Add("O campo 'Email' não pode estar vazio.");
    }

    if (string.IsNullOrEmpty(adminDTO.Password))
    {
      validation.Messages.Add("O campo 'Password' não pode estar vazio.");
    }

    if (string.IsNullOrEmpty(adminDTO.Role))
    {
      validation.Messages.Add("O campo 'Role' não pode estar vazio.");
    }
    return validation;

  }

}
