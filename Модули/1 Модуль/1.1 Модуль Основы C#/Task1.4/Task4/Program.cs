using System;
class Task4
{
    static void Main()
    {
        Random random = new Random();
        // Создание массива из 10 чисел
        int[] numbers = new int[10];
        int sum = 0;
        Console.WriteLine("Массив:");
        // Заполнение массива случайными числами и подсчет их суммы
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = random.Next(1, 101);
            Console.Write(numbers[i] + " ");
            sum += numbers[i];
        }
        Console.WriteLine();
        Console.WriteLine("Сумма всех элементов: " + sum);
    }
}
