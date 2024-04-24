var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (LoginDto user) =>
{
    if (user.Email.Equals("adm@test.com") && user.Password.Equals("123456"))
        return Results.Ok("Login realizado com sucesso");
    return Results.Unauthorized();
});

app.Run();
