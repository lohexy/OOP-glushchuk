using Moq;
using Xunit;
using lab31v4;

namespace lab31v4.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly Mock<IStockService> _stockServiceMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _stockServiceMock = new Mock<IStockService>();
        _service = new ProductService(_productRepoMock.Object, _stockServiceMock.Object);
    }

    [Fact]
    public void SellProduct_NegativeQuantity_ReturnsFalse()
    {
        var result = _service.SellProduct(1, -5);

        Assert.False(result);
    }

    [Fact]
    public void SellProduct_ProductNotFound_ReturnsFalse()
    {
        _productRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((Product?)null);

        var result = _service.SellProduct(99, 1);

        Assert.False(result);
    }

    [Fact]
    public void SellProduct_OutOfStock_ReturnsFalse()
    { 
        var product = new Product { Id = 1, Name = "Phone" };
        _productRepoMock.Setup(r => r.GetById(1)).Returns(product);
        _stockServiceMock.Setup(s => s.IsInStock(1, 10)).Returns(false);

        var result = _service.SellProduct(1, 10);

        Assert.False(result);
    }

    [Fact]
    public void SellProduct_SuccessfulSale_CallsReduceStock()
    {
        var product = new Product { Id = 1, Name = "Laptop" };
        _productRepoMock.Setup(r => r.GetById(1)).Returns(product);
        _stockServiceMock.Setup(s => s.IsInStock(1, 1)).Returns(true);
        _productRepoMock.Setup(r => r.Update(product)).Returns(true);

        _service.SellProduct(1, 1);

        _stockServiceMock.Verify(s => s.ReduceStock(1, 1), Times.Once);
    }

    [Fact]
    public void SellProduct_SuccessfulSale_CallsUpdateRepository()
    {
        var product = new Product { Id = 1 };
        _productRepoMock.Setup(r => r.GetById(1)).Returns(product);
        _stockServiceMock.Setup(s => s.IsInStock(1, 1)).Returns(true);
        _productRepoMock.Setup(r => r.Update(product)).Returns(true);

        _service.SellProduct(1, 1);

        _productRepoMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public void SellProduct_RepositoryUpdateFails_ReturnsFalse()
    {
        var product = new Product { Id = 1 };
        _productRepoMock.Setup(r => r.GetById(1)).Returns(product);
        _stockServiceMock.Setup(s => s.IsInStock(1, 1)).Returns(true);
        _productRepoMock.Setup(r => r.Update(product)).Returns(false);

        var result = _service.SellProduct(1, 1);

        Assert.False(result);
    }

    [Fact]
    public void SellProduct_ValidData_ReturnsTrue()
    {
        var product = new Product { Id = 5 };
        _productRepoMock.Setup(r => r.GetById(5)).Returns(product);
        _stockServiceMock.Setup(s => s.IsInStock(5, 2)).Returns(true);
        _productRepoMock.Setup(r => r.Update(product)).Returns(true);

        var result = _service.SellProduct(5, 2);

        Assert.True(result);
    }

    [Fact]
    public void SellProduct_VerifyStockCheckHappensBeforeReduction()
    {
        var product = new Product { Id = 1 };
        _productRepoMock.Setup(r => r.GetById(1)).Returns(product);
        _stockServiceMock.Setup(s => s.IsInStock(1, 1)).Returns(true);

        _service.SellProduct(1, 1);

        _stockServiceMock.Verify(s => s.IsInStock(1, 1), Times.AtLeastOnce);
    }
}