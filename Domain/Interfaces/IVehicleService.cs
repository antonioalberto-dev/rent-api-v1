using RentApi.Domain.Entities;
using RentApi.DTOs;

namespace RentApi.Domain.Interfaces;
public interface IVehicleService
{
    List<Vehicle> AllVehicles(int page = 1, string? desc = null, string? brand = null);
    Vehicle? VehicleId(int id);
    void AddVehicle(Vehicle vehicle);
    void UpdateVehicle(Vehicle vehicle);
    void DeleteVehicle(Vehicle vehicle);
}