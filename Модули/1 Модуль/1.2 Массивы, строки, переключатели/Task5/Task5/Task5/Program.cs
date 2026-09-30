using System;
class Task5
{
    static void Main()
    {
        // Ввод количества элементов массива
        Console.Write("Введите количество элементов K: ");
        int k = Convert.ToInt32(Console.ReadLine());
        // Русский алфавит и набор гласных букв
        string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        string vowels = "аеёиоуыэюя";
        char[] array = new char[k];
        Random random = new Random();
        // Заполнение первого массив случайными буквами
        for (int i = 0; i < k; i++)
        {
            array[i] = alphabet[random.Next(alphabet.Length)];
        }
        Console.WriteLine("Первый массив:");
        for (int i = 0; i < k; i++)
        {
            Console.Write(array[i] + " ");
        }
        Console.WriteLine();
        // Подсчет количества согласных букв в массиве
        int consonantCount = 0;
        for (int i = 0; i < k; i++)
        {
            if (!vowels.Contains(array[i]))
            {
                consonantCount++;
            }
        }
        // Создание нового массива для хранения согласных букв
        char[] consonants = new char[consonantCount];
        int index = 0;
        // Перенос согласных букв из первого массива во второй
        for (int i = 0; i < k; i++)
        {
            if (!vowels.Contains(array[i]))
            {
                consonants[index] = array[i];
                index++;
            }
        }
        Console.WriteLine("Массив согласных:");
        // Вывод массива согласных букв
        for (int i = 0; i < consonants.Length; i++)
        {
            Console.Write(consonants[i] + " ");
        }
    }
}