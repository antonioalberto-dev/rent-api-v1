namespace RentApi.DTOs;

public record VehicleDto
{
    public string? Desc { get; set; }

    public string? Brand { get; set; }

    public int Year { get; set; }
}