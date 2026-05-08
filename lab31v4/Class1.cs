namespace lab31v4;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public interface IProductRepository
{
    Product? GetById(int id);
    bool Update(Product product);
}

public interface IStockService
{
    bool IsInStock(int productId, int quantity);
    void ReduceStock(int productId, int quantity);
}

public class ProductService
{
    private readonly IProductRepository _repository;
    private readonly IStockService _stockService;

    public ProductService(IProductRepository repository, IStockService stockService)
    {
        _repository = repository;
        _stockService = stockService;
    }

    public bool SellProduct(int productId, int quantity)
    {
        if (quantity <= 0) return false;

        var product = _repository.GetById(productId);
        if (product == null) return false;

        if (!_stockService.IsInStock(productId, quantity))
            return false;

        _stockService.ReduceStock(productId, quantity);
        return _repository.Update(product);
    }
}