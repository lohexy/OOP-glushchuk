/// Виняток що виникає при спробі додати рейтинг поза  діапазоном [0; 10]
public class InvalidRatingException : Exception
{
    public InvalidRatingException(string message) : base(message) { }
}
///Виняток що виникає, коли сутність не знайдено 
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}