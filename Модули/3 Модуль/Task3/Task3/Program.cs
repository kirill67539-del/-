using System;
using System.Collections.Generic;
// Класс, описывающий одну задачу
class TaskItem
{
    public string Name { get; set; }
    // Конструктор для создания задачи с названием
    public TaskItem(string name)
    {
        Name = name;
    }
}
class Program
{
    // Делегат задаёт общий тип методов, которые могут выполнять действие с задачей
    delegate void TaskAction(TaskItem task);
    // Метод отправки уведомления о задаче
    static void SendNotification(TaskItem task)
    {
        Console.WriteLine($"Уведомление: выполнить задачу \"{task.Name}\"");
    }
    // Метод записи задачи в журнал
    static void WriteToLog(TaskItem task)
    {
        Console.WriteLine($"Запись в журнал: задача \"{task.Name}\" добавлена.");
    }
    // Метод выводит все задачи из списка
    static void ShowTasks(List<TaskItem> tasks)
    {
        Console.WriteLine("\nСписок задач:");
        if (tasks.Count == 0)
        {
            Console.WriteLine("Задач нет.");
            return;
        }
        // Перебираем список и выводим номер и название каждой задачи
        for (int i = 0; i < tasks.Count; i++)
            Console.WriteLine($"{i + 1}. {tasks[i].Name}");
    }
    static void Main()
    {
        // List хранит все созданные пользователем задачи
        List<TaskItem> tasks = new List<TaskItem>();
        // Цикл постоянно показывает меню, пока пользователь не выберет выход
        while (true)
        {
            Console.WriteLine("\n   Управление задачами");
            Console.WriteLine("1 - Добавить задачу");
            Console.WriteLine("2 - Показать задачи");
            Console.WriteLine("3 - Выполнить действие");
            Console.WriteLine("0 - Выход");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();
            // Завершение программы
            if (choice == "0")
                break;
            // Добавление новой задачи
            if (choice == "1")
            {
                Console.Write("Название задачи: ");
                string name = Console.ReadLine();
                // Проверяем, что пользователь ввёл название
                if (!string.IsNullOrWhiteSpace(name))
                {
                    tasks.Add(new TaskItem(name));
                    Console.WriteLine("Задача добавлена.");
                }
                else
                {
                    Console.WriteLine("Название не может быть пустым.");
                }
            }
            // Вывод всех задач
            else if (choice == "2")
            {
                ShowTasks(tasks);
            }
            // Выбор задачи и действия над ней
            else if (choice == "3")
            {
                if (tasks.Count == 0)
                {
                    Console.WriteLine("Сначала добавьте задачу.");
                    continue;
                }
                ShowTasks(tasks);
                Console.Write("Номер задачи: ");
                // TryParse безопасно преобразует введённый текст в число
                if (!int.TryParse(Console.ReadLine(), out int number) ||
                    number < 1 || number > tasks.Count)
                {
                    Console.WriteLine("Неверный номер.");
                    continue;
                }
                // Получаем выбранную задачу из списка
                TaskItem task = tasks[number - 1];
                Console.WriteLine("1 - Отправить уведомление");
                Console.WriteLine("2 - Записать в журнал");
                Console.Write("Выберите действие: ");
                string actionChoice = Console.ReadLine();
                // Переменная action будет хранить выбранный метод
                TaskAction action;
                if (actionChoice == "1")
                    action = SendNotification;
                else if (actionChoice == "2")
                    action = WriteToLog;
                else
                {
                    Console.WriteLine("Неверный выбор.");
                    continue;
                }
                // Вызываем выбранный метод через делегат
                action(task);
            }
            // Обработка неправильного пункта меню
            else
            {
                Console.WriteLine("Неверный пункт меню.");
            }
            // После выполнения действия while автоматически возвращает программу к началу меню
        }
        Console.WriteLine("Программа завершена.");
    }
}