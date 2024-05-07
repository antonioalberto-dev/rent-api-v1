using RentApi.Domain.Entities;
using RentApi.DTOs;

namespace RentApi.Domain.Interfaces;
public interface IAdminService
{
    Admin? Login(LoginDto loginDto);
}