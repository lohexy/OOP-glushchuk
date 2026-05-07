using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Encodings.Web;

namespace lab28v4
{
    public class Actor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        
        public List<Actor> Actors { get; set; } = new List<Actor>();
    }

    [JsonSerializable(typeof(List<Movie>))]
    public partial class MovieContext : JsonSerializerContext
    {
    }

    public class MovieRepository
    {
        private List<Movie> _movies = new List<Movie>();

        public void Add(Movie movie)
        {
            _movies.Add(movie);
        }

        public IEnumerable<Movie> GetAll()
        {
            return _movies;
        }

        public Movie GetById(int id)
        {
            return _movies.FirstOrDefault(m => m.Id == id);
        }

        public async Task SaveToFileAsync(string filename)
        {
            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true,
                TypeInfoResolver = MovieContext.Default,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            
            using FileStream createStream = File.Create(filename);
            await JsonSerializer.SerializeAsync(createStream, _movies, options);
        }

        public async Task LoadFromFileAsync(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("Файл не знайдено!");
                return;
            }

            var options = new JsonSerializerOptions 
            { 
                TypeInfoResolver = MovieContext.Default
            };

            using FileStream openStream = File.OpenRead(filename);
            _movies = await JsonSerializer.DeserializeAsync<List<Movie>>(openStream, options) ?? new List<Movie>();
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string filename = "movies.json";
            
            var repository = new MovieRepository();

            Console.WriteLine("Створення об'єктів");
            
            var actor1 = new Actor { Id = 1, FirstName = "Кіану", LastName = "Рівз" };
            var actor2 = new Actor { Id = 2, FirstName = "Керрі-Енн", LastName = "Мосс" };
            var actor3 = new Actor { Id = 3, FirstName = "Меттью", LastName = "Макконехі" };

            var movie1 = new Movie 
            { 
                Id = 1, 
                Title = "Матриця", 
                Year = 1999, 
                Actors = new List<Actor> { actor1, actor2 } 
            };

            var movie2 = new Movie 
            { 
                Id = 2, 
                Title = "Інтерстеллар", 
                Year = 2014, 
                Actors = new List<Actor> { actor3 } 
            };

            repository.Add(movie1);
            repository.Add(movie2);

            Console.WriteLine($"Зберігаємо дані у файл {filename}...");
            await repository.SaveToFileAsync(filename);
            Console.WriteLine("Дані успішно збережено!\n");

            var newRepository = new MovieRepository();

            Console.WriteLine($"Завантажуємо дані з файлу {filename}...");
            await newRepository.LoadFromFileAsync(filename);

            Console.WriteLine("\nЗавантажені фільми");
            var loadedMovies = newRepository.GetAll();
            
            foreach (var movie in loadedMovies)
            {
                Console.WriteLine($"Фільм: {movie.Title} (Рік: {movie.Year}, ID: {movie.Id})");
                Console.WriteLine("Актори:");
                foreach (var actor in movie.Actors)
                {
                    Console.WriteLine($" - {actor.FirstName} {actor.LastName}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Пошук фільму за ID = 1");
            var foundMovie = newRepository.GetById(1);
            if (foundMovie != null)
            {
                Console.WriteLine($"Знайдено: {foundMovie.Title}");
            }
        }
    }
}