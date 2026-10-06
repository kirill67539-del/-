using System;
using System.Collections.Generic;
// Класс Book описывает книгу
class Book
{
    // Свойства книги
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    // Конструктор для создания книги
    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }
    // Метод для вывода информации о книге
    public void PrintInfo()
    {
        Console.WriteLine(
            "Название: " + Title +
            ", Автор: " + Author +
            ", Год: " + Year
        );
    }
}
// Класс домашней библиотеки
class HomeLibrary
{
    // Список книг позволяет хранить произвольное количество объектов Book
    private List<Book> books = new List<Book>();
    // Метод добавления книги в библиотеку
    public void AddBook(Book book)
    {
        books.Add(book);
        Console.WriteLine("Книга добавлена: " + book.Title);
    }
    // Метод удаления книги по названию
    public void RemoveBook(string title)
    {
        // Find ищет первую книгу, которая подходит под условие
        Book book = books.Find(b => b.Title == title);
        // Если книга найдена — удаляем её из списка
        if (book != null)
        {
            books.Remove(book);
            Console.WriteLine("Книга удалена: " + title);
        }
        else
        {
            Console.WriteLine("Книга не найдена.");
        }
    }
    // Поиск книг по имени автора
    public void FindByAuthor(string author)
    {
        Console.WriteLine("\nКниги автора " + author + ":");
        // Перебираем все книги в списке
        foreach (Book book in books)
        {
            // Проверяем, совпадает ли автор книги с заданным
            if (book.Author == author)
            {
                book.PrintInfo();
            }
        }
    }
    // Поиск книг по году выпуска
    public void FindByYear(int year)
    {
        Console.WriteLine("\nКниги " + year + " года:");
        // Перебираем все книги и проверяем их год
        foreach (Book book in books)
        {
            if (book.Year == year)
            {
                book.PrintInfo();
            }
        }
    }
    // Сортировка книг по названию
    public void SortByTitle()
    {
        // Sort сравнивает названия двух книг, CompareTo определяет порядок строк
        books.Sort((book1, book2) =>
            book1.Title.CompareTo(book2.Title));
    }
    // Вывод всех книг библиотеки
    public void PrintAllBooks()
    {
        foreach (Book book in books)
        {
            book.PrintInfo();
        }
    }
}
class Program
{
    // Главный метод программы
    static void Main()
    {
        // Создание объекта домашней библиотеки
        HomeLibrary library = new HomeLibrary();
        // Создаем книги заранее
        Book book1 = new Book(
            "Война и мир",
            "Лев Толстой",
            1869
        );
        Book book2 = new Book(
            "Преступление и наказание",
            "Фёдор Достоевский",
            1866
        );
        Book book3 = new Book(
            "Анна Каренина",
            "Лев Толстой",
            1877
        );
        Book book4 = new Book(
            "Идиот",
            "Фёдор Достоевский",
            1869
        );
        // Добавляем созданные книги в список библиотеки
        library.AddBook(book1);
        library.AddBook(book2);
        library.AddBook(book3);
        library.AddBook(book4);
        // Вывод всех книг
        Console.WriteLine("\nВсе книги:");
        library.PrintAllBooks();
        // Поиск книг определённого автора
        library.FindByAuthor("Лев Толстой");
        // Поиск книг определённого года
        library.FindByYear(1869);
        // Сортировка всех книг по названию
        Console.WriteLine("\nСортировка по названию:");
        library.SortByTitle();
        library.PrintAllBooks();
        // Удаление книги по названию
        Console.WriteLine("\nУдаление книги:");
        library.RemoveBook("Идиот");
        // Вывод библиотеки после удаления
        Console.WriteLine("\nБиблиотека после удаления");
        library.PrintAllBooks();
    }
}