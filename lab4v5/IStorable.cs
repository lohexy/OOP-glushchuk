namespace lab4v5
{
    public interface IStorable
    {
        string Title { get; }
        int Pages { get; }

        void ShowInfo();
    }
}

