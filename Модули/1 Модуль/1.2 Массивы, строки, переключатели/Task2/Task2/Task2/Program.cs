using System;
class Task2
{
    static void Main()
    {
        // Создание исходного массива
        int[] array = { 12, 5, 27, 8, 19, 3, 45, 16, 7, 31 };
        Console.WriteLine("Исходный массив:");
        // Вывод исходного массива
        for (int i = 0; i < array.Length; i++)
        {
            Console.Write(array[i] + " ");
        }
        Console.WriteLine();
        // Находим индекс максимального элемента
        int maxIndex = 0;
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > array[maxIndex])
            {
                maxIndex = i;
            }
        }
        // Ввод числа для замены максимального элемента
        Console.Write("Введите целое число: ");
        int number = Convert.ToInt32(Console.ReadLine());
        // Заменяем максимальный элемент введённым числом
        array[maxIndex] = number;
        Console.WriteLine("Измененный массив:");
        // Вывод изменённого массива
        for (int i = 0; i < array.Length; i++)
        {
            Console.Write(array[i] + " ");
        }
    }
}