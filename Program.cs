using MinimalApi.Domain.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Ola mundo");
app.MapGet("/login", (LoginDTO loginDTO) =>
{
    if (loginDTO.Email == "admi@email.com.br" && loginDTO.Password == "S3nh@ F0rt&")
    {
        return Results.Ok("Login Realizado com sucesso!");
    }
    else
    {
        return Results.Unauthorized();
    }

});

app.Run();
