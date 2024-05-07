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

#endregion

#region Vehicle
app.MapPost("/vehicles", ([FromBody] VehicleDto vehicleDto, IVehicleService vehicleService) =>
{
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

app.Run();

#endregion
