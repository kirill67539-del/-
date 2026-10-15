using System;
interface IFigure
{
    double Area();
    double Perimeter();
}
class Circle : IFigure
{
    private double r;
    public Circle(double radius)
    {
        r = radius;
    }
    // Площадь круга: π * r²
    public double Area() => Math.PI * r * r;
    // Длина окружности: 2 * π * r
    public double Perimeter() => 2 * Math.PI * r;
}
class Rectangle : IFigure
{
    private double a, b;
    public Rectangle(double width, double height)
    {
        a = width;
        b = height;
    }
    // Площадь прямоугольника: ширина * высота
    public double Area() => a * b;
    // Периметр прямоугольника: 2 * (ширина + высота)
    public double Perimeter() => 2 * (a + b);
}
class Triangle : IFigure
{
    private double a, b, c;
    public Triangle(double x, double y, double z)
    {
        a = x;
        b = y;
        c = z;
    }
    // Проверка неравенства треугольника, сумма любых двух сторон должна быть больше третьей
    public bool IsValid() =>
        a + b > c && a + c > b && b + c > a;
    public double Area()
    {
        // Формула Герона: сначала находим полупериметр
        double p = Perimeter() / 2;
        // Вычисляем площадь по длинам трёх сторон
        return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
    }
    public double Perimeter() => a + b + c;
}
class Program
{
    // Метод запрашивает число, пока пользователь не введет корректное положительное значение
    static double ReadPositive(string message)
    {
        double value;
        do
        {
            Console.Write(message);
            // TryParse проверяет, можно ли преобразовать ввод в число, а также что число >0
            if (double.TryParse(Console.ReadLine(), out value) && value > 0)
                return value;
            Console.WriteLine("Введите положительное число.");
        } while (true);
    }
    static void Main()
    {
        Console.WriteLine("Расчёт площади и периметра");
        Console.WriteLine("1 — Круг");
        Console.WriteLine("2 — Прямоугольник");
        Console.WriteLine("3 — Треугольник");
        Console.Write("Выберите фигуру: ");
        string choice = Console.ReadLine();
        // Переменная интерфейсного типа может хранить объект любой фигуры, реализующей IFigure
        IFigure figure;
        switch (choice)
        {
            case "1":
                figure = new Circle(ReadPositive("Радиус: "));
                break;
            case "2":
                figure = new Rectangle(
                    ReadPositive("Ширина: "),
                    ReadPositive("Высота: "));
                break;
            case "3":
                Triangle t = new Triangle(
                    ReadPositive("Сторона A: "),
                    ReadPositive("Сторона B: "),
                    ReadPositive("Сторона C: "));
                // Не допускаем расчёт площади несуществующего треугольника
                if (!t.IsValid())
                {
                    Console.WriteLine("Такого треугольника не существует.");
                    return;
                }
                figure = t;
                break;
            default:
                Console.WriteLine("Неверный пункт меню.");
                return;
        }
        // Вызываются методы выбранной фигуры, это полиморфизм, результат зависит от типа объекта.
        Console.WriteLine($"Площадь: {figure.Area():F2}");
        Console.WriteLine($"Периметр: {figure.Perimeter():F2}");
    }
}