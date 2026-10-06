using System;
namespace TemperatureApp
{
    // Класс-издатель: создаёт событие при изменении температуры
    class TemperatureSensor
    {
        // Делегат задаёт тип методов, которые могут обрабатывать событие
        public delegate void TemperatureChangedHandler(int newTemperature);
        // Событие сообщает термостату об изменении температуры
        public event TemperatureChangedHandler TemperatureChanged;
        private int _currentTemperature;
        private Random _random = new Random();
        // Возвращает текущую температуру
        public int CurrentTemperature => _currentTemperature;
        // Метод имитирует измерение температуры
        public void MeasureTemperature()
        {
            // Генерируем случайную температуру от 15 до 30 градусов
            int newTemperature = _random.Next(15, 31);
            Console.WriteLine($"Датчик: {newTemperature}°C");
            // Если температура изменилась, вызываем событие
            if (newTemperature != _currentTemperature)
            {
                _currentTemperature = newTemperature;
                // Уведомляем всех подписчиков события
                TemperatureChanged?.Invoke(newTemperature);
            }
        }
    }
    // Класс-подписчик: получает данные от датчика
    class Thermostat
    {
        private int _comfortThreshold;
        // Конструктор задаёт комфортную температуру
        public Thermostat(int comfortThreshold)
        {
            _comfortThreshold = comfortThreshold;
        }
        // Обработчик события изменения температуры
        public void OnTemperatureChanged(int temperature)
        {
            // Определяем состояние отопления
            if (temperature < _comfortThreshold)
            {
                Console.WriteLine($"Термостат: {temperature}°C — отопление ВКЛ.");
            }
            else
            {
                Console.WriteLine($"Термостат: {temperature}°C — отопление ВЫКЛ.");
            }
        }
    }
    // Главный класс программы
    class Program
    {
        static void Main(string[] args)
        {
            // Создаём датчик температуры
            TemperatureSensor sensor = new TemperatureSensor();
            // Создаём термостат с комфортной температурой 22°C
            Thermostat thermostat = new Thermostat(22);
            // Подписываем термостат на событие датчика
            sensor.TemperatureChanged += thermostat.OnTemperatureChanged;
            Console.WriteLine("Система запущена:\n");
            // Выполняем 5 измерений температуры
            for (int i = 0; i < 5; i++)
            {
                sensor.MeasureTemperature();
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}