using System;

namespace lab21
{
    //стратегії
    public class StandardShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight) => distance * 1.5m + weight * 0.5m;
    }

    public class ExpressShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight) => (distance * 2.5m + weight * 1.0m) + 50m;
    }

    public class InternationalShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight) => (distance * 5.0m + weight * 2.0m) * 1.15m;
    }

    public class NightShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight) => (distance * 1.5m + weight * 0.5m) + 100m;
    }

    //створення стратегій
    public static class ShippingStrategyFactory
    {
        public static IShippingStrategy CreateStrategy(string type) => type.ToLower() switch
        {
            "standard" => new StandardShippingStrategy(),
            "express" => new ExpressShippingStrategy(),
            "international" => new InternationalShippingStrategy(),
            "night" => new NightShippingStrategy(),
            _ => throw new ArgumentException("Unknown shipping type")
        };
    }

    //сервіс доставки
    public class DeliveryService
    {
        public decimal CalculateDeliveryCost(decimal distance, decimal weight, IShippingStrategy strategy)
        {
            return strategy.CalculateCost(distance, weight);
        }
    }
}