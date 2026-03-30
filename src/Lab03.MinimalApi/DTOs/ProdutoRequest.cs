namespace Lab03.MinimalApi.DTOs;

public class ProdutoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
    public bool Ativo { get; set; } = true;
}
