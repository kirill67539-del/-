using System;
// Класс BankAccount описывает банковский счет
class BankAccount
{
    // Приватные поля счета
    private string accountNumber;
    private string owner;
    private double balance;
    // Конструктор для создания банковского счета
    public BankAccount(string accountNumber, string owner, double balance)
    {
        this.accountNumber = accountNumber;
        this.owner = owner;
        this.balance = balance;
    }
    // Метод для получения номера счета
    public string GetAccountNumber()
    {
        return accountNumber;
    }
    // Метод для изменения номера счета
    public void SetAccountNumber(string accountNumber)
    {
        this.accountNumber = accountNumber;
    }
    // Метод для получения имени владельца
    public string GetOwner()
    {
        return owner;
    }
    // Метод для изменения имени владельца
    public void SetOwner(string owner)
    {
        this.owner = owner;
    }
    // Метод для получения текущего баланса
    public double GetBalance()
    {
        return balance;
    }
    // Метод для пополнения счета
    public void Deposit(double amount)
    {
        // Проверяем, что сумма пополнения положительная
        if (amount > 0)
        {
            balance += amount;
            Console.WriteLine("Счет пополнен на " + amount + " руб.");
        }
        else
        {
            Console.WriteLine("Сумма пополнения должна быть больше нуля.");
        }
    }
    // Метод для снятия денег со счета
    public void Withdraw(double amount)
    {
        // Проверяем корректность суммы снятия
        if (amount <= 0)
        {
            Console.WriteLine("Сумма снятия должна быть больше нуля.");
        }
        // Проверяем, достаточно ли денег на счете
        else if (amount > balance)
        {
            Console.WriteLine("Недостаточно средств на счете.");
        }
        else
        {
            // Уменьшаем баланс на сумму снятия
            balance -= amount;
            Console.WriteLine("Со счета снято " + amount + " руб.");
        }
    }
    // Метод для вывода полной информации о счете
    public void PrintInfo()
    {
        Console.WriteLine("Номер счета: " + accountNumber);
        Console.WriteLine("Владелец: " + owner);
        Console.WriteLine("Баланс: " + balance + " руб.");
    }
}
class Program
{
    // Главный метод программы
    static void Main()
    {
        // Создаем два объекта банковского счета
        BankAccount account1 = new BankAccount(
            "123456",
            "Иван Иванов",
            500
        );
        BankAccount account2 = new BankAccount(
            "654321",
            "Анна Петрова",
            300
        );
        // Вывод начальной информации по первому счету
        Console.WriteLine("Счет  1:");
        account1.PrintInfo();
        // Пополнение первого счета
        Console.WriteLine("\nПополняем счет");
        account1.Deposit(100);
        // Снятие денег с первого счета
        Console.WriteLine("\nСнимаем деньги");
        account1.Withdraw(150);
        // Вывод итогового баланса первого счета
        Console.WriteLine("\nИтоговая информация:");
        account1.PrintInfo();
        // Вывод начальной информации по второму счету
        Console.WriteLine("\nСчет 2");
        account2.PrintInfo();
        // Пополнение второго счета
        Console.WriteLine("\nПополняем счет");
        account2.Deposit(50);
        // Снятие денег со второго счета
        Console.WriteLine("\nСнимаем деньги");
        account2.Withdraw(100);
        // Вывод итогового баланса второго счета
        Console.WriteLine("\nИтоговая информация:");
        account2.PrintInfo();
    }
}