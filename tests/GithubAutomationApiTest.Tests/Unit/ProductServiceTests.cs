using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Application.DTOs;
using GithubAutomationApiTest.Application.Services;
using GithubAutomationApiTest.Domain.Entities;
using GithubAutomationApiTest.Domain.Interfaces;
using Moq;
using Xunit;

namespace GithubAutomationApiTest.Tests.Unit
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetProductsAsync_ValidRequest_ReturnsFilteredAndPagedProducts()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetActiveProductsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>
                {
                    new Product { Id = Guid.NewGuid(), Name = "Product A", Category = "Category 1", Price = 10.0m, IsActive = true },
                    new Product { Id = Guid.NewGuid(), Name = "Product B", Category = "Category 2", Price = 20.0m, IsActive = true },
                    new Product { Id = Guid.NewGuid(), Name = "Product C", Category = "Category 1", Price = 15.0m, IsActive = true }
                });

            var service = new ProductService(mockRepository.Object);

            // Act
            var result = await service.GetProductsAsync(1, 2, "Category 1", CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal("Category 1", p.Category));
        }
    }
}