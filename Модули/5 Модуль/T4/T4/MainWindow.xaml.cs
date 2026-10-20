using System; // Подключаем основные классы C#
using System.Windows; // Подключаем элементы WPF
using System.Windows.Media.Imaging; // Для работы с изображениями
using Microsoft.Win32; // Для открытия диалогового окна выбора файла
namespace ImageViewerApp
{
    public partial class MainWindow : Window
    {
        private double zoom = 1.0; // Текущий масштаб изображения
        public MainWindow()
        {
            InitializeComponent(); // Загружаем интерфейс окна
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog(); // Создаём окно выбора файла
            dialog.Filter =
                "Изображения|*.png;*.jpg;*.jpeg;*.bmp;*.gif"; // Разрешённые форматы
            if (dialog.ShowDialog() == true) // Если файл выбран
            {
                try
                {
                    BitmapImage image = new BitmapImage(); // Создаём объект изображения
                    image.BeginInit(); // Начинаем настройку загрузки
                    image.UriSource = new Uri(dialog.FileName); // Указываем путь к файлу
                    image.CacheOption = BitmapCacheOption.OnLoad; // Загружаем изображение в память
                    image.EndInit(); // Завершаем загрузку изображения
                    Picture.Source = image; // Показываем изображение в окне
                    zoom = 1.0; // Сбрасываем масштаб
                    UpdateZoom(); // Обновляем отображение масштаба
                }
                catch (Exception ex) // Если произошла ошибка
                {
                    MessageBox.Show(
                        "Не удалось открыть изображение:\n" + ex.Message); // Показываем сообщение об ошибке
                }
            }
        }
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            zoom = Math.Min(zoom * 1.2, 5.0); // Увеличиваем масштаб, максимум — 5
            UpdateZoom(); // Применяем новый масштаб
        }
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            zoom = Math.Max(zoom / 1.2, 0.2); // Уменьшаем масштаб, минимум — 0,2
            UpdateZoom(); // Применяем новый масштаб
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            zoom = 1.0; // Возвращаем исходный масштаб
            UpdateZoom(); // Обновляем изображение
        }
        private void UpdateZoom()
        {
            ImageScale.ScaleX = zoom; // Масштаб по горизонтали
            ImageScale.ScaleY = zoom; // Масштаб по вертикали
        }
    }
}