using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
namespace TextEditorApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Инициализируем элементы интерфейса из файла MainWindow.xaml.
            InitializeComponent();
        }
        // Обработчик нажатия кнопки «Открыть».
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Создаём диалоговое окно для выбора файла.
            OpenFileDialog dialog = new OpenFileDialog();
            // Настраиваем фильтр для текстовых файлов и всех остальных файлов.
            dialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            // Проверяем, выбрал ли пользователь файл и подтвердил ли открытие.
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Читаем содержимое выбранного файла и выводим его в редактор.
                    Editor.Text = File.ReadAllText(dialog.FileName);
                }
                catch (Exception ex)
                {
                    // Показываем сообщение, если файл не удалось открыть.
                    MessageBox.Show("Не удалось открыть файл:\n" + ex.Message);
                }
            }
        }
        // Обработчик нажатия кнопки «Сохранить».
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Создаём диалоговое окно для выбора места сохранения файла.
            SaveFileDialog dialog = new SaveFileDialog();
            // Настраиваем фильтр форматов для сохранения файла.
            dialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            // Указываем расширение .txt по умолчанию.
            dialog.DefaultExt = ".txt";
            // Проверяем, подтвердил ли пользователь сохранение файла.
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Записываем текст из редактора в выбранный файл.
                    File.WriteAllText(dialog.FileName, Editor.Text);
                    // Сообщаем пользователю об успешном сохранении.
                    MessageBox.Show("Файл успешно сохранён!");
                }
                catch (Exception ex)
                {
                    // Показываем сообщение, если при сохранении возникла ошибка.
                    MessageBox.Show("Не удалось сохранить файл:\n" + ex.Message);
                }
            }
        }
    }
}