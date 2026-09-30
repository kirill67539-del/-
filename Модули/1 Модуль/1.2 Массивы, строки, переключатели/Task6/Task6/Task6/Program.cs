using System;
class Task6
{
    static void Main()
    {
        double[] array = new double[10];
        Random random = new Random();
        // Заполнение массива случайными числами от -10 до 10
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = random.NextDouble() * 20 - 10;
        }
        Console.WriteLine("Исходный массив:");
        for (int i = 0; i < array.Length; i++)
        {
            Console.Write($"{array[i]:F2} ");
        }
        Console.WriteLine();
        // Создание массива индексов исходного массива
        int[] indexes = new int[10];
        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] = i;
        }
        // Сортирование индексов в соответствии со значениями исходного массива
        for (int i = 0; i < indexes.Length - 1; i++)
        {
            for (int j = 0; j < indexes.Length - i - 1; j++)
            {
                if (array[indexes[j]] > array[indexes[j + 1]])
                {
                    int temp = indexes[j];
                    indexes[j] = indexes[j + 1];
                    indexes[j + 1] = temp;
                }
            }
        }
        Console.WriteLine("Индексы в порядке возрастания значений:");
        for (int i = 0; i < indexes.Length; i++)
        {
            Console.Write(indexes[i] + " ");
        }
        Console.WriteLine();
        Console.WriteLine("Элементы в порядке возрастания:");
        // Вывод элементов исходного массива по отсортированным индексам
        for (int i = 0; i < indexes.Length; i++)
        {
            Console.Write($"{array[indexes[i]]:F2} ");
        }
    }
}