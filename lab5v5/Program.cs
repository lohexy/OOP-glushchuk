using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Лабораторна робота 5: Рейтинги фільмів");

        
        IRepository<Movie> movieRepository = new Repository<Movie>();

        //Сутності для репозиторію
        var movie1 = new Movie("Interstellar"); 
        var movie2 = new Movie("Dune");
        movieRepository.Add(movie1);
        movieRepository.Add(movie2);

        //Демонстрація InvalidRatingException
        Console.WriteLine("\nДемонстрація обробки винятків");
        try
        {
            // Додаємо валідні рейтинги
            movie1.AddRating(new Rating("user1", 10));
            movie1.AddRating(new Rating("user2", 8));

            // Спроба додати невалідний рейтинг
            Console.WriteLine("[Test] Спроба додати рейтинг 11...");
            movie1.AddRating(new Rating("user3", 11)); 
        }
        catch (InvalidRatingException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Error] Помилка: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("\nДемонстрація обчислень (< 5)");
        Console.WriteLine($"Середній рейтинг '{movie1.Title}' ({movie1.GetRatingCount()} оцінки): {movie1.GetAverageRating():F2}");
              Console.WriteLine("\nДемонстрація обчислень (>= 5)");
        try
        {
            movie2.AddRating(new Rating("u1", 10)); //Max (буде відтіснено)
            movie2.AddRating(new Rating("u2", 9));
            movie2.AddRating(new Rating("u3", 8));
            movie2.AddRating(new Rating("u4", 7));
            movie2.AddRating(new Rating("u5", 1));  //Min (буде відтіснено)
        }
        catch (InvalidRatingException ex)
        {
            Console.WriteLine($"Неочікувана помилка: {ex.Message}");
        }
        
        Console.WriteLine($"Середній рейтинг '{movie2.Title}' ({movie2.GetRatingCount()} оцінки): {movie2.GetAverageRating():F2}");

        //Метод Max<T>(...)
        Console.WriteLine("\nДемонстрація Generics (MaxBy)");
        var allMovies = movieRepository.GetAll();
        var topRatedMovie = allMovies.MaxBy(m => m.GetAverageRating());
        
        Console.WriteLine($"Фільм з найвищим рейтингом: {topRatedMovie.Title} ({topRatedMovie.GetAverageRating():F2})");

        //Винятки - NotFoundException 
        Console.WriteLine("\nДемонстрація Generics (Repository + NotFoundException) ");
        try
        {
            Console.WriteLine("[Test] Пошук фільму 'Dune'...");
            var foundMovie = movieRepository.Find(m => m.Title == "Dune");
            Console.WriteLine($"Знайдено: {foundMovie.Title}");

            Console.WriteLine("\n[Test] Пошук фільму 'Titanic'...");
            var missingMovie = movieRepository.Find(m => m.Title == "Titanic"); 
        }
        catch (NotFoundException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Error] Помилка: {ex.Message}");
            Console.ResetColor();
        }
    }
}