using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
namespace DrawingApp
{
    public partial class MainWindow : Window
    {
        // Хранит название выбранного инструмента рисования.
        private string selectedTool = "Линия";
        // Координаты точки, в которой пользователь начал рисовать.
        private Point startPoint;
        // Ссылка на фигуру, которую пользователь рисует в данный момент.
        private Shape previewShape;
        // Показывает, находится ли процесс рисования в активном состоянии.
        private bool isDrawing;
        public MainWindow()
        {
            // Загружает элементы интерфейса, описанные в файле MainWindow.xaml.
            InitializeComponent();
        }
        // Обработчик нажатия кнопки «Линия».
        private void Line_Click(object sender, RoutedEventArgs e)
        {
            selectedTool = "Линия";
        }
        // Обработчик нажатия кнопки «Круг».
        private void Circle_Click(object sender, RoutedEventArgs e)
        {
            selectedTool = "Круг";
        }
        // Обработчик нажатия кнопки «Квадрат».
        private void Square_Click(object sender, RoutedEventArgs e)
        {
            selectedTool = "Квадрат";
        }
        // Обработчик нажатия кнопки «Очистить».
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            // Удаляем все фигуры с холста.
            DrawingCanvas.Children.Clear();
            // Убираем ссылку на текущую фигуру.
            previewShape = null;
            // Завершаем рисование, если оно было начато.
            isDrawing = false;
        }
        // Вызывается, когда пользователь нажимает кнопку мыши на холсте.
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Получаем координаты начала линии или одним из углов фигуры.
            startPoint = e.GetPosition(DrawingCanvas);
            // Отмечаем, что пользователь начал рисовать.
            isDrawing = true;
            // Создаём фигуру в соответствии с выбранным инструментом.
            if (selectedTool == "Линия")
            {
                // Line хранит координаты начала и конца отрезка.
                previewShape = new Line();
            }
            else if (selectedTool == "Круг")
            {
                // Ellipse позволяет рисовать круги и другие эллипсы.
                previewShape = new Ellipse();
            }
            else
            {
                // Rectangle используется для создания квадратов и прямоугольников.
                previewShape = new Rectangle();
            }
            // Устанавливаем цвет контура фигуры.
            previewShape.Stroke = Brushes.DarkBlue;
            // Задаём толщину линии контура.
            previewShape.StrokeThickness = 2;
            // Добавляем фигуру на холст.
            DrawingCanvas.Children.Add(previewShape);
            // Захватываем мышь
            DrawingCanvas.CaptureMouse();
            // Сразу обновляем координаты фигуры.
            UpdateShape(e.GetPosition(DrawingCanvas));
        }
        // Вызывается при перемещении мыши.
        // Позволяет изменять фигуру прямо во время движения курсора.
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Обновляем фигуру только тогда, когда рисование начато
            if (isDrawing && previewShape != null)
            {
                // Передаём текущие координаты мыши в метод изменения фигуры.
                UpdateShape(e.GetPosition(DrawingCanvas));
            }
        }
        // Вызывается, когда пользователь отпускает кнопку мыши, завершается рисование.
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Если рисование не было начато, ничего не делаем.
            if (!isDrawing)
                return;
            // Обновляем фигуру по последнему положению курсора.
            UpdateShape(e.GetPosition(DrawingCanvas));
            // Сообщаем программе, что рисование завершено.
            isDrawing = false;
            // Освобождаем захват мыши, можно выбрать что рисуем
            DrawingCanvas.ReleaseMouseCapture();
            // Убираем ссылку на временную фигуру, фигура остаётся на холсте.
            previewShape = null;
        }
        // Изменяет координаты и размеры фигуры в зависимости от положения мыши.
        // startPoint — начало, current - конец
        private void UpdateShape(Point current)
        {
            // Если создаваемая фигура является линией задаём её начальную и конечную точки.
            if (previewShape is Line line)
            {
                // Координаты начала линии.
                line.X1 = startPoint.X;
                line.Y1 = startPoint.Y;
                // Координаты конца линии — текущее положение мыши.
                line.X2 = current.X;
                line.Y2 = current.Y;
            }
            else
            {
                // Вычисляем размер фигуры по горизонтали и вертикали.
                // Выбираем большее расстояние, чтобы ширина и высота были одинаковыми.
                double size = Math.Max(
                    Math.Abs(current.X - startPoint.X),
                    Math.Abs(current.Y - startPoint.Y));
                // Определяем левую координату фигуры с учётом направления движения мыши.
                double x = current.X < startPoint.X
                    ? startPoint.X - size
                    : startPoint.X;
                // Определяем верхнюю координату фигуры с учётом направления движения мыши.
                double y = current.Y < startPoint.Y
                    ? startPoint.Y - size
                    : startPoint.Y;
                // Устанавливаем положение фигуры на холсте.
                Canvas.SetLeft(previewShape, x);
                Canvas.SetTop(previewShape, y);
                // Задаём одинаковые ширину и высоту для создания квадрата или круга.
                previewShape.Width = size;
                previewShape.Height = size;
            }
        }
    }
}