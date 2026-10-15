using System;
using System.Collections.Generic;
// Интерфейс задаёт методы, которые должен реализовать товар
interface IProduct
{
    double Cost(int quantity);
    int Stock();
}
class Product : IProduct
{
    public string Name { get; private set; }
    // protected позволяет дочерним классам использовать эти поля
    protected double Price;
    protected int Quantity;
    // Конструктор задаёт название, цену и количество товара
    public Product(string name, double price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
    // Вычисляем стоимость выбранного количества товара
    public double Cost(int quantity) => Price * quantity;
    // Возвращаем количество товара на складе
    public int Stock() => Quantity;
}
// Класс продуктов питания наследуется от Product
class FoodProduct : Product
{
    // base передаёт параметры конструктору родительского класса
    public FoodProduct(string name, double price, int quantity)
        : base(name, price, quantity) { }
}
// Класс хозяйственных товаров также наследуется от Product
class HouseholdProduct : Product
{
    public HouseholdProduct(string name, double price, int quantity)
        : base(name, price, quantity) { }
}
class Program
{
    static void Main()
    {
        // Список хранит разные виды товаров через общий тип Product
        List<Product> products = new List<Product>
        {
            new FoodProduct("Молоко", 95, 20),
            new FoodProduct("Хлеб", 50, 15),
            new HouseholdProduct("Мыло", 80, 10)
        };
        Console.WriteLine("Товары магазина");
        // Выводим названия товаров и их остатки на складе
        for (int i = 0; i < products.Count; i++)
            Console.WriteLine($"{i + 1}. {products[i].Name}, остаток: {products[i].Stock()}");
        Console.Write("Выберите товар: ");
        // Проверяем, что введён номер существующего товара
        if (!int.TryParse(Console.ReadLine(), out int index) ||
            index < 1 || index > products.Count)
        {
            Console.WriteLine("Неверный номер товара.");
            return;
        }
        Console.Write("Введите количество: ");
        // Проверяем, что количество является положительным целым числом
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Количество должно быть положительным.");
            return;
        }
        // Получаем выбранный товар из списка, index - 1 нужен, индексы списка начинаются с нуля
        Product selected = products[index - 1];
        // Проверяем, достаточно ли товара на складе
        if (count > selected.Stock())
        {
            Console.WriteLine("На складе недостаточно товара.");
            return;
        }
        // Рассчитываем стоимость покупки и показываем остаток после неё
        Console.WriteLine($"Стоимость: {selected.Cost(count):F2}");
        Console.WriteLine($"Остаток после покупки: {selected.Stock() - count}");
    }
}