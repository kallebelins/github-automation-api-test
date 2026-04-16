var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var products = new[]
{
    new Product(1, "Notebook", 3500.00m),
    new Product(2, "Mouse", 99.90m),
    new Product(3, "Teclado", 199.90m)
};

app.MapGet("/produtos", () => Results.Ok(products));

app.Run();

public record Product(int Id, string Nome, decimal Preco);
public partial class Program;
