using System;
using System.Collections.Generic;
// Класс одного элемента данных
class DataItem
{
    public string Text { get; set; }
    public DateTime Date { get; set; }
    // Создание элемента с текстом и датой
    public DataItem(string text, DateTime date)
    {
        Text = text;
        Date = date;
    }
}
class Program
{
    // Делегат принимает элемент данных и возвращает true,
    // если элемент подходит под выбранный фильтр
    delegate bool DataFilter(DataItem item);
    // Метод выводит все элементы списка
    static void ShowData(List<DataItem> data)
    {
        Console.WriteLine("\nВсе данные:");
        foreach (DataItem item in data)
            Console.WriteLine($"{item.Date:dd.MM.yyyy} - {item.Text}");
    }
    // Метод применяет переданный фильтр ко всем данным
    static void ApplyFilter(List<DataItem> data, DataFilter filter)
    {
        Console.WriteLine("\nРезультат фильтрации:");
        bool found = false;
        foreach (DataItem item in data)
        {
            // Проверяем, подходит ли текущий элемент под фильтр
            if (filter(item))
            {
                Console.WriteLine($"{item.Date:dd.MM.yyyy} - {item.Text}");
                found = true;
            }
        }
        if (!found)
            Console.WriteLine("Подходящих данных не найдено.");
    }
    static void Main()
    {
        // Создаём список исходных данных
        List<DataItem> data = new List<DataItem>
        {
            new DataItem("Изучение C#", new DateTime(2026, 10, 1)),
            new DataItem("Создание проекта", new DateTime(2026, 10, 3)),
            new DataItem("Работа с делегатами", new DateTime(2026, 10, 5)),
            new DataItem("Изучение событий", new DateTime(2026, 10, 7)),
            new DataItem("Практическая работа", new DateTime(2026, 10, 10))
        };
        // Цикл работает до тех пор, пока пользователь не выберет выход
        while (true)
        {
            Console.WriteLine("\n   Фильтрация данных");
            Console.WriteLine("1 - Показать все данные");
            Console.WriteLine("2 - Фильтр по ключевому слову");
            Console.WriteLine("3 - Фильтр по дате");
            Console.WriteLine("0 - Выход");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();
            // Завершение программы
            if (choice == "0")
                break;
            // Вывод всех данных без фильтра
            if (choice == "1")
            {
                ShowData(data);
            }
            // Фильтрация по ключевому слову
            else if (choice == "2")
            {
                Console.Write("Введите ключевое слово: ");
                string keyword = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    Console.WriteLine("Ключевое слово не может быть пустым.");
                    continue;
                }
                // Создаём фильтр по ключевому слову
                DataFilter filter = item =>
                    item.Text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                // Передаём созданный фильтр в метод
                ApplyFilter(data, filter);
            }
            // Фильтрация по дате
            else if (choice == "3")
            {
                Console.Write("Введите дату (дд.мм.гггг): ");
                // TryParse проверяет правильность введённой даты
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
                {
                    Console.WriteLine("Некорректная дата.");
                    continue;
                }
                // Создаем фильтр по выбранной дате
                DataFilter filter = item => item.Date.Date == date.Date;
                // Применяем фильтр к списку данных
                ApplyFilter(data, filter);
            }
            else
            {
                Console.WriteLine("Неверный пункт меню.");
            }
            // После действия цикл автоматически возвращается к меню
        }
        Console.WriteLine("Программа завершена.");
    }
}