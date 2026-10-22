using Microsoft.Data.Sqlite; // Работа с базой SQLite
using System;
using System.Data; // Таблица данных
using System.Windows; // Элементы WPF
using System.Windows.Controls; // Элементы управления
namespace EmployeesApp
{
    public partial class MainWindow : Window
    {
        string cs = "Data Source=employees.db"; // Файл базы данных
        public MainWindow()
        {
            InitializeComponent(); // Загружаем интерфейс
            using var c = new SqliteConnection(cs);
            c.Open(); // Открываем базу данных
            using var cmd = c.CreateCommand();
            // Создаём таблицу сотрудников, если её нет.
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Employees(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT,
                Position TEXT,
                Phone TEXT,
                Salary REAL)";
            cmd.ExecuteNonQuery();
            // Добавляем столбец Phone в старую базу.
            using var alter = c.CreateCommand();
            alter.CommandText = "ALTER TABLE Employees ADD COLUMN Phone TEXT";
            try
            {
                alter.ExecuteNonQuery();
            }
            catch (SqliteException)
            {
                // Столбец уже существует.
            }
            LoadData(); // Загружаем сотрудников
        }
        // Загружает сотрудников в таблицу.
        void LoadData()
        {
            using var c = new SqliteConnection(cs);
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = "SELECT * FROM Employees"; // Получаем записи
            using var reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader); // Заполняем таблицу данными
            Grid.ItemsSource = dt.DefaultView; // Показываем данные
        }
        // Добавляет нового сотрудника.
        void Add_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем обязательные поля.
            if (string.IsNullOrWhiteSpace(NameBox.Text) ||
                string.IsNullOrWhiteSpace(PositionBox.Text))
            {
                MessageBox.Show("Введите ФИО и должность.");
                return; // Прерываем добавление
            }
            // Проверяем правильность зарплаты.
            if (!double.TryParse(SalaryBox.Text, out double salary))
            {
                MessageBox.Show("Введите правильную зарплату.");
                return;
            }
            using var c = new SqliteConnection(cs);
            c.Open();
            using var cmd = c.CreateCommand();
            // Добавляем данные сотрудника в базу.
            cmd.CommandText = @"INSERT INTO Employees
                (Name, Position, Phone, Salary)
                VALUES ($n, $p, $ph, $s)";
            cmd.Parameters.AddWithValue("$n", NameBox.Text);
            cmd.Parameters.AddWithValue("$p", PositionBox.Text);
            cmd.Parameters.AddWithValue("$ph", PhoneBox.Text);
            cmd.Parameters.AddWithValue("$s", salary);
            cmd.ExecuteNonQuery(); // Сохраняем запись
            LoadData(); // Обновляем таблицу
        }
        // Изменяет данные выбранного сотрудника.
        void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem is not DataRowView r) return;
            // Проверяем зарплату.
            if (!double.TryParse(SalaryBox.Text, out double salary))
            {
                MessageBox.Show("Введите правильную зарплату.");
                return;
            }
            using var c = new SqliteConnection(cs);
            c.Open();
            using var cmd = c.CreateCommand();
            // Обновляем данные сотрудника с указанным Id.
            cmd.CommandText = @"UPDATE Employees SET
                Name=$n, Position=$p, Phone=$ph, Salary=$s
                WHERE Id=$id";
            cmd.Parameters.AddWithValue("$n", NameBox.Text);
            cmd.Parameters.AddWithValue("$p", PositionBox.Text);
            cmd.Parameters.AddWithValue("$ph", PhoneBox.Text);
            cmd.Parameters.AddWithValue("$s", salary);
            cmd.Parameters.AddWithValue("$id", r["Id"]);
            cmd.ExecuteNonQuery(); // Сохраняем изменения
            LoadData(); // Обновляем таблицу
        }
        // Удаляет выбранного сотрудника.
        void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem is not DataRowView r) return;
            using var c = new SqliteConnection(cs);
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = "DELETE FROM Employees WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", r["Id"]);
            cmd.ExecuteNonQuery(); // Удаляем запись
            LoadData(); // Обновляем таблицу
        }
        // Переносит данные выбранной строки в поля.
        void Grid_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            if (Grid.SelectedItem is not DataRowView r) return;
            NameBox.Text = r["Name"].ToString();
            PositionBox.Text = r["Position"].ToString();
            PhoneBox.Text = r["Phone"].ToString();
            SalaryBox.Text = r["Salary"].ToString();
        }
    }
}