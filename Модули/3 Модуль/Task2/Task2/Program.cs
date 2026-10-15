using System;
class Notification
{
    // События для трёх типов уведомлений, Action<string> значит, что событие передаёт одну строку.
    public event Action<string> MessageReceived;
    public event Action<string> CallReceived;
    public event Action<string> EmailReceived;
    public void SendMessage(string message)
    {
        // Вызываем событие и передаём ему текст сообщения.
        // ?. проверяет, есть ли методы, подписанные на событие.
        MessageReceived?.Invoke(message);
    }
    public void MakeCall(string phoneNumber)
    {
        // Вызываем событие и передаём номер телефона.
        CallReceived?.Invoke(phoneNumber);
    }
    public void SendEmail(string email)
    {
        // Вызываем событие и передаём адрес отправителя.
        EmailReceived?.Invoke(email);
    }
}
class Program
{
    // Метод-обработчик события сообщения, автоматически вызывается после срабатывания MessageReceived.
    static void OnMessageReceived(string message)
    {
        Console.WriteLine();
        Console.WriteLine($"Получено сообщение: {message}");
    }
    // Метод-обработчик события звонка.
    static void OnCallReceived(string phoneNumber)
    {
        Console.WriteLine();
        Console.WriteLine($"Входящий звонок с номера: {phoneNumber}");
    }
    // Метод-обработчик события электронной почты.
    static void OnEmailReceived(string email)
    {
        Console.WriteLine();
        Console.WriteLine($"Получено электронное письмо от: {email}");
    }
    static void Main()
    {
        // Создаём объект системы уведомлений.
        Notification notification = new Notification();
        // Подписываем обработчики на соответствующие события, += добавляет метод в список обработчиков события.
        notification.MessageReceived += OnMessageReceived;
        notification.CallReceived += OnCallReceived;
        notification.EmailReceived += OnEmailReceived;
        // Переменная управляет работой основного цикла программы.
        bool running = true;
        // Меню повторяется, пока running имеет значение true.
        while (running)
        {
            Console.Clear();
            Console.WriteLine("   Система уведомлений");
            Console.WriteLine();
            Console.WriteLine("1 - Отправить сообщение");
            Console.WriteLine("2 - Входящий звонок");
            Console.WriteLine("3 - Получить электронное письмо");
            Console.WriteLine("0 - Выход");
            Console.WriteLine();
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();
            // Проверка выбранного пользователем пункта меню.
            switch (choice)
            {
                case "1":
                    Console.Write("Введите текст сообщения: ");
                    string message = Console.ReadLine();
                    // Вызов метода, который запускает событие сообщения.
                    notification.SendMessage(message);
                    break;
                case "2":
                    Console.Write("Введите номер телефона: ");
                    string phoneNumber = Console.ReadLine();
                    // Вызыв метода, который запускает событие звонка.
                    notification.MakeCall(phoneNumber);
                    break;
                case "3":
                    Console.Write("Введите адрес отправителя: ");
                    string email = Console.ReadLine();
                    // Вызыв метода, который запускает событие электронной почты.
                    notification.SendEmail(email);
                    break;
                case "0":
                    // Останавливание цикл и завершение программы.
                    running = false;
                    Console.WriteLine("Программа завершена.");
                    break;
                default:
                    // Выполняется, если пользователь ввёл неизвестный пункт.
                    Console.WriteLine("Неверный пункт меню.");
                    break;
            }
            // Если программа ещё работает, ждём нажатия клавиши перед повторным показом меню.
            if (running)
            {
                Console.WriteLine("Для продолжения нажать клавишу");
                Console.ReadKey();
            }
        }
    }
}