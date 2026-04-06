using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Application.DTOs;
using GithubAutomationApiTest.Application.Interfaces;
using GithubAutomationApiTest.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GithubAutomationApiTest.Tests.Unit.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductsController(_productServiceMock.Object);
        }

        [Fact]
        public async Task GetAsync_ValidParameters_ReturnsOkResult()
        {
            // Arrange
            var pagedResult = new PagedResultDto<ProductDto>
            {
                Items = new List<ProductDto>
                {
                    new ProductDto { Id = Guid.NewGuid(), Name = "Product A", Category = "Category1", Price = 10 }
                },
                Page = 1,
                Size = 10,
                TotalItems = 1,
                TotalPages = 1
            };

            _productServiceMock
                .Setup(service => service.GetProductsAsync(null, 1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetAsync(1, 10, null, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedValue = Assert.IsType<PagedResultDto<ProductDto>>(okResult.Value);
            Assert.Single(returnedValue.Items);
        }

        [Fact]
        public async Task GetAsync_InvalidParameters_ReturnsBadRequest()
        {
            // Arrange
            _productServiceMock
                .Setup(service => service.GetProductsAsync(null, 0, 10, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Page must be greater than or equal to 1."));

            // Act
            var result = await _controller.GetAsync(0, 10, null, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Parâmetros inválidos", ((dynamic)badRequestResult.Value).error);
        }
    }
}