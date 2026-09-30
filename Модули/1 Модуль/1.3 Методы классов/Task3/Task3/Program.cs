using System;
class Program
{
    static void Main()
    {
        // Ввод размера квадратной матрицы
        Console.Write("Введите размер квадратной матрицы N: ");
        int n = Convert.ToInt32(Console.ReadLine());
        int[,] matrix = new int[n, n];
        Random random = new Random();
        // Заполнение матрицу случайными числами от -50 до 50
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matrix[i, j] = random.Next(-50, 51);
            }
        }
        // Вывод исходной матрицы
        Console.WriteLine("\nИсходная матрица:");
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write($"{matrix[i, j],5}");
            }
            Console.WriteLine();
        }
        // Сортирование строки матрицы по возрастанию суммы их элементов
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                int sum1 = 0;
                int sum2 = 0;
                // Вычисление суммы элементов текущей строки
                for (int k = 0; k < n; k++)
                {
                    sum1 += matrix[j, k];
                }
                // Вычисление суммы элементов следующей строки
                for (int k = 0; k < n; k++)
                {
                    sum2 += matrix[j + 1, k];
                }
                // Если текущая строка имеет большую сумму, то меняем две строки местами
                if (sum1 > sum2)
                {
                    for (int k = 0; k < n; k++)
                    {
                        int temp = matrix[j, k];
                        matrix[j, k] = matrix[j + 1, k];
                        matrix[j + 1, k] = temp;
                    }
                }
            }
        }
        // Вывод матрицы после сортировки
        Console.WriteLine("\nМатрица после сортировки строк:");
        for (int i = 0; i < n; i++)
        {
            int sum = 0;
            for (int j = 0; j < n; j++)
            {
                Console.Write($"{matrix[i, j],5}");
                sum += matrix[i, j];
            }
            Console.WriteLine($"   Сумма: {sum}");
        }
    }
}