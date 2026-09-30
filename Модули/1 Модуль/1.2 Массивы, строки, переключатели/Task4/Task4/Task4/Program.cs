using System;
class Task4
{
    static void Main()
    {
        // Ввод размера массива и диапазона чисел
        Console.Write("Введите количество элементов K: ");
        int k = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите A: ");
        int a = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите B: ");
        int b = Convert.ToInt32(Console.ReadLine());
        int[] array = new int[k];
        Random random = new Random();
        // Заполнение массива случайными числами из заданного диапазона
        for (int i = 0; i < k; i++)
        {
            array[i] = random.Next(a, b);
        }
        Console.WriteLine("Исходный массив:");
        for (int i = 0; i < k; i++)
        {
            Console.Write(array[i] + " ");
        }
        Console.WriteLine();
        // Поиск индексов минимального и максимального элементов
        int minIndex = 0;
        int maxIndex = 0;
        for (int i = 1; i < k; i++)
        {
            if (array[i] < array[minIndex])
            {
                minIndex = i;
            }
            if (array[i] > array[maxIndex])
            {
                maxIndex = i;
            }
        }
        Console.WriteLine("Индекс минимального элемента: " + minIndex);
        Console.WriteLine("Индекс максимального элемента: " + maxIndex);
        // Определение границ диапазона между минимальным и максимальным элементами
        int start = Math.Min(minIndex, maxIndex);
        int end = Math.Max(minIndex, maxIndex);
        Console.WriteLine("Элементы между минимальным и максимальным:");
        // Вывод элементов от меньшего индекса до большего
        for (int i = start; i <= end; i++)
        {
            Console.Write(array[i] + " ");
        }
    }
}