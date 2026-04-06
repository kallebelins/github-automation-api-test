using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Application.DTOs;
using GithubAutomationApiTest.Application.Interfaces;
using GithubAutomationApiTest.Application.Services;
using GithubAutomationApiTest.Domain.Entities;
using Moq;
using Xunit;

namespace GithubAutomationApiTest.Tests.Unit.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetProductsAsync_ValidParameters_ReturnsPagedResult()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product A", Category = "Category1", Price = 10, IsActive = true },
                new Product { Id = Guid.NewGuid(), Name = "Product B", Category = "Category1", Price = 20, IsActive = true }
            };

            _productRepositoryMock
                .Setup(repo => repo.GetActiveProductsAsync(null, 1, 2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((products, products.Count));

            // Act
            var result = await _productService.GetProductsAsync(null, 1, 2, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.Page);
            Assert.Equal(2, result.Size);
            Assert.Equal(2, result.TotalItems);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public async Task GetProductsAsync_PageLessThanOne_ThrowsArgumentException()
        {
            // Arrange
            var page = 0;
            var size = 10;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _productService.GetProductsAsync(null, page, size, CancellationToken.None));
        }

        [Fact]
        public async Task GetProductsAsync_SizeOutOfRange_ThrowsArgumentException()
        {
            // Arrange
            var page = 1;
            var size = 101;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _productService.GetProductsAsync(null, page, size, CancellationToken.None));
        }

        [Fact]
        public async Task GetProductsAsync_WithCategory_FiltersByCategory()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product A", Category = "Category1", Price = 10, IsActive = true }
            };

            _productRepositoryMock
                .Setup(repo => repo.GetActiveProductsAsync("Category1", 1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync((products, products.Count));

            // Act
            var result = await _productService.GetProductsAsync("Category1", 1, 10, CancellationToken.None);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal("Category1", result.Items.First().Category);
        }
    }
}