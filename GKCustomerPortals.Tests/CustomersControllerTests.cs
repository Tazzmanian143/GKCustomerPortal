using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using GKCustomerPortal.Controllers;
using GKCustomerPortal.Model;
using GKCustomerPortal.Services;

namespace GKCustomerPortals.Tests;

public class CustomersControllerTests
{
    private readonly Mock<ICustomerService> _mockService;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _mockService = new Mock<ICustomerService>();
        _controller = new CustomersController(_mockService.Object);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenCustomerExists()
    {
        // Arrange
        var mockCustomer = new CustomerModel { Id = 1, FirstName = "John", Email = "john@test.com" };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(mockCustomer);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomer = Assert.IsType<CustomerModel>(okResult.Value);
        Assert.Equal(1, returnedCustomer.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((CustomerModel)null);

        // Act
        var result = await _controller.Get(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedResponse_WhenModelIsValid()
    {
        // Arrange
        var newCustomer = new CustomerModel { FirstName = "Jane", Email = "jane@test.com" };
        var createdCustomer = new CustomerModel { Id = 2, FirstName = "Jane", Email = "jane@test.com" };
        _mockService.Setup(s => s.CreateAsync(newCustomer)).ReturnsAsync(createdCustomer);

        // Act
        var result = await _controller.Create(newCustomer);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.Get), createdResult.ActionName);
        var returnedCustomer = Assert.IsType<CustomerModel>(createdResult.Value);
        Assert.Equal(2, returnedCustomer.Id);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
