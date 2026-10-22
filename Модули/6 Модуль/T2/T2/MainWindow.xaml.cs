using Microsoft.Data.Sqlite; // Работа с базой SQLite
using System;
using System.Data; // Таблица данных
using System.Windows; // Элементы WPF
using System.Windows.Controls; // Элементы управления WPF
namespace TasksApp
{
    public partial class MainWindow : Window
    {
        // Подключение к базе данных, в которой хранятся задачи.
        string connectionString = "Data Source=tasks.db";
        public MainWindow()
        {
            InitializeComponent();
            // При запуске создаём таблицу, если её ещё нет
            CreateDatabase();
            LoadTasks();
        }
        // Создаёт таблицу для хранения задач.
        void CreateDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // Id — уникальный номер задачи, Title — её название, Status — текущее состояние.
                string sql = @"CREATE TABLE IF NOT EXISTS Tasks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT,
                    Status TEXT)";
                using (var command = new SqliteCommand(sql, connection))
                {
                    // Выполняем команду создания таблицы.
                    command.ExecuteNonQuery();
                }
            }
        }
        // Загружает задачи из базы данных и показывает их в таблице.
        void LoadTasks()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // Получаем все сохранённые задачи.
                string sql = "SELECT * FROM Tasks";
                using (var command = new SqliteCommand(sql, connection))
                {
                    DataTable table = new DataTable();
                    // Читаем результаты запроса и помещаем их в таблицу.
                    using (var reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                    // Связываем данные с таблицей в окне программы.
                    TasksGrid.ItemsSource = table.DefaultView;
                }
            }
        }
        // Возвращает выбранный статус задачи, если статус не выбран, устанавливает «Не выполнена».
        string GetStatus()
        {
            return (StatusBox.SelectedItem as ComboBoxItem)
                ?.Content.ToString() ?? "Не выполнена";
        }
        // Добавляет новую задачу в базу данных.
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Не разрешаем добавлять задачу без названия.
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Введите название задачи.");
                return;
            }
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // Сохраняем название и статус новой задачи.
                string sql = "INSERT INTO Tasks (Title, Status) VALUES (@title, @status)";
                using (var command = new SqliteCommand(sql, connection))
                {
                    // Передаём значения через параметры запроса, чтобы безопасно подставить пользовательские данные.
                    command.Parameters.AddWithValue("@title", TitleBox.Text);
                    command.Parameters.AddWithValue("@status", GetStatus());
                    // Выполняем добавление записи.
                    command.ExecuteNonQuery();
                }
            }
            // Обновляем список и очищаем поле для следующей задачи.
            LoadTasks();
            TitleBox.Clear();
        }
        // Изменяет название и статус выбранной задачи.
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбрана ли задача в таблице, DataRowView содержит данные выбранной строки.
            if (TasksGrid.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Выберите задачу.");
                return;
            }
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // UPDATE изменяет запись с указанным Id.
                string sql = @"UPDATE Tasks
                               SET Title = @title, Status = @status
                               WHERE Id = @id";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@title", TitleBox.Text);
                    command.Parameters.AddWithValue("@status", GetStatus());
                    // Id определяет, какую именно задачу нужно изменить.
                    command.Parameters.AddWithValue("@id", row["Id"]);
                    command.ExecuteNonQuery();
                }
            }
            // Показываем обновлённые данные в таблице.
            LoadTasks();
        }
        // Удаляет выбранную задачу из базы данных.
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Без выбранной строки невозможно определить, какую задачу необходимо удалить.
            if (TasksGrid.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Выберите задачу.");
                return;
            }
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // Удаляем только ту запись, у которой совпадает Id.
                string sql = "DELETE FROM Tasks WHERE Id = @id";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", row["Id"]);
                    command.ExecuteNonQuery();
                }
            }
            // Обновляем таблицу и очищаем поле названия.
            LoadTasks();
            TitleBox.Clear();
        }
        // При выборе задачи переносит её данные в поля ввода, чтобы пользователь мог изменить название или статус.
        private void TasksGrid_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (TasksGrid.SelectedItem is DataRowView row)
            {
                // Заполняем поле названием выбранной задачи.
                TitleBox.Text = row["Title"].ToString();
                // Находим в списке статусов тот, который сохранён в базе.
                foreach (ComboBoxItem item in StatusBox.Items)
                {
                    if (item.Content.ToString() == row["Status"].ToString())
                    {
                        StatusBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }
    }
}