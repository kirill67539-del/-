using System;
// Класс Author хранит информацию об авторе
class Author
{
    // Приватные поля класса Author
    private string name;
    private int birthYear;
    // Конструктор для создания объекта автора
    public Author(string name, int birthYear)
    {
        this.name = name;
        this.birthYear = birthYear;
    }
    // Метод для получения имени автора
    public string GetName()
    {
        return name;
    }
    // Метод для получения года рождения автора
    public int GetBirthYear()
    {
        return birthYear;
    }
}
// Класс Book хранит информацию о книге и её авторе
class Book
{
    // Приватные поля класса Book
    private string title;
    private int year;
    // Объект автора, связанный с книгой
    private Author author;
    // Конструктор для создания книги
    public Book(string title, int year, Author author)
    {
        this.title = title;
        this.year = year;
        this.author = author;
    }
    // Метод для вывода всей информации о книге
    public void PrintInfo()
    {
        Console.WriteLine("Название: " + title);
        Console.WriteLine("Год выпуска: " + year);
        // Получаем данные об авторе через объект Author
        Console.WriteLine("Автор: " + author.GetName());
        Console.WriteLine("Год рождения автора: " + author.GetBirthYear());
    }
}
// Основной класс программы
class Program
{
    // Главный метод программы
    static void Main()
    {
        // Создание объектов авторов
        Author author1 = new Author("Лев Толстой", 1828);
        Author author2 = new Author("Фёдор Достоевский", 1821);
        // Создание объектов книг и передача соответствующего автора
        // Один автор может быть указан у нескольких книг
        Book book1 = new Book("Война и мир", 1869, author1);
        Book book2 = new Book("Анна Каренина", 1877, author1);
        Book book3 = new Book("Преступление и наказание", 1866, author2);
        // Вывод информации о первой книге
        Console.WriteLine("Книга 1:");
        book1.PrintInfo();
        Console.WriteLine();
        // Вывод информации о второй книге
        Console.WriteLine("Книга 2:");
        book2.PrintInfo();
        Console.WriteLine();
        // Вывод информации о третьей книге
        Console.WriteLine("Книга 3:");
        book3.PrintInfo();
    }
}