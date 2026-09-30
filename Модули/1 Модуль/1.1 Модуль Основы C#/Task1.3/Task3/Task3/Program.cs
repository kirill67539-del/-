using System;
class Task3
{
    static void Main()
    {
        Console.Write("Введите первую строку: ");
        string firstString = Console.ReadLine();
        Console.Write("Введите вторую строку: ");
        string secondString = Console.ReadLine();
        // Проверка на содержание второй строки внутри первой
        if (firstString.Contains(secondString))
        {
            Console.WriteLine("Вторая строка является подстрокой первой.");
        }
        else
        {
            Console.WriteLine("Вторая строка не является подстрокой первой.");
        }
    }
}