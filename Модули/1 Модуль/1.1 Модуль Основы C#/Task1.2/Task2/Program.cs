using System;
class Task2
{
    static void Main()
    {
        Console.Write("Введите первое число: ");
        double number1 = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите второе число: ");
        double number2 = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите третье число: ");
        double number3 = Convert.ToDouble(Console.ReadLine());
        // Вычисление среднего арифметического трёх чисел
        double average = (number1 + number2 + number3) / 3;
        // Вывод результата
        Console.WriteLine("Среднее арифметическое: " + average);
    }
}
