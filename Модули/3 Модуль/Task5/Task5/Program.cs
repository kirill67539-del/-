using System;
using System.Collections.Generic;
class Program
{
    // Делегат для хранения метода сортировки
    delegate int[] SortMethod(int[] numbers);
    // Пузырьковая сортировка
    static int[] BubbleSort(int[] numbers)
    {
        // Копируем массив, чтобы сохранить исходные числа
        int[] result = (int[])numbers.Clone();
        for (int i = 0; i < result.Length - 1; i++)
        {
            for (int j = 0; j < result.Length - i - 1; j++)
            {
                // Меняем соседние элементы, если они стоят неправильно
                if (result[j] > result[j + 1])
                {
                    int temp = result[j];
                    result[j] = result[j + 1];
                    result[j + 1] = temp;
                }
            }
        }
        return result;
    }
    // Быстрая сортировка
    static int[] QuickSort(int[] numbers)
    {
        int[] result = (int[])numbers.Clone();
        QuickSortRecursive(result, 0, result.Length - 1);
        return result;
    }
    // Рекурсивная сортировка частей массива
    static void QuickSortRecursive(int[] numbers, int left, int right)
    {
        if (left >= right)
            return;
        // Выбираем опорный элемент из середины массива
        int pivot = numbers[(left + right) / 2];
        int i = left;
        int j = right;
        // Распределяем числа относительно опорного элемента
        while (i <= j)
        {
            while (numbers[i] < pivot) i++;
            while (numbers[j] > pivot) j--;
            if (i <= j)
            {
                int temp = numbers[i];
                numbers[i] = numbers[j];
                numbers[j] = temp;
                i++;
                j--;
            }
        }
        // Сортируем левую и правую части отдельно
        if (left < j)
            QuickSortRecursive(numbers, left, j);
        if (i < right)
            QuickSortRecursive(numbers, i, right);
    }
    static void Main()
    {
        Console.WriteLine("Выберите сортировку:");
        Console.WriteLine("1 - Пузырьковая сортировка");
        Console.WriteLine("2 - Быстрая сортировка");
        Console.Write("Выбор: ");
        string choice = Console.ReadLine();
        // Выбираем метод, который будет вызван через делегат
        SortMethod sort;
        if (choice == "1")
            sort = BubbleSort;
        else if (choice == "2")
            sort = QuickSort;
        else
        {
            Console.WriteLine("Неверный выбор.");
            return;
        }
        Console.Write("Введите числа через пробел: ");
        string input = Console.ReadLine();
        string[] parts = input.Split(
            new[] { ' ', ',', ';' },
            StringSplitOptions.RemoveEmptyEntries);
        List<int> list = new List<int>();
        // Проверяем и преобразуем введённые значения в числа
        foreach (string part in parts)
        {
            if (int.TryParse(part, out int number))
                list.Add(number);
            else
            {
                Console.WriteLine("Ошибка: введены некорректные данные.");
                return;
            }
        }
        if (list.Count == 0)
        {
            Console.WriteLine("Массив не должен быть пустым.");
            return;
        }
        int[] numbers = list.ToArray();
        // Запускаем выбранный алгоритм через делегат
        int[] result = sort(numbers);
        Console.WriteLine("Отсортированный массив:");
        Console.WriteLine(string.Join(" ", result));
    }
}