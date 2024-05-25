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

    public Admin Insert(Admin admin)
    {
        _context.AdminUsers.Add(admin);
        _context.SaveChanges();

        return admin;
    }

    public Admin Login(LoginDto loginDto)
    {
        return _context.AdminUsers.Where(a => a.Email == loginDto.Email && a.Password == loginDto.Password).FirstOrDefault();
    }

    public Admin? SearchAdmin(int id)
    {
        return _context.AdminUsers.Where(v => v.Id == id).FirstOrDefault();
    }

    public List<Admin> ViewAll(int? page)
    {
        var query = _context.AdminUsers.AsQueryable();

        if (page != null)
            query = query.Skip(((int)page - 1) * 10).Take(10);

        return query.ToList();
    }

    
}