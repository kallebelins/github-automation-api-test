var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var products = new[]
{
    new Product(1, "Notebook", "Notebook para trabalho e estudos", 3500.00m, true),
    new Product(2, "Mouse", "Mouse sem fio ergonômico", 99.90m, true),
    new Product(3, "Teclado", "Teclado mecânico ABNT2", 199.90m, false)
};

app.MapGet("/produtos", () => Results.Ok(products));

app.Run();

public record Product(int Id, string Nome, string Descricao, decimal Preco, bool Ativo);
public partial class Program;
