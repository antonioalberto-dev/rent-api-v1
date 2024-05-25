using RentApi.Domain.Entities;
using RentApi.DTOs;

namespace RentApi.Domain.Interfaces;
public interface IAdminService
{
    Admin? Login(LoginDto loginDto);

    Admin Insert(Admin admin);

    List<Admin> ViewAll(int? page);

    Admin? SearchAdmin(int id);
}