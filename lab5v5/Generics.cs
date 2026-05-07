//Узагальнений репозиторій 
public interface IRepository<T>
{
    void Add(T item);
    T Find(Func<T, bool> predicate);
    IEnumerable<T> GetAll();
    IEnumerable<T> Where(Func<T, bool> predicate);
    void Remove(T item);
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public IEnumerable<T> GetAll()
    {
        return _items;
    }

    public IEnumerable<T> Where(Func<T, bool> predicate)
    {
        return _items.Where(predicate);
    }

    public T Find(Func<T, bool> predicate)
    {
        var item = _items.FirstOrDefault(predicate);
        if (item == null)
        {
            throw new NotFoundException("Об'єкт не знайдено за заданим критерієм.");
        }
        return item;
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }
}

//Метод Max<T>(...) (MaxBy) 
public static class CollectionExtensions
{

    public static T MaxBy<T>(this IEnumerable<T> items, Func<T, double> selector)
    {
        if (!items.Any())
        {
            throw new InvalidOperationException("Послідовність не містить елементів.");
        }

        T maxItem = items.First();
        double maxValue = selector(maxItem);

        foreach (var item in items.Skip(1))
        {
            double currentValue = selector(item);
            if (currentValue > maxValue)
            {
                maxValue = currentValue;
                maxItem = item;
            }
        }
        return maxItem;
    }
}