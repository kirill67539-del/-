using System;
class Task3
{
    // Проверка на простое число
    static bool IsPrime(int number)
    {
        if (number < 2)
        {
            return false;
        }
        // Проверка на наличие делителей от 2 до корня квадратного из числа
        for (int i = 2; i * i <= number; i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }
        return true;
    }
    static void Main()
    {
        // Ввод количества простых чисел, которые нужно вывести
        Console.Write("Введите количество простых чисел K: ");
        int k = Convert.ToInt32(Console.ReadLine());
        int count = 0;
        int number = 2;
        Console.WriteLine("Простые числа:");
        // Поиск простых чисел, пока не будет найдено K чисел
        while (count < k)
        {
            if (IsPrime(number))
            {
                Console.Write(number + " ");
                count++;
                // После каждых 10 чисел переход на новую строку
                if (count % 10 == 0)
                {
                    Console.WriteLine();
                }
            }
            number++;
        }
    }
}
