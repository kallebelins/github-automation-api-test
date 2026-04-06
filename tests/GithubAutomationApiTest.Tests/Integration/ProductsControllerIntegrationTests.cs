using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GithubAutomationApiTest.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GithubAutomationApiTest.Tests.Integration
{
    public class ProductsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProductsAsync_ValidRequest_ReturnsOkWithProducts()
        {
            // Arrange
            var url = "/api/v1/products?page=1&size=2&category=Category 1";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<IEnumerable<ProductDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(products);
            Assert.All(products, p => Assert.Equal("Category 1", p.Category));
        }
    }
}