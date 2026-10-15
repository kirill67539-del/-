using System;
using System.Collections.Generic;
// Интерфейс задаёт методы проверки доступности и выдачи книги
interface IBook
{
    bool IsAvailable();
    bool Issue();
}
class Book : IBook
{
    // Название и автор доступны для чтения, но изменить их извне нельзя
    public string Title { get; private set; }
    public string Author { get; private set; }
    // true — книга доступна, false — недоступна
    private bool available;
    // Конструктор задаёт название, автора и начальное состояние книги.
    // По умолчанию книга доступна, если не указано другое значение
    public Book(string title, string author, bool isAvailable = true)
    {
        Title = title;
        Author = author;
        available = isAvailable;
    }
    // Возвращаем текущее состояние книги
    public bool IsAvailable() => available;
    public bool Issue()
    {
        // Если книга уже выдана, повторно выдать её нельзя
        if (!available)
            return false;
        // Меняем состояние книги на «недоступна»
        available = false;
        return true;
    }
}
// Класс обычной книги наследует свойства и методы Book
class PaperBook : Book
{
    // base передаёт параметры конструктору родительского класса
    public PaperBook(string title, string author, bool isAvailable = true)
        : base(title, author, isAvailable) { }
}
// Класс справочной книги также наследует свойства и методы Book
class ReferenceBook : Book
{
    public ReferenceBook(string title, string author, bool isAvailable = true)
        : base(title, author, isAvailable) { }
}
class Program
{
    // Метод выводит список книг и их актуальные статусы
    static void ShowBooks(List<Book> books)
    {
        Console.WriteLine("\nКаталог библиотеки");
        // Перебираем все книги в списке
        for (int i = 0; i < books.Count; i++)
        {
            // Тернарный оператор выбирает статус в зависимости от доступности
            string status = books[i].IsAvailable()
                ? "Доступна"
                : "Не доступна";
            // i + 1 нужен, чтобы номера книг начинались с 1, а не с 0
            Console.WriteLine(
                $"{i + 1}. {books[i].Title} — {books[i].Author}. {status}");
        }
    }
    static void Main()
    {
        // Создаём список книг разных типов.
        List<Book> books = new List<Book>
        {
            new PaperBook("Война и мир", "Лев Толстой"),
            new PaperBook("Преступление и наказание", "Фёдор Достоевский"),
            new ReferenceBook("Энциклопедия", "Коллектив авторов"),
            new PaperBook("Мастер и Маргарита", "Михаил Булгаков", false)
        };
        // Цикл позволяет выдавать несколько книг без перезапуска программы
        while (true)
        {
            // При каждом повторении показываем каталог с обновлёнными статусами
            ShowBooks(books);
            Console.Write("\nВведите номер книги для выдачи: ");
            // Проверяем, что введено целое число в допустимом диапазоне
            if (!int.TryParse(Console.ReadLine(), out int number) ||
                number < 0 || number > books.Count)
            {
                Console.WriteLine("Неверный номер книги.");
                continue; // Возвращаемся к началу цикла
            }
            // Если пользователь ввёл 0, завершаем программу
            if (number == 0)
                break;
            // Индексы списка начинаются с 0, поэтому вычитаем 1 из номера книги
            Book selected = books[number - 1];
            // Issue() проверяет доступность и меняет состояние книги, если она ещё доступна
            if (selected.Issue())
                Console.WriteLine($"Книга «{selected.Title}» выдана.");
            else
                Console.WriteLine("Эта книга уже недоступна.");
            // При следующем повторении цикла каталог покажет новый статус
        }
        Console.WriteLine("Программа завершена.");
    }
}