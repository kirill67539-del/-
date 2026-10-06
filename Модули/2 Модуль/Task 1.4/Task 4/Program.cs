using System;

// Интерфейс задаёт общий метод Draw()
interface IDrawable
{
    void Draw();
}
// Класс Circle реализует интерфейс IDrawable
class Circle : IDrawable
{
    // Радиус круга
    private double radius;
    // Конструктор для создания объекта круга
    public Circle(double radius)
    {
        this.radius = radius;
    }
    // Реализация метода Draw() для круга
    public void Draw()
    {
        Console.WriteLine("Рисуется круг с радиусом " + radius);
    }
}
// Класс Rectangle реализует интерфейс IDrawable
class Rectangle : IDrawable
{
    // Размеры прямоугольника
    private double width;
    private double height;
    // Конструктор для создания прямоугольника
    public Rectangle(double width, double height)
    {
        this.width = width;
        this.height = height;
    }
    // Реализация метода Draw() для прямоугольника
    public void Draw()
    {
        Console.WriteLine("Рисуется прямоугольник: ширина = "
            + width + ", высота = " + height);
    }
}
// Класс Triangle реализует интерфейс IDrawable
class Triangle : IDrawable
{
    // Сторона треугольника
    private double side;
    // Конструктор для создания треугольника
    public Triangle(double side)
    {
        this.side = side;
    }
    // Реализация метода Draw() для треугольника
    public void Draw()
    {
        Console.WriteLine("Рисуется треугольник со стороной " + side);
    }
}
class Program
{
    // Главный метод программы
    static void Main()
    {
        // Массив типа интерфейса может хранить объекты разных классов,
        // если они реализуют интерфейс IDrawable
        IDrawable[] objects =
        {
            new Circle(5),
            new Rectangle(4, 6),
            new Triangle(7)
        };
        // Используется полиморфизм, для каждого объекта вызывается его собственная реализация метода Draw()
        foreach (IDrawable obj in objects)
        {
            obj.Draw();
        }
    }
}