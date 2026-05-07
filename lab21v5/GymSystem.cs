using System;

namespace lab21
{
    //алгоритми для абонементів
    public class MorningPass : IPassStrategy
    {
        public decimal CalculatePrice(int hours, bool extra) => (hours * 50m) + (extra ? 30m : 0);
    }

    public class DayPass : IPassStrategy
    {
        public decimal CalculatePrice(int hours, bool extra) => (hours * 80m) + (extra ? 50m : 0);
    }

    public class FullPass : IPassStrategy
    {
        public decimal CalculatePrice(int hours, bool extra) => (hours * 120m) + (extra ? 100m : 0);
    }

    //вибір абонементів
    public static class GymPassFactory
    {
        public static IPassStrategy CreatePass(string type) => type.ToLower() switch
        {
            "morning" => new MorningPass(),
            "day" => new DayPass(),
            "full" => new FullPass(),
            _ => throw new ArgumentException("Unknown pass type")
        };
    }
}