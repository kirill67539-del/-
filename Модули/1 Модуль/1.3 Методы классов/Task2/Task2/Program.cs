using System;
class Program
{
    static void Main()
    {
        // Ввод заданной суммы
        Console.Write("Введите заданное число: ");
        int s = Convert.ToInt32(Console.ReadLine());
        Random random = new Random();
        int sum = 0;
        int count = 0;
        // Создание массива максимального возможного размера
        int[] array = new int[s];
        // Заполнение массива случайными числами, пока сумма не достигнет S
        while (sum < s)
        {
            int number = random.Next(1, 10);
            // Проверка, на превышение добавленным числом заданной суммы
            if (sum + number <= s)
            {
                array[count] = number;
                sum += number;
                count++;
            }
        }
        // Вывод элементов заполненной части массива
        Console.WriteLine("Элементы массива:");
        for (int i = 0; i < count; i++)
        {
            Console.Write(array[i] + " ");
        }
        Console.WriteLine();
        Console.WriteLine("Количество элементов: " + count);
        Console.WriteLine("Сумма элементов: " + sum);
    }
}