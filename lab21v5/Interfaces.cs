namespace lab21
{
    //контракт для доставки
    public interface IShippingStrategy
    {
        decimal CalculateCost(decimal distance, decimal weight);
    }

    //контракт для спортзалу
    public interface IPassStrategy
    {
        decimal CalculatePrice(int hours, bool hasExtraServices);
    }
}