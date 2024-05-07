using RentApi.Domain.Entities;
using RentApi.Domain.Interfaces;
using RentApi.DTOs;
using RentApi.Infra.Db;

namespace MinimalApi.Domain.Services;

public class AdminService : IAdminService

{
    private readonly DbContextInfra _context;
    public AdminService(DbContextInfra context)
    {
        _context = context;
    }

    public Admin Login(LoginDto loginDto)
    {
        return _context.AdminUsers.Where(a => a.Email == loginDto.Email && a.Password == loginDto.Password).FirstOrDefault();
    }
}