using System;
using System.Collections.Generic;
// Класс для работы с массивом строк
class StringArray
{
    // Массив строк и максимальная длина одной строки
    private string[] items;
    private int stringLength;
    // Конструктор массива
    public StringArray(int size, int stringLength)
    {
        items = new string[size];
        this.stringLength = stringLength;
    }
    // Установка элемента по индексу
    public void Set(int index, string value)
    {
        // Проверяем, находится ли индекс внутри границ массива
        if (index < 0 || index >= items.Length)
        {
            Console.WriteLine("Ошибка: индекс находится за пределами массива.");
            return;
        }
        // Проверяем, не превышает ли строка допустимую длину
        if (value.Length > stringLength)
        {
            Console.WriteLine(
                "Ошибка: строка слишком длинная. Максимум символов: "
                + stringLength
            );
            return;
        }
        // Если все проверки пройдены, сохраняем строку в массив
        items[index] = value;
    }
    // Получение элемента по индексу
    public string Get(int index)
    {
        // Проверяем выход индекса за границы массива
        if (index < 0 || index >= items.Length)
        {
            Console.WriteLine("Ошибка: индекс находится за пределами массива.");
            return null;
        }
        return items[index];
    }
    // Вывод одного элемента массива
    public void PrintElement(int index)
    {
        // Получаем элемент через метод Get()
        string value = Get(index);

        // Если элемент существует, выводим его
        if (value != null)
        {
            Console.WriteLine(
                "Элемент с индексом " + index + ": " + value
            );
        }
    }
    // Вывод всех элементов массива
    public void Print()
    {
        for (int i = 0; i < items.Length; i++)
        {
            Console.WriteLine(
                "[" + i + "] " + items[i]
            );
        }
    }
    // Поэлементное сцепление двух массивов
    public static StringArray Concatenate(
        StringArray first,
        StringArray second)
    {
        // Берём размер меньшего массива, чтобы для каждого индекса существовал элемент в обоих массивах
        int newSize = Math.Min(
            first.items.Length,
            second.items.Length
        );
        // Создаём новый массив.
        // Максимальная длина строки равна сумме длин исходных строк
        StringArray result = new StringArray(
            newSize,
            first.stringLength + second.stringLength
        );
        // Объединяем элементы с одинаковыми индексами
        for (int i = 0; i < newSize; i++)
        {
            result.items[i] =
                first.items[i] + second.items[i];
        }
        return result;
    }
    // Объединение двух массивов без повторяющихся элементов
    public static StringArray MergeWithoutDuplicates(
        StringArray first,
        StringArray second)
    {
        // Временный список для хранения уникальных строк
        List<string> uniqueItems = new List<string>();
        // Добавляем элементы первого массива
        foreach (string item in first.items)
        {
            // Добавляем элемент только при отсутствии такого элемента в списке
            if (!uniqueItems.Contains(item))
            {
                uniqueItems.Add(item);
            }
        }
        // Добавляем элементы второго массива без повторений
        foreach (string item in second.items)
        {
            if (!uniqueItems.Contains(item))
            {
                uniqueItems.Add(item);
            }
        }
        // Создаём новый массив размером с количество уникальных элементов
        StringArray result = new StringArray(
            uniqueItems.Count,
            Math.Max(first.stringLength, second.stringLength)
        );
        // Копируем уникальные элементы из списка в новый массив
        for (int i = 0; i < uniqueItems.Count; i++)
        {
            result.items[i] = uniqueItems[i];
        }
        return result;
    }
}
class Program
{
    // Главный метод программы
    static void Main()
    {
        // Создаём первый массив: 4 элемента, максимальная длина строки — 10 символов
        StringArray array1 = new StringArray(4, 10);
        // Заполняем первый массив
        array1.Set(0, "Apple");
        array1.Set(1, "Book");
        array1.Set(2, "Cat");
        array1.Set(3, "Dog");
        // Создаём и заполняем второй массив
        StringArray array2 = new StringArray(4, 10);
        array2.Set(0, "Red");
        array2.Set(1, "Blue");
        array2.Set(2, "Cat");
        array2.Set(3, "Green");
        // Вывод первого массива
        Console.WriteLine("Первый массив:");
        array1.Print();
        // Вывод второго массива
        Console.WriteLine("\nВторой массив:");
        array2.Print();
        // Получение элемента по индексу
        Console.WriteLine("\nЭлемент по индексу:");
        array1.PrintElement(2);
        // Проверка обработки неправильного индекса
        Console.WriteLine("\nПроверка индекса:");
        array1.PrintElement(10);
        // Поэлементное объединение двух массивов
        StringArray concatenated =
            StringArray.Concatenate(array1, array2);
        Console.WriteLine("\nПоэлементное сцепление:");
        concatenated.Print();
        // Объединение двух массивов без повторяющихся элементов
        StringArray merged =
            StringArray.MergeWithoutDuplicates(array1, array2);
        Console.WriteLine("\nОбьединение без повторений:");
        merged.Print();
    }
}