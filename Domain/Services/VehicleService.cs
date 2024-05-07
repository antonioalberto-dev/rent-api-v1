using Microsoft.EntityFrameworkCore;
using RentApi.Domain.Entities;
using RentApi.Domain.Interfaces;
using RentApi.Infra.Db;

namespace MinimalApi.Domain.Services;

public class VehicleService : IVehicleService

{
    private readonly DbContextInfra _context;
    public VehicleService(DbContextInfra context)
    {
        _context = context;
    }

    public void AddVehicle(Vehicle vehicle)
    {
        _context.Vehicles.Add(vehicle);
        _context.SaveChanges();
    }

    public List<Vehicle> AllVehicles(int? page = 1, string? desc = null, string? brand = null)
    {
        var query = _context.Vehicles.AsQueryable();
        if (!string.IsNullOrEmpty(desc))
            query = query.Where(v => EF.Functions.Like(v.Desc.ToLower(), $"%{desc}%"));

        if (page != null)
            query = query.Skip(((int)page - 1) * 10).Take(10);

        return query.ToList();
    }

    public void DeleteVehicle(Vehicle vehicle)
    {
        _context.Vehicles.Remove(vehicle);
        _context.SaveChanges();
    }

    public void UpdateVehicle(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        _context.SaveChanges();
    }

    public Vehicle? VehicleId(int id)
    {
        return _context.Vehicles.Where(v => v.Id == id).FirstOrDefault();
    }
}