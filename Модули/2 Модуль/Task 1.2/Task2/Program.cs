using System;
// Базовый класс для всех геометрических фигур
class Shape
{
    // Виртуальный метод для вычисления площади
    // Дочерние классы могут изменить его реализацию
    public virtual double Area()
    {
        return 0;
    }
    // Виртуальный метод для вычисления периметра
    public virtual double Perimeter()
    {
        return 0;
    }
}
// Класс Circle наследуется от Shape
class Circle : Shape
{
    // Радиус круга
    private double radius;
    // Конструктор для создания круга
    public Circle(double radius)
    {
        this.radius = radius;
    }
    // Переопределение метода Area() для круга
    public override double Area()
    {
        // Формула площади круга: π * r²
        return Math.PI * radius * radius;
    }
    // Переопределение метода Perimeter() для круга
    public override double Perimeter()
    {
        // Формула длины окружности: 2 * π * r
        return 2 * Math.PI * radius;
    }
}
// Класс Rectangle наследуется от Shape
class Rectangle : Shape
{
    // Ширина и высота прямоугольника
    private double width;
    private double height;
    // Конструктор для создания прямоугольника
    public Rectangle(double width, double height)
    {
        this.width = width;
        this.height = height;
    }
    // Переопределение метода Area() для прямоугольника
    public override double Area()
    {
        // Формула площади: ширина * высота
        return width * height;
    }
    // Переопределение метода Perimeter() для прямоугольника
    public override double Perimeter()
    {
        // Формула периметра: 2 * (ширина + высота)
        return 2 * (width + height);
    }
}
class Program
{
    // Главный метод программы
    static void Main()
    {
        // Создание объектов круга и прямоугольника
        Circle circle = new Circle(5);
        Rectangle rectangle = new Rectangle(4, 6);
        // Вывод площади и периметра круга
        Console.WriteLine("Круг:");
        Console.WriteLine("Площадь: " + circle.Area());
        Console.WriteLine("Периметр: " + circle.Perimeter());
        Console.WriteLine();
        // Вывод площади и периметра прямоугольника
        Console.WriteLine("Прямоугольник:");
        Console.WriteLine("Площадь: " + rectangle.Area());
        Console.WriteLine("Периметр: " + rectangle.Perimeter());
    }
}