using System.Net;
using System.Net.Http.Json;
using Lab03.MinimalApi.Data;
using Lab03.MinimalApi.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lab03.MinimalApi.Tests;

public class ProdutoEndpointsTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly string _databaseName;

    public ProdutoEndpointsTests()
    {
        _databaseName = "TestDb_" + Guid.NewGuid();
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Find and remove any DbContextOptions or DbContext registrations for SQL Server
                var descriptorsToRemove = services
                    .Where(d => 
                        (d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                         d.ServiceType == typeof(DbContextOptions) ||
                         d.ServiceType == typeof(AppDbContext) ||
                         d.ServiceType.Name.Contains("DbContextOptions")))
                    .ToList();
                
                foreach (var descriptor in descriptorsToRemove)
                    services.Remove(descriptor);

                // Clear any EF Core service providers that might have SQL Server registered
                var efServices = services.Where(d => 
                    d.ServiceType.FullName?.Contains("EntityFrameworkCore") == true &&
                    d.ServiceType.FullName?.Contains("Internal") == true).ToList();
                foreach (var svc in efServices)
                    services.Remove(svc);

                // Add InMemory DbContext unique per test instance (new Guid per constructor call)
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(_databaseName));
            });
        });
        _client = _factory.CreateClient();
        
        // Initialize database
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();
        }
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithEmptyList()
    {
        var response = await _client.GetAsync("/api/produtos");
        response.EnsureSuccessStatusCode();
        var produtos = await response.Content.ReadFromJsonAsync<List<ProdutoResponse>>();
        Assert.NotNull(produtos);
        Assert.Empty(produtos);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreated()
    {
        var request = new ProdutoRequest
        {
            Nome = "Produto Teste",
            Descricao = "Descrição do produto",
            Preco = 99.99m,
            Quantidade = 10,
            Ativo = true
        };

        var response = await _client.PostAsJsonAsync("/api/produtos", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var produto = await response.Content.ReadFromJsonAsync<ProdutoResponse>();
        Assert.NotNull(produto);
        Assert.Equal("Produto Teste", produto.Nome);
        Assert.True(produto.Id > 0);
    }

    [Fact]
    public async Task Create_WithInvalidData_ReturnsBadRequest()
    {
        var request = new ProdutoRequest
        {
            Nome = "",
            Preco = -1m,
            Quantidade = -5
        };

        var response = await _client.PostAsJsonAsync("/api/produtos", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ExistingProduct_ReturnsOk()
    {
        var createRequest = new ProdutoRequest
        {
            Nome = "Produto GetById",
            Preco = 50m,
            Quantidade = 5
        };
        var createResponse = await _client.PostAsJsonAsync("/api/produtos", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<ProdutoResponse>();

        var response = await _client.GetAsync($"/api/produtos/{created!.Id}");
        response.EnsureSuccessStatusCode();

        var produto = await response.Content.ReadFromJsonAsync<ProdutoResponse>();
        Assert.NotNull(produto);
        Assert.Equal("Produto GetById", produto.Nome);
    }

    [Fact]
    public async Task GetById_NonExistingProduct_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/produtos/99999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_ExistingProduct_ReturnsOk()
    {
        var createRequest = new ProdutoRequest { Nome = "Original", Preco = 10m, Quantidade = 1 };
        var createResponse = await _client.PostAsJsonAsync("/api/produtos", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<ProdutoResponse>();

        var updateRequest = new ProdutoRequest { Nome = "Atualizado", Preco = 20m, Quantidade = 2 };
        var response = await _client.PutAsJsonAsync($"/api/produtos/{created!.Id}", updateRequest);
        response.EnsureSuccessStatusCode();

        var produto = await response.Content.ReadFromJsonAsync<ProdutoResponse>();
        Assert.Equal("Atualizado", produto!.Nome);
        Assert.Equal(20m, produto.Preco);
    }

    [Fact]
    public async Task Delete_ExistingProduct_ReturnsNoContent()
    {
        var createRequest = new ProdutoRequest { Nome = "Para Deletar", Preco = 5m, Quantidade = 1 };
        var createResponse = await _client.PostAsJsonAsync("/api/produtos", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<ProdutoResponse>();

        var response = await _client.DeleteAsync($"/api/produtos/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _client.GetAsync($"/api/produtos/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
