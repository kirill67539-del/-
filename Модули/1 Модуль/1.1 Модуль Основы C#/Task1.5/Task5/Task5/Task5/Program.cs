using System;
class Task5
{
    static void Main()
    {
        // Перебор чисел от 1 до 100
        for (int i = 1; i <= 100; i++)
        {
            // Проверка деления числа одновременно на 3 и 5
            if (i % 3 == 0 && i % 5 == 0)
            {
                Console.WriteLine("FizzBuzz");
            }
            // Проверка деления числа только на 3
            else if (i % 3 == 0)
            {
                Console.WriteLine("Fizz");
            }
            // Проверка деления числа только на 5
            else if (i % 5 == 0)
            {
                Console.WriteLine("Buzz");
            }
            // Если число не делится ни на 3, ни на 5
            else
            {
                Console.WriteLine(i);
            }
        }
    }
}