using Microsoft.Data.Sqlite; // Работа с базой данных SQLite
using System;
using System.Data; // Работа с таблицами данных
using System.Windows; // Окна и сообщения
using System.Windows.Controls; // Кнопки, таблицы и другие элементы WPF
namespace ProjectManager
{
    public partial class MainWindow : Window
    {
        // Строка подключения к базе данных projects.db
        string cs = "Data Source=projects.db;Foreign Keys=True";
        // Конструктор главного окна программы
        public MainWindow()
        {
            InitializeComponent(); // Загружаем интерфейс из MainWindow.xaml
            // Открываем подключение к базе данных
            using var con = new SqliteConnection(cs);
            con.Open();
            // Создаём таблицы, если их ещё нет в базе данных
            string sql = @"
                CREATE TABLE IF NOT EXISTS Projects(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL, Description TEXT, Status TEXT);
                CREATE TABLE IF NOT EXISTS Employees(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FullName TEXT NOT NULL, Position TEXT);
                CREATE TABLE IF NOT EXISTS Tasks(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL, ProjectId INTEGER, EmployeeId INTEGER,
                    DueDate TEXT, Priority TEXT, Status TEXT, Progress INTEGER,
                    FOREIGN KEY(ProjectId) REFERENCES Projects(Id) ON DELETE SET NULL,
                    FOREIGN KEY(EmployeeId) REFERENCES Employees(Id) ON DELETE SET NULL);";
            // Выполняем команду создания таблиц
            new SqliteCommand(sql, con).ExecuteNonQuery();
            // Загружаем данные из базы в таблицы интерфейса
            LoadData();
        }
        // Обновляет содержимое всех трёх таблиц на экране
        void LoadData()
        {
            ProjectsGrid.ItemsSource = GetTable("Projects").DefaultView;
            EmployeesGrid.ItemsSource = GetTable("Employees").DefaultView;
            TasksGrid.ItemsSource = GetTable("Tasks").DefaultView;
        }
        // Получает данные из указанной таблицы базы данных
        DataTable GetTable(string table)
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            // Выполняем запрос на получение всех записей
            using var cmd = new SqliteCommand("SELECT * FROM " + table, con);
            using var reader = cmd.ExecuteReader();
            // Переносим результат запроса в объект DataTable
            var dt = new DataTable();
            dt.Load(reader);
            return dt; // Возвращаем полученную таблицу
        }
        // Универсальный метод для выполнения SQL-команд
        // Параметры передаются отдельно, чтобы безопасно подставлять значения
        void Run(string sql, params (string, object)[] p)
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            using var cmd = new SqliteCommand(sql, con);
            // Добавляем параметры запроса
            foreach (var x in p)
                cmd.Parameters.AddWithValue(x.Item1, x.Item2 ?? DBNull.Value);
            // Выполняем команду добавления, изменения или удаления
            cmd.ExecuteNonQuery();
        }
        // Создаёт небольшое окно для ввода одного значения
        // Возвращает введённый текст или null, если окно закрыли
        string Ask(string title, string message)
        {
            var w = new Window
            {
                Title = title,
                Width = 340,
                Height = 180,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize
            };
            // Размещаем элементы ввода внутри окна
            var panel = new StackPanel { Margin = new Thickness(15) };
            // Текст с подсказкой пользователю
            panel.Children.Add(new TextBlock
            {
                Text = message,
                Margin = new Thickness(0, 0, 0, 8)
            });
            // Поле, в которое пользователь вводит данные
            var input = new TextBox { Height = 28 };
            panel.Children.Add(input);
            // Кнопка подтверждения ввода
            var button = new Button
            {
                Content = "Добавить",
                Margin = new Thickness(0, 12, 0, 0)
            };
            // Закрываем окно с подтверждением после нажатия
            button.Click += (s, e) => w.DialogResult = true;
            panel.Children.Add(button);
            w.Content = panel;
            // Возвращаем введённый текст
            return w.ShowDialog() == true ? input.Text.Trim() : null;
        }
        // Добавляет новый проект в базу данных
        void AddProject(object s, RoutedEventArgs e)
        {
            // Запрашиваем название проекта
            string name = Ask("Новый проект", "Название проекта:");
            // Не добавляем запись, если название не введено
            if (string.IsNullOrWhiteSpace(name)) return;
            // Запрашиваем описание проекта
            string desc = Ask("Описание", "Описание проекта:") ?? "";
            // Сохраняем проект со статусом «В работе»
            Run("INSERT INTO Projects(Name,Description,Status) VALUES(@n,@d,@s)",
                ("@n", name), ("@d", desc), ("@s", "В работе"));
            LoadData(); // Обновляем таблицы
        }
        // Добавляет нового сотрудника
        void AddEmployee(object s, RoutedEventArgs e)
        {
            // Запрашиваем ФИО сотрудника
            string name = Ask("Новый сотрудник", "ФИО:");
            if (string.IsNullOrWhiteSpace(name)) return;
            // Запрашиваем должность сотрудника
            string job = Ask("Должность", "Должность сотрудника:") ?? "";
            // Записываем сотрудника в базу данных
            Run("INSERT INTO Employees(FullName,Position) VALUES(@n,@p)",
                ("@n", name), ("@p", job));
            LoadData(); // Обновляем таблицы
        }
        // Добавляет новую задачу
        void AddTask(object s, RoutedEventArgs e)
        {
            // Запрашиваем название задачи
            string title = Ask("Новая задача", "Название задачи:");
            if (string.IsNullOrWhiteSpace(title)) return;
            // Запрашиваем срок выполнения и приоритет
            string date = Ask("Срок", "Дата (ГГГГ-ММ-ДД):") ?? "";
            string priority = Ask("Приоритет",
                "Низкий, Средний или Высокий:") ?? "Средний";
            // Добавляем задачу с начальным статусом и прогрессом 0%
            Run(@"INSERT INTO Tasks(Title,DueDate,Priority,Status,Progress)
                  VALUES(@t,@d,@p,@s,@g)",
                ("@t", title), ("@d", date), ("@p", priority),
                ("@s", "Новая"), ("@g", 0));
            LoadData(); // Показываем обновлённый список задач
        }
        // Удаляет выбранную запись из указанной таблицы
        void Delete(DataGrid grid, string table)
        {
            // Проверяем, выбрал ли пользователь запись
            if (grid.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Выберите запись.");
                return;
            }
            // Просим подтвердить удаление
            if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
                MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
            // Удаляем запись по её уникальному идентификатору
            Run("DELETE FROM " + table + " WHERE Id=@id",
                ("@id", row["Id"]));
            LoadData(); // Обновляем таблицы
        }
        // Обработчики кнопок удаления для разных разделов
        void DeleteProject(object s, RoutedEventArgs e)
            => Delete(ProjectsGrid, "Projects");
        void DeleteEmployee(object s, RoutedEventArgs e)
            => Delete(EmployeesGrid, "Employees");
        void DeleteTask(object s, RoutedEventArgs e)
            => Delete(TasksGrid, "Tasks");
        // Сохраняет изменения, внесённые непосредственно в таблицу
        void Save(DataGrid grid, string table, string[] cols)
        {
            // Завершаем редактирование текущей ячейки и строки
            grid.CommitEdit(DataGridEditingUnit.Cell, true);
            grid.CommitEdit(DataGridEditingUnit.Row, true);
            // Проверяем, что источник данных является таблицей DataView
            if (grid.ItemsSource is not DataView view) return;
            // Перебираем все строки таблицы
            foreach (DataRow r in view.Table.Rows)
            {
                // Пропускаем строки, которые не изменялись
                if (r.RowState != DataRowState.Modified) continue;
                // Формируем список полей для обновления
                string set = string.Join(",", Array.ConvertAll(cols,
                    c => c + "=@" + c));
                using var con = new SqliteConnection(cs);
                con.Open();
                // Создаём запрос UPDATE для сохранения изменений
                using var cmd = new SqliteCommand(
                    "UPDATE " + table + " SET " + set + " WHERE Id=@id", con);
                // Передаём новые значения изменённых полей
                foreach (string c in cols)
                    cmd.Parameters.AddWithValue("@" + c, r[c] ?? DBNull.Value);
                // Указываем, какую именно запись нужно изменить
                cmd.Parameters.AddWithValue("@id", r["Id"]);
                cmd.ExecuteNonQuery(); // Сохраняем изменения в SQLite
            }
            LoadData(); // Обновляем таблицы после сохранения
            MessageBox.Show("Изменения сохранены.");
        }
        // Сохраняет изменения в таблице проектов
        void SaveProjects(object s, RoutedEventArgs e) =>
            Save(ProjectsGrid, "Projects",
                new[] { "Name", "Description", "Status" });
        // Сохраняет изменения в таблице сотрудников
        void SaveEmployees(object s, RoutedEventArgs e) =>
            Save(EmployeesGrid, "Employees",
                new[] { "FullName", "Position" });
        // Сохраняет изменения в таблице задач
        void SaveTasks(object s, RoutedEventArgs e) =>
            Save(TasksGrid, "Tasks", new[] {
                "Title", "ProjectId", "EmployeeId", "DueDate",
                "Priority", "Status", "Progress"
            });
        // Формирует отчёт о количестве задач и их выполнении по проектам
        void ShowReport(object s, RoutedEventArgs e)
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            // Считаем общее число задач, завершённые задачи и средний прогресс
            string sql = @"
                SELECT p.Name, COUNT(t.Id) AS Total,
                SUM(CASE WHEN t.Status='Завершена' THEN 1 ELSE 0 END) AS Done,
                COALESCE(AVG(t.Progress),0) AS Progress
                FROM Projects p LEFT JOIN Tasks t ON p.Id=t.ProjectId
                GROUP BY p.Id";
            using var cmd = new SqliteCommand(sql, con);
            using var r = cmd.ExecuteReader();
            // Собираем текст отчёта
            string report = "ОТЧЁТ ПО ПРОЕКТАМ\n\n";
            while (r.Read())
            {
                report += $"{r["Name"]}\nЗадач: {r["Total"]}\n" +
                    $"Завершено: {r["Done"]}\n" +
                    $"Прогресс: {Convert.ToDouble(r["Progress"]):F0}%\n\n";
            }
            // Выводим готовый отчёт пользователю
            MessageBox.Show(report, "Отчёт");
        }
    }
}