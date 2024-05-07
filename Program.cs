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
    var vehicle = new Vehicle{
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

    return Results.Ok(vehicles);
}).WithTags("Vehicles");
#endregion

#region App

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

#endregion
