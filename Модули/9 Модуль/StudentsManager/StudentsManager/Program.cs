using System.Text.Json;
namespace StudentsManager;
class Program
{
    static List<Student> students = new();
    static string file = "students.json";
    static void Main()
    {
        Load();
        while (true)
        {
            Console.WriteLine("\n=== Управление студентами ===");
            Console.WriteLine("1. Показать студентов");
            Console.WriteLine("2. Добавить студента");
            Console.WriteLine("3. Редактировать студента");
            Console.WriteLine("4. Удалить студента");
            Console.WriteLine("5. Поиск студента");
            Console.WriteLine("6. Сортировка по имени");
            Console.WriteLine("7. Сортировка по баллу");
            Console.WriteLine("8. Сохранить данные");
            Console.WriteLine("0. Выход");
            Console.Write("Выбор: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Show(students);
                    break;
                case "2":
                    Add();
                    break;
                case "3":
                    Edit();
                    break;
                case "4":
                    Delete();
                    break;
                case "5":
                    Search();
                    break;
                case "6":
                    Show(students.OrderBy(s => s.Name).ToList());
                    break;
                case "7":
                    Show(students.OrderByDescending(s => s.Grade).ToList());
                    break;
                case "8":
                    Save();
                    break;
                case "0":
                    Save();
                    return;
                default:
                    Console.WriteLine("Неверный пункт меню.");
                    break;
            }
        }
    }
    // Вывод списка студентов.
    static void Show(IEnumerable<Student> list)
    {
        if (!list.Any())
        {
            Console.WriteLine("Список пуст.");
            return;
        }
        int number = 1;
        foreach (var student in list)
        {
            Console.WriteLine(
                $"{number++}. {student.Name}, " +
                $"возраст: {student.Age}, балл: {student.Grade}");
        }
    }
    // Добавление студента.
    static void Add()
    {
        var student = new Student();
        Console.Write("Имя: ");
        student.Name = Console.ReadLine() ?? "";
        Console.Write("Возраст: ");
        int.TryParse(Console.ReadLine(), out int age);
        student.Age = age;
        Console.Write("Средний балл: ");
        double.TryParse(Console.ReadLine(), out double grade);
        student.Grade = grade;
        students.Add(student);
        Console.WriteLine("Студент добавлен.");
    }
    // Редактирование студента.
    static void Edit()
    {
        Show(students);
        Console.Write("Номер студента: ");
        if (!int.TryParse(Console.ReadLine(), out int number) ||
            number < 1 || number > students.Count)
        {
            Console.WriteLine("Неверный номер.");
            return;
        }
        var student = students[number - 1];
        Console.Write("Новое имя: ");
        student.Name = Console.ReadLine() ?? "";
        Console.Write("Новый возраст: ");
        int.TryParse(Console.ReadLine(), out int age);
        student.Age = age;
        Console.Write("Новый средний балл: ");
        double.TryParse(Console.ReadLine(), out double grade);
        student.Grade = grade;
        Console.WriteLine("Данные обновлены.");
    }
    // Удаление студента.
    static void Delete()
    {
        Show(students);
        Console.Write("Номер студента: ");
        if (!int.TryParse(Console.ReadLine(), out int number) ||
            number < 1 || number > students.Count)
        {
            Console.WriteLine("Неверный номер.");
            return;
        }
        students.RemoveAt(number - 1);
        Console.WriteLine("Студент удалён.");
    }
    // Поиск по имени.
    static void Search()
    {
        Console.Write("Введите имя: ");
        string query = Console.ReadLine() ?? "";
        var result = students
            .Where(s => s.Name.Contains(
                query, StringComparison.OrdinalIgnoreCase))
            .ToList();
        Show(result);
    }
    // Сохранение данных в JSON-файл.
    static void Save()
    {
        string json = JsonSerializer.Serialize(
            students,
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(file, json);
        Console.WriteLine("Данные сохранены.");
    }
    // Загрузка данных из JSON-файла.
    static void Load()
    {
        if (File.Exists(file))
        {
            string json = File.ReadAllText(file);
            students = JsonSerializer
                .Deserialize<List<Student>>(json) ?? new List<Student>();
        }
    }
}