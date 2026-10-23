using System;
using System.Text.Json.Serialization;

namespace InteractiveMap
{
    // Категории объектов карты.
    public enum LocationCategory
    {
        Landmark,
        Shop,
        Cafe,
        Park,
        Other
    }
    // Модель объекта на карте.
    public class MapLocation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public LocationCategory Category { get; set; }
        public string Description { get; set; } = "";
        // Координаты объекта на карте.
        public double X { get; set; }
        public double Y { get; set; }
        // Название категории на русском языке.
        [JsonIgnore]
        public string CategoryName => Category switch
        {
            LocationCategory.Landmark => "Достопримечательность",
            LocationCategory.Shop => "Магазин",
            LocationCategory.Cafe => "Кафе",
            LocationCategory.Park => "Парк",
            _ => "Другое"
        };
        // Координаты в удобном для отображения формате.
        [JsonIgnore]
        public string CoordinatesText => $"X: {X:0.00}; Y: {Y:0.00}";
    }
}