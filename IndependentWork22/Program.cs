using System;
using System.Collections.Generic;
using System.Linq;

public interface IComponent
{
    decimal GetPrice();
}

public class SingleProduct : IComponent
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public SingleProduct(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public decimal GetPrice() => Price;
}

public class ProductBundle : IComponent
{
    public string Name { get; set; }
    private readonly List<IComponent> _components = new List<IComponent>();

    public ProductBundle(string name)
    {
        Name = name;
    }

    public void Add(IComponent component) => _components.Add(component);
    public void Remove(IComponent component) => _components.Remove(component);

    public decimal GetPrice() => _components.Sum(c => c.GetPrice());
}

public abstract class Decorator : IComponent
{
    protected IComponent Component;

    public Decorator(IComponent component)
    {
        Component = component;
    }

    public virtual decimal GetPrice() => Component.GetPrice();
}

public class DiscountDecorator : Decorator
{
    private decimal _discountPercent;

    public DiscountDecorator(IComponent component, decimal discountPercent) : base(component)
    {
        _discountPercent = discountPercent;
    }

    public override decimal GetPrice()
    {
        decimal price = base.GetPrice();
        return price - (price * (_discountPercent / 100m));
    }
}

public class TaxDecorator : Decorator
{
    private decimal _taxPercent;

    public TaxDecorator(IComponent component, decimal taxPercent) : base(component)
    {
        _taxPercent = taxPercent;
    }

    public override decimal GetPrice()
    {
        decimal price = base.GetPrice();
        return price + (price * (_taxPercent / 100m));
    }
}

class Program22
{
    static void Main()
    {
        var phone = new SingleProduct("Smartphone", 1000m);
        var caseCover = new SingleProduct("Case", 50m);
        var charger = new SingleProduct("Charger", 30m);

        var accessoryBundle = new ProductBundle("Accessories");
        accessoryBundle.Add(caseCover);
        accessoryBundle.Add(charger);

        var mainBundle = new ProductBundle("Full Package");
        mainBundle.Add(phone);
        mainBundle.Add(accessoryBundle);

        Console.WriteLine($"Base Price: {mainBundle.GetPrice()}");

        var discountedBundle = new DiscountDecorator(mainBundle, 10m);
        Console.WriteLine($"Price with 10% Discount: {discountedBundle.GetPrice()}");

        var taxedBundle = new TaxDecorator(discountedBundle, 20m);
        Console.WriteLine($"Final Price with 20% Tax: {taxedBundle.GetPrice()}");
    }
}