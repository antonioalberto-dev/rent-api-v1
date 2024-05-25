using RentApi.Domain.Enuns;

namespace RentApi.DTOs;

public class AdminDto
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public Perfil? Profile { get; set; }
}