///Рейтинг, залишений користувачами
public class Rating
{
    public string UserId { get; private set; }
    public int Value { get; private set; }

    public Rating(string userId, int value)
    {
        //Перевірка на діапазон
        if (value < 0 || value > 10)
        {
            throw new InvalidRatingException($"Рейтинг {value} є неприпустимим. Повинен бути в [0; 10].");
        }
        UserId = userId;
        Value = value;
    }
}

///Представляє фільм, який агрегує в собі список рейтингів
public class Movie
{
    public string Title { get; private set; }
    
    // Агрегація: Movie має список Rating, але не керує їх життєвим циклом
    private readonly List<Rating> _ratings = new();

    public Movie(string title)
    {
        Title = title;
    }

    ///Додає новий рейтинг до фільму
    public void AddRating(Rating rating)
    {
        if (rating.Value < 0 || rating.Value > 10)
        {
            throw new InvalidRatingException($"Неприпустимий рейтинг {rating.Value} для фільму '{Title}'.");
        }
        _ratings.Add(rating);
        Console.WriteLine($"[Info] Додано рейтинг {rating.Value} до фільму '{Title}'.");
    }

    ///Середній рейтинг
    public double GetAverageRating()
    {
        if (_ratings.Count == 0)
        {
            return 0.0;
        }

        var ratingValues = _ratings.Select(r => r.Value).ToList();

        //Якщо записів < 5 то звичайне середнє
        if (ratingValues.Count < 5)
        {
            return ratingValues.Average();
        }

        //Якщо записів >= 5, відтіскаємо 1 мін. та 1 макс.
        ratingValues.Sort();
        
        var trimmedRatings = ratingValues.Skip(1).Take(ratingValues.Count - 2);

        return trimmedRatings.Average();
    }

    ///Повертає загальну кількість оцінок фільму
    public int GetRatingCount()
    {
        return _ratings.Count;
    }
}