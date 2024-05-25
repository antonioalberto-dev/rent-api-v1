using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Services;
using RentApi.Domain.Entities;
using RentApi.Domain.Interfaces;
using RentApi.Domain.ModelViews;
using RentApi.DTOs;
using RentApi.Infra.Db;

# region Builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContextInfra>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();

#endregion

# region Home

app.MapGet("/", () => Results.Json(new Home()));

# endregion

# region Admin
app.MapPost("/admin/login", ([FromBody] LoginDto user, IAdminService adminService) =>
{
    if (adminService.Login(user) != null)
        return Results.Ok("Login realizado com sucesso");
    return Results.Unauthorized();
}).WithTags("Admin");

app.MapPost("/admin", ([FromBody] AdminDto adminDto, IAdminService adminService) =>
{
    var validation = new ErrorValidations
    {
        Messages = new List<string>()
    };

    if (string.IsNullOrEmpty(adminDto.Email)) validation.Messages.Add("Email não pode ser vazio!");
    if (string.IsNullOrEmpty(adminDto.Password)) validation.Messages.Add("Senha não pode estar em branco!");
    if (adminDto.Profile.ToString() == null) validation.Messages.Add("O Perfil deve ser informado!");

    if (validation.Messages.Count > 0) return Results.BadRequest(validation);

    var admin = new Admin
    {
        Email = adminDto.Email,
        Password = adminDto.Password,
        Profile = adminDto.Profile.ToString()
    };

    return Results.Created($"/admin/{admin.Id}", admin);
}).WithTags("Admin");

app.MapGet("/admin/all", ([FromQuery] int? page, IAdminService adminService) =>
{
    return Results.Ok(adminService.ViewAll(page));
}).WithTags("Admin");

app.MapGet("/admin/{id}", ([FromQuery] int id, IAdminService adminService) =>
{
    var admin = adminService.SearchAdmin(id);

    if (admin == null) return Results.NotFound("Admin não encontrado");

    return Results.Ok(admin);
}).WithTags("Admin");

#endregion

#region Vehicle

ErrorValidations validationVehicle(VehicleDto vehicleDto)
{
    var errorValidationMessages = new ErrorValidations
    {
        Messages = new List<string>()
    };

    if (string.IsNullOrEmpty(vehicleDto.Desc))
        errorValidationMessages.Messages.Add("Precisa ser informado o modelo do veículo.");
    if (string.IsNullOrEmpty(vehicleDto.Brand))
        errorValidationMessages.Messages.Add("Precisa ser informada a marca do veículo.");
    if (vehicleDto.Year < 1950)
        errorValidationMessages.Messages.Add("Veículo muito antigo! Somente aceito os veículos com ano acima de 1950.");

    return errorValidationMessages;
}

app.MapPost("/vehicles", ([FromBody] VehicleDto vehicleDto, IVehicleService vehicleService) =>
{
    var validation = validationVehicle(vehicleDto);
    if (validation.Messages.Count > 0) return Results.BadRequest(validation);

    var vehicle = new Vehicle
    {
        Desc = vehicleDto.Desc,
        Brand = vehicleDto.Brand,
        Year = vehicleDto.Year
    };
    vehicleService.AddVehicle(vehicle);

    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
}).WithTags("Vehicles");

app.MapGet("/vehicles", ([FromQuery] int? page, IVehicleService vehicleService) =>
{
    var vehicles = vehicleService.AllVehicles(page);

    if (vehicles.Count == 0) return Results.Ok("Sem veículos cadastrados");

    return Results.Ok(vehicles);
}).WithTags("Vehicles");

app.MapGet("/vehicles/{id}", ([FromQuery] int id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.VehicleId(id);

    if (vehicle == null) return Results.NotFound("Veículo não encontrado");

    return Results.Ok(vehicle);
}).WithTags("Vehicles");

app.MapPut("/vehicles/{id}", ([FromRoute] int id, VehicleDto vehicleDto, IVehicleService vehicleService) =>
{

    var validation = validationVehicle(vehicleDto);
    if (validation.Messages.Count > 0) return Results.BadRequest(validation);

    var vehicle = vehicleService.VehicleId(id);
    if (vehicle == null) return Results.NotFound("Veículo não encontrado");

    vehicle.Desc = vehicleDto.Desc;
    vehicle.Brand = vehicleDto.Brand;
    vehicle.Year = vehicleDto.Year;

    vehicleService.UpdateVehicle(vehicle);

    return Results.Ok(vehicle);
}).WithTags("Vehicles");

app.MapDelete("/vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.VehicleId(id);
    if (vehicle == null) return Results.NotFound("Veículo não encontrado");

    vehicleService.DeleteVehicle(vehicle);

    return Results.Ok("Os dados do veículo foram deletados");
}).WithTags("Vehicles");
#endregion

#region App

app.UseSwagger();
app.UseSwaggerUI();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

#endregion
