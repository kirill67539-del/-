using System;
class Task1
{
    static void Main()
    {
        Random random = new Random();
        // Загадываем случайное число от 1 до 100
        int secretNumber = random.Next(1, 101);
        int guess = 0;
        Console.WriteLine("Я загадал число от 1 до 100, угадайте его.");
        // Повторяем попытки, пока число не будет угадано
        while (guess != secretNumber)
        {
            Console.Write("Введите число: ");
            guess = Convert.ToInt32(Console.ReadLine());
            if (guess < secretNumber)
            {
                Console.WriteLine("Загаданное число больше.");
            }
            else if (guess > secretNumber)
            {
                Console.WriteLine("Загаданное число меньше.");
            }
            else
            {
                Console.WriteLine("Вы угадали число!");
            }
        }
    }
}