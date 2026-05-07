using System;

class City
{
    private string name;
    private string country;

    public int Population { get; set; }

    public City(string name, string country, int population)
    {
        this.name = name;
        this.country = country;
        this.Population = population;
        Console.WriteLine($"Створено місто: {name}, {country}");
    }

    public string GetInfo()
    {
        return $"Місто: {name}, Країна: {country}, Населення: {Population}";
    }

    ~City()
    {
        Console.WriteLine($"Об'єкт {name} знищено");
    }
}

class Program
{
    static void Main(string[] args)
    {
        City city1 = new City("Київ", "Україна", 2800000);
        City city2 = new City("Лондон", "Велика Британія", 8900000);
        City city3 = new City("Токіо", "Японія", 14000000);


        Console.WriteLine(city1.GetInfo());
        Console.WriteLine(city2.GetInfo());
        Console.WriteLine(city3.GetInfo());

        Console.WriteLine("Програма завершена");
    }
}
