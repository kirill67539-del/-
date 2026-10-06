using System;
// Класс Person описывает человека и хранит его данные
class Person
{
    // Приватные поля класса — данные доступны только внутри Person
    private string name;
    private int age;
    private string address;
    // Метод для установки имени человека
    public void SetName(string name)
    {
        this.name = name;
    }
    // Метод для получения имени человека
    public string GetName()
    {
        return name;
    }
    // Метод для установки возраста
    public void SetAge(int age)
    {
        this.age = age;
    }
    // Метод для получения возраста
    public int GetAge()
    {
        return age;
    }
    // Метод для установки адреса
    public void SetAddress(string address)
    {
        this.address = address;
    }
    // Метод для получения адреса
    public string GetAddress()
    {
        return address;
    }
}
class Program
{
    // Главный метод программы
    static void Main()
    {
        // Создание первого объекта класса Person
        Person person1 = new Person();
        // Заполнение данных первого человека
        person1.SetName("Иван");
        person1.SetAge(20);
        person1.SetAddress("Москва");
        // Создание второго объекта класса Person
        Person person2 = new Person();
        // Заполнение данных второго человека
        person2.SetName("Анна");
        person2.SetAge(22);
        person2.SetAddress("Санкт-Петербург");
        // Вывод информации о первом человеке
        Console.WriteLine("Человек 1:");
        Console.WriteLine("Имя: " + person1.GetName());
        Console.WriteLine("Возраст: " + person1.GetAge());
        Console.WriteLine("Адрес: " + person1.GetAddress());
        Console.WriteLine();
        // Вывод информации о втором человеке
        Console.WriteLine("Человек 2:");
        Console.WriteLine("Имя: " + person2.GetName());
        Console.WriteLine("Возраст: " + person2.GetAge());
        Console.WriteLine("Адрес: " + person2.GetAddress());
    }
}