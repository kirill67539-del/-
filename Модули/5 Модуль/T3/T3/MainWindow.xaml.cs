using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
namespace TasksApp
{
    public partial class MainWindow : Window
    {
        // Создаём коллекцию задач с автоматическим обновлением списка.
        private ObservableCollection<TaskItem> tasks =
            new ObservableCollection<TaskItem>();
        public MainWindow()
        {
            // Загружаем элементы интерфейса из файла MainWindow.xaml.
            InitializeComponent();
            // Связываем список в окне с коллекцией задач.
            TaskList.ItemsSource = tasks;
        }
        // Обработчик нажатия кнопки «Добавить».
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Получаем название задачи и удаляем пробелы по краям строки.
            string title = TaskInput.Text.Trim();
            // Проверяем, что пользователь ввёл непустое название задачи.
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Введите название задачи.");
                return;
            }
            // Создаём новую задачу и добавляем её в коллекцию.
            tasks.Add(new TaskItem { Title = title });
            // Очищаем поле ввода после добавления задачи.
            TaskInput.Clear();
            // Возвращаем курсор в поле ввода для следующей задачи.
            TaskInput.Focus();
        }
        // Обработчик нажатия кнопки «Удалить».
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбрана ли задача в списке.
            if (TaskList.SelectedItem is TaskItem selected)
            {
                // Удаляем выбранную задачу из коллекции.
                tasks.Remove(selected);
            }
            else
            {
                // Предупреждаем пользователя, если задача не выбрана.
                MessageBox.Show("Выберите задачу для удаления.");
            }
        }
    }
    // Класс описывает одну задачу в списке.
    public class TaskItem : INotifyPropertyChanged
    {
        // Хранит состояние выполнения задачи: true — выполнена, false — не выполнена.
        private bool isCompleted;
        // Название задачи, отображаемое в списке.
        public string Title { get; set; } = "";
        // Свойство для отметки выполнения задачи.
        public bool IsCompleted
        {
            // Возвращаем текущее состояние задачи.
            get => isCompleted;
            set
            {
                // Сохраняем новое состояние выполнения задачи.
                isCompleted = value;
                // Сообщаем интерфейсу об изменении свойства, чтобы он мог обновить отображение задачи.
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
        }
        // Событие уведомляет интерфейс об изменении свойств объекта.
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}