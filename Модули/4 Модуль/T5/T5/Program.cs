using System;
interface IRisunok
{
    void DrawLine();
    void DrawCircle();
    void DrawRectangle();
}
// Класс Canvas реализует интерфейс IRisunok
class Canvas : IRisunok
{
    // Метод выводит сообщение о рисовании линии
    public void DrawLine()
    {
        Console.WriteLine("Отрисована линия (---)");
    }
    // Метод выводит сообщение о рисовании круга
    public void DrawCircle()
    {
        Console.WriteLine("Отрисован круг (O)");
    }
    // Метод выводит сообщение о рисовании прямоугольника
    public void DrawRectangle()
    {
        Console.WriteLine("Отрисован прямоугольник ([])");
    }
}
class Program
{
    static void Main()
    {
        // Создаём объект холста, через который будем рисовать фигуры
        Canvas canvas = new Canvas();
        // Цикл позволяет рисовать несколько фигур подряд, пока пользователь не выберет закончить
        while (true)
        {
            Console.WriteLine("\nРисование на холсте");
            Console.WriteLine("1 — Нарисовать линию");
            Console.WriteLine("2 — Нарисовать круг");
            Console.WriteLine("3 — Нарисовать прямоугольник");
            Console.WriteLine("0 — Выход");
            Console.Write("Ваш выбор: ");
            // Считываем выбор пользователя с клавиатуры
            string choice = Console.ReadLine();
            // В зависимости от введённого значения вызываем нужный метод
            switch (choice)
            {
                case "1":
                    canvas.DrawLine();
                    break; // Завершает текущий case
                case "2":
                    canvas.DrawCircle();
                    break;
                case "3":
                    canvas.DrawRectangle();
                    break;
                case "0":
                    // return завершает выполнение метода Main и программы
                    Console.WriteLine("Программа завершена.");
                    return;
                default:
                    // Выполняется, если введённого пункта нет в меню
                    Console.WriteLine("Такого пункта меню нет.");
                    break;
            }
        }
    }
}