using Microsoft.Data.Sqlite;
using System.Data;
using System.Windows;
using System.Windows.Controls;
namespace LibraryApp
{
    public partial class MainWindow : Window
    {
        // Подключение к файлу базы данных библиотеки.
        string cs = "Data Source=library.db";
        public MainWindow()
        {
            InitializeComponent();
            // Создаём таблицу книг, если её ещё нет в базе данных.
            using var c = new SqliteConnection(cs);
            c.Open();
            // Id автоматически присваивается каждой новой книге, по умолчанию книга — «Доступна».
            using var cmd = c.CreateCommand();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Books(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT, Author TEXT,
                Status TEXT DEFAULT 'Доступна')";
            cmd.ExecuteNonQuery();
            // После создания таблицы загружаем книги в интерфейс.
            LoadData();
        }
        // Загружает книги из базы данных в таблицу приложения.
        // Если указан поисковый запрос, показывает только подходящие книги.
        void LoadData(string search = "")
        {
            using var c = new SqliteConnection(cs);
            c.Open();
            using var cmd = c.CreateCommand();
            // Ищем совпадения по названию или автору.
            // Символы % позволяют найти текст даже внутри слова.
            cmd.CommandText = @"SELECT * FROM Books
                WHERE Title LIKE $s OR Author LIKE $s";
            // Передаём поисковый текст отдельным параметром, чтобы подставить его в запрос к базе данных.
            cmd.Parameters.AddWithValue("$s", "%" + search + "%");
            // Получаем найденные записи и передаём их в таблицу интерфейса.
            using var reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            Grid.ItemsSource = dt.DefaultView;
        }
        // Добавляет новую книгу в библиотеку.
        void Add_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, заполнены ли оба обязательных поля.
            if (string.IsNullOrWhiteSpace(TitleBox.Text) ||
                string.IsNullOrWhiteSpace(AuthorBox.Text))
            {
                MessageBox.Show("Введите название и автора книги.");
                return; // Прерываем добавление, если данные не введены.
            }
            using var c = new SqliteConnection(cs);
            c.Open();
            using var cmd = c.CreateCommand();
            // Новая книга получает статус «Доступна».
            // Название и автор передаются через параметры запроса.
            cmd.CommandText = @"INSERT INTO Books
                (Title, Author, Status)
                VALUES($t, $a, 'Доступна')";
            cmd.Parameters.AddWithValue("$t", TitleBox.Text);
            cmd.Parameters.AddWithValue("$a", AuthorBox.Text);
            cmd.ExecuteNonQuery();
            // Обновляем таблицу, сохраняя текущий поисковый запрос.
            LoadData(SearchBox.Text);
        }
        // Выполняет поиск книг по названию или автору.
        void Search_Click(object sender, RoutedEventArgs e)
        {
            LoadData(SearchBox.Text);
        }
        // Выдаёт выбранную книгу читателю, меняя её статус.
        void Rent_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбрана ли строка в таблице.
            // DataRowView позволяет получить данные выбранной книги.
            if (Grid.SelectedItem is not DataRowView r) return;
            // Нельзя выдать книгу, которая уже арендована.
            if (r["Status"].ToString() != "Доступна")
            {
                MessageBox.Show("Эта книга уже арендована.");
                return;
            }
            using var c = new SqliteConnection(cs);
            c.Open();
            using var cmd = c.CreateCommand();
            // Меняем статус только у книги с указанным Id, Id помогает изменить именно выбранную запись.
            cmd.CommandText =
                "UPDATE Books SET Status='Арендована' WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", r["Id"]);
            cmd.ExecuteNonQuery();
            // Показываем обновлённый статус книги в таблице.
            LoadData(SearchBox.Text);
        }
        // При выборе книги переносит её название и автора в поля ввода.
        void Grid_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            // Если строка не выбрана, ничего не делаем.
            if (Grid.SelectedItem is not DataRowView r) return;
            // Получаем значения из столбцов выбранной строки.
            TitleBox.Text = r["Title"].ToString();
            AuthorBox.Text = r["Author"].ToString();
        }
    }
}