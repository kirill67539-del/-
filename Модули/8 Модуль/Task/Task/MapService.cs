// Подключение библиотеки для работы со списками.
using System.Collections.Generic;

// Подключение библиотеки для чтения и записи файлов.
using System.IO;

// Подключение библиотеки для преобразования объектов в JSON и обратно.
using System.Text.Json;

namespace InteractiveMap
{
    // Класс для управления данными интерактивной карты.
    // Отвечает за добавление, удаление, сохранение и загрузку объектов.
    public class MapService
    {
        // Список всех объектов, размещённых на карте.
        // private set означает, что список можно получить извне,
        // но заменить его напрямую можно только внутри этого класса.
        public List<MapLocation> Locations { get; private set; } = new();

        // Добавляет новый объект в список карты.
        public void Add(MapLocation location)
        {
            Locations.Add(location);
        }

        // Удаляет указанный объект из списка карты.
        public void Remove(MapLocation location)
        {
            Locations.Remove(location);
        }

        // Сохраняет все объекты карты в JSON-файл.
        // Параметр path содержит путь к файлу для сохранения.
        public void Save(string path)
        {
            // Настраиваем форматирование JSON,
            // чтобы файл был удобен для чтения человеком.
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // Преобразуем список объектов в строку JSON.
            string json = JsonSerializer.Serialize(Locations, options);

            // Записываем полученную строку в файл.
            File.WriteAllText(path, json);
        }

        // Загружает объекты карты из JSON-файла.
        // Параметр path содержит путь к файлу для загрузки.
        public void Load(string path)
        {
            // Читаем содержимое файла в виде строки.
            string json = File.ReadAllText(path);

            // Преобразуем JSON обратно в список объектов MapLocation.
            // Если данные отсутствуют, создаём пустой список.
            Locations = JsonSerializer.Deserialize<List<MapLocation>>(json)
                        ?? new List<MapLocation>();
        }
    }
}