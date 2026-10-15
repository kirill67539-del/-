using System;
// Абстрактный класс — общий шаблон для всех фигур
abstract class Figure
{
    // Используем абстрактный метод, каждый класс-наследник должен сам написать способ вычисления площади
    public abstract double Area();
}
class Circle : Figure
{
    // Радиус круга
    private double radius;
    // Конструктор получает радиус при создании объекта
    public Circle(double radius)
    {
        // this.radius — поле объекта, radius — переданный параметр
        this.radius = radius;
    }
    // override заменяет абстрактный метод из класса Figure
    public override double Area()
    {
        // Формула площади круга
        return Math.PI * radius * radius;
    }
}
class Rectangle : Figure
{
    private double width;  // Ширина прямоугольника
    private double height; // Высота прямоугольника
    public Rectangle(double width, double height)
    {
        this.width = width;
        this.height = height;
    }
    // Реализация метода вычисления площади прямоугольника
    public override double Area()
    {
        return width * height;
    }
}
class Triangle : Figure
{
    private double basis;  // Основание треугольника
    private double height; // Высота треугольника
    public Triangle(double basis, double height)
    {
        this.basis = basis;
        this.height = height;
    }
    // Реализация метода вычисления площади треугольника
    public override double Area()
    {
        // Формула площади
        return basis * height / 2;
    }
}
class Program
{
    // Делегат — тип, который позволяет хранить ссылку на метод без параметров
    delegate double AreaDelegate();
    static void Main()
    {
        // Создаём объекты фигур и передаём их размеры
        Circle circle = new Circle(5);
        Rectangle rectangle = new Rectangle(4, 6);
        Triangle triangle = new Triangle(8, 5);
        // Сохраняем ссылки на методы Area() в переменных-делегатах, методы иожно вызвать через соответствующие делегаты.
        AreaDelegate circleArea = circle.Area;
        AreaDelegate rectangleArea = rectangle.Area;
        AreaDelegate triangleArea = triangle.Area;
        Console.WriteLine("Круг:");
        // Вызов делегата запускает метод Area() круга
        Console.WriteLine("Площадь: " + circleArea());
        Console.WriteLine("Прямоугольник:");
        // Вызов делегата запускает метод Area() прямоугольника
        Console.WriteLine("Площадь: " + rectangleArea());
        Console.WriteLine("Треугольник:");
        // Вызов делегата запускает метод Area() треугольника
        Console.WriteLine("Площадь: " + triangleArea());
    }
}
