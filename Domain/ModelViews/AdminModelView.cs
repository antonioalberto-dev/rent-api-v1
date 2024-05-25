using RentApi.Domain.Enuns;

namespace RentApi.Domain.ModelViews;

public record AdminModelView{
    public int Id { get; set; }

    public string Email { get; set; } = default!;

    public string Profile { get; set; } = default!;
    
}