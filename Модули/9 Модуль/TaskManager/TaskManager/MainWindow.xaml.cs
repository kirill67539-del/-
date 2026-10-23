using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TaskManager;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<TaskItem> tasks = new();

    private readonly HashSet<string> notifiedTasks = new();

    private readonly string filePath = "tasks.json";

    private readonly DispatcherTimer timer = new();

    public MainWindow()
    {
        InitializeComponent();

        TasksGrid.ItemsSource = tasks;
        DueDatePicker.SelectedDate = DateTime.Today;

        LoadTasks();

        // Проверяем сроки выполнения каждую минуту.
        timer.Interval = TimeSpan.FromMinutes(1);
        timer.Tick += CheckDeadlines;
        timer.Start();

        CheckDeadlines(this, EventArgs.Empty);
    }

    // Добавляем новую задачу.
    private void AddTask(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleBox.Text))
        {
            MessageBox.Show("Введите название задачи.");
            return;
        }

        if (!int.TryParse(ProgressBox.Text, out int progress)
            || progress < 0 || progress > 100)
        {
            MessageBox.Show("Прогресс должен быть от 0 до 100.");
            return;
        }

        tasks.Add(new TaskItem
        {
            Title = TitleBox.Text,
            Priority = GetSelected(PriorityBox),
            DueDate = DueDatePicker.SelectedDate ?? DateTime.Today,
            Status = GetSelected(StatusBox),
            Progress = progress
        });

        SaveTasks();
        ClearFields();
    }

    // Загружаем данные выбранной задачи в поля.
    private void SelectTask(
        object sender, SelectionChangedEventArgs e)
    {
        if (TasksGrid.SelectedItem is not TaskItem task)
            return;

        TitleBox.Text = task.Title;
        ProgressBox.Text = task.Progress.ToString();
        DueDatePicker.SelectedDate = task.DueDate;

        SelectCombo(PriorityBox, task.Priority);
        SelectCombo(StatusBox, task.Status);
    }

    // Изменяем выбранную задачу.
    private void EditTask(object sender, RoutedEventArgs e)
    {
        if (TasksGrid.SelectedItem is not TaskItem task)
            return;

        if (string.IsNullOrWhiteSpace(TitleBox.Text))
        {
            MessageBox.Show("Введите название задачи.");
            return;
        }

        if (!int.TryParse(ProgressBox.Text, out int progress)
            || progress < 0 || progress > 100)
        {
            MessageBox.Show("Прогресс должен быть от 0 до 100.");
            return;
        }

        task.Title = TitleBox.Text;
        task.Priority = GetSelected(PriorityBox);
        task.DueDate = DueDatePicker.SelectedDate ?? DateTime.Today;
        task.Status = GetSelected(StatusBox);
        task.Progress = progress;

        TasksGrid.Items.Refresh();
        SaveTasks();
    }

    // Удаляем выбранную задачу.
    private void DeleteTask(object sender, RoutedEventArgs e)
    {
        if (TasksGrid.SelectedItem is not TaskItem task)
            return;

        tasks.Remove(task);
        SaveTasks();
        ClearFields();
    }

    private string GetSelected(ComboBox combo)
    {
        return (combo.SelectedItem as ComboBoxItem)?
            .Content?.ToString() ?? "";
    }

    private void SelectCombo(ComboBox combo, string value)
    {
        foreach (ComboBoxItem item in combo.Items)
        {
            if (item.Content?.ToString() == value)
            {
                combo.SelectedItem = item;
                break;
            }
        }
    }

    // Очищаем поля ввода.
    private void ClearFields()
    {
        TitleBox.Clear();
        ProgressBox.Text = "0";
        DueDatePicker.SelectedDate = DateTime.Today;
        PriorityBox.SelectedIndex = 1;
        StatusBox.SelectedIndex = 0;
        TasksGrid.SelectedItem = null;
    }

    // Сохраняем задачи в JSON-файл.
    private void SaveTasks()
    {
        File.WriteAllText(
            filePath,
            JsonSerializer.Serialize(tasks.ToList()));
    }

    // Загружаем задачи при запуске программы.
    private void LoadTasks()
    {
        if (!File.Exists(filePath))
            return;

        try
        {
            var saved = JsonSerializer.Deserialize<List<TaskItem>>(
                File.ReadAllText(filePath));

            if (saved == null)
                return;

            foreach (var task in saved)
                tasks.Add(task);
        }
        catch (JsonException)
        {
            MessageBox.Show("Не удалось прочитать файл задач.");
        }
    }

    // Напоминаем о задачах со сроком сегодня или завтра.
    private void CheckDeadlines(object? sender, EventArgs e)
    {
        foreach (var task in tasks)
        {
            if (task.Status == "Завершена")
                continue;

            double daysLeft =
                (task.DueDate.Date - DateTime.Today).TotalDays;

            string key = task.Title + task.DueDate;

            if (daysLeft >= 0 && daysLeft <= 1
                && notifiedTasks.Add(key))
            {
                MessageBox.Show(
                    $"Скоро срок выполнения задачи: {task.Title}",
                    "Напоминание");
            }
        }
    }
}