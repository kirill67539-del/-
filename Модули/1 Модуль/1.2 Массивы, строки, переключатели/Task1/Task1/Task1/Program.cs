using System;
class Task1
{
    static void Main()
    {
        // Ввод размера массива
        Console.Write("Введите размер массива N: ");
        int n = Convert.ToInt32(Console.ReadLine());
        // Создание массива заданного размера
        double[] array = new double[n];
        Console.WriteLine("Введите элементы массива:");
        // Заполнение массива элементами, введёнными пользователем
        for (int i = 0; i < n; i++)
        {
            array[i] = Convert.ToDouble(Console.ReadLine());
        }
        // Нахождение максимального по модулю элемента
        double maxAbs = Math.Abs(array[0]);
        for (int i = 1; i < n; i++)
        {
            if (Math.Abs(array[i]) > maxAbs)
            {
                maxAbs = Math.Abs(array[i]);
            }
        }
        // Деление каждого элемента массива на максимальный по модулю элемент
        for (int i = 0; i < n; i++)
        {
            array[i] /= maxAbs;
        }
        // Вывод нормированного массива
        Console.WriteLine("Нормированный массив:");
        for (int i = 0; i < n; i++)
        {
            Console.Write(array[i] + " ");
        }
    }
}