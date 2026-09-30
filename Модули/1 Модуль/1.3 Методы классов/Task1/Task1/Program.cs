using System;
class Program
{
    // Вычисление НОД двух натуральных чисел
    static int GCD(int a, int b)
    {
        // Использование алгоритма Евклида
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
    static void Main()
    {
        // Ввод числителя и знаменателя
        Console.Write("Введите числитель: ");
        int numerator = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите знаменатель: ");
        int denominator = Convert.ToInt32(Console.ReadLine());
        // Нахождение НОД числителя и знаменателя
        int gcd = GCD(numerator, denominator);
        // Сокращение дроби, разделив числитель и знаменатель на НОД
        numerator /= gcd;
        denominator /= gcd;
        // Вывод сокращённой дроби
        Console.WriteLine("Сокращенная дробь: " + numerator + "/" + denominator);
    }
}