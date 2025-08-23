using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimalApi.Domain.Services;
using MinimalApi.Domain.Dtos;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Enuns;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Domain.ModelViews;
using MinimalApi.Domain.Services;
using MinimalApi.Domain.Utilities;
using MinimalApi.Infrastructure.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using minimal_api.Domain.ModelViews;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
var stringConection = builder.Configuration.GetConnectionString("MySql");
var key = builder.Configuration.GetSection("Jwt").ToString();
if (string.IsNullOrEmpty(key)) key = "lalaland";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseMySql(stringConection, ServerVersion.AutoDetect(stringConection));
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT assim: Bearer {seu token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Json(new Home()))
.AllowAnonymous()
.WithTags("Home");

app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
{
    var adm = adminService.Login(loginDTO);
    if (adm != null)
    {
        string token = Auth.GenerateToken(key, adm);
        return Results.Ok(new AdminAuthenticate
        {
            Email = adm.Email,
            Role = (Roles)Enum.Parse(typeof(Roles), adm.Role),
            Token = token
        });
    }
    else
    {
        return Results.Unauthorized();
    }
})
.AllowAnonymous()
.WithTags("Auth");

app.MapPost("/admin", ([FromBody] AdminDTO adminDTO, IAdminService adminService) =>
{
    var validation = Helpers.AdminValidation(adminDTO);
    if (validation.Messages.Count > 0) return Results.BadRequest();

    var person = new Admin
    {
        Email = adminDTO.Email,
        Password = adminDTO.Password,
        Role = adminDTO.Role.ToString()
    };

    adminService.Add(person);

    return Results.Created($"/admin/{person.Id}", person);
})
.RequireAuthorization()
.WithTags("Employee");

app.MapGet("/admin", ([FromQuery] int? page, IAdminService adminService) =>
{
    var adms = new List<AdminModelView>();
    var administrators = adminService.GetAll(page);
    foreach (var adm in administrators)
    {
        adms.Add(new AdminModelView
        {
            Id = adm.Id,
            Email = adm.Email,
            Role = (Roles)Enum.Parse(typeof(Roles), adm.Role),

        });
    }
    return Results.Ok(adms);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
.WithTags("Employee");

app.MapGet("/admin/{id}", ([FromRoute] int id, IAdminService adminService) =>
{
    var person = adminService.GetById(id);

    if (person == null) return Results.NotFound();

    return Results.Ok(new AdminModelView
    {
        Id = person.Id,
        Email = person.Email,
        Role = (Roles)Enum.Parse(typeof(Roles), person.Role)
    });
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
.WithTags("Employee");

app.MapPut("/admin/{id}", ([FromRoute] int id, [FromBody] AdminDTO adminDTO, IAdminService adminService) =>
{
    var person = adminService.GetById(id);

    if (person == null) return Results.NotFound();

    var validation = Helpers.AdminValidation(adminDTO);
    if (validation.Messages.Count > 0) return Results.BadRequest();

    person.Email = adminDTO.Email;
    person.Password = adminDTO.Password;
    person.Role = adminDTO.Role.ToString();

    adminService.Update(person);

    return Results.Ok(person);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
.WithTags("Employee");

app.MapPost("/vehicle", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{
    var validation = Helpers.CarValidation(vehicleDTO);
    if (validation.Messages.Count > 0) return Results.BadRequest();

    var vehicle = new Vehicle

    {
        Name = vehicleDTO.Name,
        Brand = vehicleDTO.Brand,
        Year = vehicleDTO.Year
    };

    vehicleService.Add(vehicle);

    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "admin, editor" })
.WithTags("Vehicles");

app.MapGet("/vehicle", ([FromQuery] int? page, IVehicleService vehicleService) =>
{
    var vehicles = vehicleService.GetAll(page);
    return Results.Ok(vehicles);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "admin, editor" })
.WithTags("Vehicles");

app.MapGet("/vehicle/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetById(id);

    if (vehicle == null) return Results.NotFound();

    return Results.Ok(vehicle);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "admin, editor" })
.WithTags("Vehicles");

app.MapPut("/vehicle/{id}", ([FromRoute] int id, [FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetById(id);

    if (vehicle == null) return Results.NotFound();

    var validation = Helpers.CarValidation(vehicleDTO);
    if (validation.Messages.Count > 0) return Results.BadRequest();

    vehicle.Name = vehicleDTO.Name;
    vehicle.Brand = vehicleDTO.Brand;
    vehicle.Year = vehicleDTO.Year;

    vehicleService.Update(vehicle);

    return Results.Ok(vehicle);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
.WithTags("Vehicles");

app.MapDelete("/vehicle/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetById(id);

    if (vehicle == null) return Results.NotFound();

    vehicleService.Delete(id);

    return Results.Ok(vehicle);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
.WithTags("Vehicles");

app.Run();
