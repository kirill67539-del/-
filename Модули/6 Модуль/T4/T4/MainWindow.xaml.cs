using Microsoft.Data.Sqlite; // Работа с базой данных SQLite
using System;
using System.Data; // Таблицы данных
using System.Windows; // Элементы WPF
using System.Windows.Controls; // Кнопки и списки WPF
namespace FinanceApp
{
    public partial class MainWindow : Window
    {
        string cs = "Data Source=finance.db"; // Файл базы данных
        public MainWindow()
        {
            InitializeComponent(); // Загружаем интерфейс
            using var c = new SqliteConnection(cs);
            c.Open(); // Открываем базу данных
            using var cmd = c.CreateCommand();
            // Создаём таблицу доходов и расходов.
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Finance(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Type TEXT, Description TEXT, Amount REAL)";
            cmd.ExecuteNonQuery();
            LoadData(); // Загружаем записи
        }
        // Загружает финансовые записи в таблицу.
        void LoadData()
        {
            using var c = new SqliteConnection(cs);
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = "SELECT * FROM Finance"; // Получаем все записи
            using var reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader); // Заполняем таблицу данными
            Grid.ItemsSource = dt.DefaultView; // Показываем записи в интерфейсе
        }
        // Возвращает выбранный тип операции.
        string GetTypeValue() =>
            (TypeBox.SelectedItem as ComboBoxItem)?.Content?.ToString()
            ?? "Доход";
        // Проверяет правильность введённой суммы.
        bool TryAmount(out double amount)
        {
            if (double.TryParse(AmountBox.Text, out amount) &&
                amount >= 0)
                return true;
            MessageBox.Show("Введите корректную сумму.");
            return false;
        }
        // Добавляет новую финансовую запись.
        void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!TryAmount(out double amount)) return;
            using var c = new SqliteConnection(cs);
            c.Open();
            using var cmd = c.CreateCommand();
            // Добавляем тип, описание и сумму.
            cmd.CommandText = @"INSERT INTO Finance
                (Type, Description, Amount)
                VALUES($t, $d, $a)";
            cmd.Parameters.AddWithValue("$t", GetTypeValue());
            cmd.Parameters.AddWithValue("$d", DescriptionBox.Text);
            cmd.Parameters.AddWithValue("$a", amount);
            cmd.ExecuteNonQuery(); // Сохраняем запись
            LoadData(); // Обновляем таблицу
        }
        // Изменяет выбранную запись.
        void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem is not DataRowView r) return;
            if (!TryAmount(out double amount)) return;
            using var c = new SqliteConnection(cs);
            c.Open();
            using var cmd = c.CreateCommand();
            // Обновляем запись с указанным Id.
            cmd.CommandText = @"UPDATE Finance SET
                Type=$t, Description=$d, Amount=$a WHERE Id=$id";
            cmd.Parameters.AddWithValue("$t", GetTypeValue());
            cmd.Parameters.AddWithValue("$d", DescriptionBox.Text);
            cmd.Parameters.AddWithValue("$a", amount);
            cmd.Parameters.AddWithValue("$id", r["Id"]);
            cmd.ExecuteNonQuery(); // Сохраняем изменения
            LoadData(); // Обновляем таблицу
        }
        // Переносит выбранную запись в поля ввода.
        void Grid_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            if (Grid.SelectedItem is not DataRowView r) return;
            DescriptionBox.Text = r["Description"].ToString();
            AmountBox.Text = r["Amount"].ToString();
            // Выбираем тип операции из списка.
            foreach (ComboBoxItem item in TypeBox.Items)
                if (item.Content.ToString() == r["Type"].ToString())
                    TypeBox.SelectedItem = item;
        }
    }
}