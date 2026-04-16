using System.Net;
using System.Net.Http.Json;

namespace ProductsApi.Tests;

public class UnitTest1 : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UnitTest1(CustomWebApplicationFactory factory)
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
                Assert.Equal(3500.00m, product.Preco);
            },
            product =>
            {
                Assert.Equal(2, product.Id);
                Assert.Equal("Mouse", product.Nome);
                Assert.Equal(99.90m, product.Preco);
            },
            product =>
            {
                Assert.Equal(3, product.Id);
                Assert.Equal("Teclado", product.Nome);
                Assert.Equal(199.90m, product.Preco);
            });
    }

    private sealed record ProductResponse(int Id, string Nome, decimal Preco);
}
