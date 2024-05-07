using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Services;
using RentApi.Domain.Interfaces;
using RentApi.Domain.ModelViews;
using RentApi.DTOs;
using RentApi.Infra.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdminService, AdminService>();

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

app.MapGet("/", () => Results.Json(new Home()));

app.MapPost("/login", ([FromBody] LoginDto user, IAdminService adminService) =>
{
    if (adminService.Login(user) != null)
        return Results.Ok("Login realizado com sucesso");
    return Results.Unauthorized();
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
