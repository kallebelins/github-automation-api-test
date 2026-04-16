using System.Net;
using System.Net.Http.Json;

namespace ProductsApi.Tests;

public class ProductsApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductsApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProdutos_ReturnsExpectedProductList()
    {
        var response = await _client.GetAsync("/produtos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();

        Assert.NotNull(products);
        Assert.Equal(3, products.Count);
        Assert.Collection(products,
            product =>
            {
                Assert.Equal(1, product.Id);
                Assert.Equal("Notebook", product.Nome);
                Assert.Equal("Notebook para trabalho e estudos", product.Descricao);
                Assert.Equal(3500.00m, product.Preco);
                Assert.True(product.Ativo);
            },
            product =>
            {
                Assert.Equal(2, product.Id);
                Assert.Equal("Mouse", product.Nome);
                Assert.Equal("Mouse sem fio ergonômico", product.Descricao);
                Assert.Equal(99.90m, product.Preco);
                Assert.True(product.Ativo);
            },
            product =>
            {
                Assert.Equal(3, product.Id);
                Assert.Equal("Teclado", product.Nome);
                Assert.Equal("Teclado mecânico ABNT2", product.Descricao);
                Assert.Equal(199.90m, product.Preco);
                Assert.True(product.Ativo);
            });
    }

    private sealed record ProductResponse(int Id, string Nome, string Descricao, decimal Preco, bool Ativo);
}
