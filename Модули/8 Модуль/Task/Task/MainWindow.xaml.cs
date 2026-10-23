using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
namespace InteractiveMap
{
    // Главный класс окна приложения.
    // Отвечает за отображение карты и взаимодействие с пользователем.
    public partial class MainWindow : Window
    {
        // Объект сервиса для управления данными карты.
        private readonly MapService _mapService = new();
        // Ссылка на выбранный или создаваемый объект карты.
        private MapLocation? _selectedLocation;
        // Флаг показывает, завершена ли начальная настройка интерфейса.
        private bool _ready;
        // Конструктор окна. Выполняется при запуске приложения.
        public MainWindow()
        {
            // Создание элементов интерфейса из файла XAML.
            InitializeComponent();
            // Добавление примеров объектов на карту.
            AddDemoLocations();
            // Разрешение обновления списка и фильтрации.
            _ready = true;
            // Первоначальное отображение объектов и карты.
            RefreshLocations();
            DrawMap();
        }
        // Создаёт несколько демонстрационных объектов, чтобы карта не была пустой при первом запуске.
        private void AddDemoLocations()
        {
            _mapService.Add(new MapLocation
            {
                Name = "Центральный парк",
                Category = LocationCategory.Park,
                Description = "Зелёная зона для прогулок.",
                X = 0.25,
                Y = 0.30
            });
            _mapService.Add(new MapLocation
            {
                Name = "Городское кафе",
                Category = LocationCategory.Cafe,
                Description = "Уютное кафе в центре города.",
                X = 0.57,
                Y = 0.42
            });

            _mapService.Add(new MapLocation
            {
                Name = "Исторический музей",
                Category = LocationCategory.Landmark,
                Description = "Музей исторических экспонатов.",
                X = 0.72,
                Y = 0.68
            });

            _mapService.Add(new MapLocation
            {
                Name = "Магазин",
                Category = LocationCategory.Shop,
                Description = "Магазин товаров повседневного спроса.",
                X = 0.38,
                Y = 0.75
            });
        }

        // Обновляет список объектов с учётом введённого поиска
        // и выбранной категории.
        private void RefreshLocations()
        {
            // Если интерфейс ещё настраивается, обновление не нужно.
            if (!_ready)
                return;

            // Получаем поисковый запрос и выбранную категорию.
            string search = SearchBox.Text.Trim();
            int category = CategoryFilter.SelectedIndex;

            // Отбираем объекты по названию и категории.
            LocationsList.ItemsSource = _mapService.Locations
                .Where(x => x.Name.Contains(
                    search, StringComparison.OrdinalIgnoreCase))
                .Where(x => category <= 0 ||
                    (int)x.Category == category - 1)
                .ToList();

            // Показываем количество найденных объектов
            // и общее количество объектов на карте.
            LocationsCountText.Text =
                $"Объектов: {LocationsList.Items.Count} из {_mapService.Locations.Count}";

            // Перерисовываем карту после обновления данных.
            DrawMap();
        }

        // Отрисовывает фон карты, координатную сетку,
        // условные улицы и маркеры объектов.
        private void DrawMap()
        {
            // Проверяем, что элемент карты существует.
            if (MapCanvas == null)
                return;

            // Удаляем прежнее изображение карты.
            MapCanvas.Children.Clear();

            // Получаем ширину и высоту области карты.
            double w = MapCanvas.ActualWidth;
            double h = MapCanvas.ActualHeight;

            // Не рисуем карту, если её размеры ещё не определены.
            if (w < 10 || h < 10)
                return;

            // Создаём вертикальные и горизонтальные линии сетки.
            for (int i = 1; i < 10; i++)
            {
                AddLine(w * i / 10, 0, w * i / 10, h,
                    Color.FromRgb(211, 224, 237), 1);

                AddLine(0, h * i / 10, w, h * i / 10,
                    Color.FromRgb(211, 224, 237), 1);
            }

            // Рисуем условные улицы белыми линиями.
            AddLine(0, h * 0.25, w, h * 0.40,
                Colors.White, 13);
            AddLine(w * 0.15, 0, w * 0.40, h,
                Colors.White, 13);
            AddLine(w * 0.72, 0, w * 0.60, h,
                Colors.White, 13);
            AddLine(0, h * 0.78, w, h * 0.65,
                Colors.White, 13);

            // Размещаем маркер для каждого сохранённого объекта.
            foreach (var location in _mapService.Locations)
                DrawMarker(location, w, h);
        }

        // Создаёт линию с заданными координатами, цветом и толщиной.
        // Используется для сетки и условных улиц.
        private void AddLine(
            double x1, double y1,
            double x2, double y2,
            Color color, double thickness)
        {
            MapCanvas.Children.Add(new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = new SolidColorBrush(color),
                StrokeThickness = thickness,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,

                // Линия не должна перехватывать нажатия мыши.
                IsHitTestVisible = false
            });
        }

        // Создаёт маркер объекта и размещает его на карте
        // в соответствии с относительными координатами X и Y.
        private void DrawMarker(MapLocation location, double w, double h)
        {
            var marker = new Ellipse
            {
                Width = 22,
                Height = 22,

                // Цвет маркера зависит от категории объекта.
                Fill = GetColor(location.Category),

                // Выбранный объект выделяется жёлтой обводкой.
                Stroke = _selectedLocation?.Id == location.Id
                    ? Brushes.Yellow : Brushes.White,

                StrokeThickness = _selectedLocation?.Id == location.Id
                    ? 4 : 2,

                Cursor = Cursors.Hand,
                ToolTip = location.Name,

                // Сохраняем ссылку на объект в маркере.
                Tag = location
            };

            // Подключаем обработчик нажатия на маркер.
            marker.MouseLeftButtonDown += Marker_Click;

            // Добавляем маркер на область карты.
            MapCanvas.Children.Add(marker);

            // Переводим относительные координаты в пиксели.
            Canvas.SetLeft(marker, location.X * w - 11);
            Canvas.SetTop(marker, location.Y * h - 11);
        }

        // Возвращает цвет маркера в зависимости от категории.
        private Brush GetColor(LocationCategory category)
        {
            string color = category switch
            {
                LocationCategory.Landmark => "#2563EB",
                LocationCategory.Shop => "#16A34A",
                LocationCategory.Cafe => "#EA580C",
                LocationCategory.Park => "#9333EA",
                _ => "#64748B"
            };

            // Преобразуем строковое представление цвета в кисть WPF.
            return new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(color));
        }

        // Обрабатывает нажатие на маркер объекта.
        private void Marker_Click(object sender, MouseButtonEventArgs e)
        {
            // Проверяем, что маркер содержит объект карты.
            if (sender is FrameworkElement element &&
                element.Tag is MapLocation location)
            {
                // Выбираем объект и показываем его данные в форме.
                SelectLocation(location);

                // Не передаём нажатие другим элементам интерфейса.
                e.Handled = true;
            }
        }

        // Выбирает объект и отображает его информацию в полях формы.
        private void SelectLocation(MapLocation location)
        {
            _selectedLocation = location;

            // Заполняем поля данными выбранного объекта.
            NameBox.Text = location.Name;
            DescriptionBox.Text = location.Description;
            CategoryBox.SelectedIndex = (int)location.Category;
            CoordinatesLabel.Text = location.CoordinatesText;

            // Снимаем прежнее выделение в списке.
            LocationsList.SelectedItem = null;

            // Если объект присутствует в отфильтрованном списке,
            // выделяем его там тоже.
            if (LocationsList.ItemsSource is
                System.Collections.Generic.IEnumerable<MapLocation> items)
            {
                LocationsList.SelectedItem =
                    items.FirstOrDefault(x => x.Id == location.Id);
            }

            // Перерисовываем карту, чтобы выделить маркер.
            DrawMap();
        }

        // Обрабатывает выбор объекта в списке слева.
        private void LocationsList_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            // Игнорируем события во время начальной настройки.
            if (!_ready)
                return;

            // Если выбран элемент списка, отображаем его данные.
            if (LocationsList.SelectedItem is MapLocation location)
                SelectLocation(location);
        }

        // Обрабатывает нажатие на свободную область карты.
        // Создаёт временную точку для добавления нового объекта.
        private void MapCanvas_MouseLeftButtonDown(
            object sender, MouseButtonEventArgs e)
        {
            // Не создаём точку, если нажатие было не по фону карты.
            if (e.OriginalSource != MapCanvas)
                return;

            // Получаем координаты нажатия мыши.
            Point point = e.GetPosition(MapCanvas);

            // Проверяем размеры карты.
            if (MapCanvas.ActualWidth <= 0 ||
                MapCanvas.ActualHeight <= 0)
                return;

            // Преобразуем координаты в диапазон от 0 до 1.
            _selectedLocation = new MapLocation
            {
                X = Math.Clamp(point.X / MapCanvas.ActualWidth, 0, 1),
                Y = Math.Clamp(point.Y / MapCanvas.ActualHeight, 0, 1)
            };

            // Очищаем форму для ввода нового объекта.
            NameBox.Clear();
            DescriptionBox.Clear();
            CategoryBox.SelectedIndex = 0;

            // Показываем координаты выбранной точки.
            CoordinatesLabel.Text = _selectedLocation.CoordinatesText;

            // Снимаем выделение существующего объекта в списке.
            LocationsList.SelectedItem = null;

            // Обновляем карту и переводим фокус в поле названия.
            DrawMap();
            NameBox.Focus();
        }

        // Повторно рисует карту при изменении размера окна.
        private void MapCanvas_SizeChanged(
            object sender, SizeChangedEventArgs e)
        {
            DrawMap();
        }

        // Обновляет список при изменении поискового запроса.
        private void SearchBox_TextChanged(
            object sender, TextChangedEventArgs e)
        {
            if (_ready)
                RefreshLocations();
        }

        // Обновляет список при выборе другой категории фильтра.
        private void CategoryFilter_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (_ready)
                RefreshLocations();
        }

        // Получает категорию, выбранную в форме объекта.
        private LocationCategory SelectedCategory()
        {
            int index = CategoryBox.SelectedIndex;

            // Если категория выбрана правильно,
            // преобразуем индекс в значение перечисления.
            return index >= 0 && index <= 4
                ? (LocationCategory)index
                : LocationCategory.Landmark;
        }

        // Добавляет новый объект на карту.
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Для добавления нужна новая точка на карте.
            // Если выбран существующий объект, повторно его не добавляем.
            if (_selectedLocation == null ||
                _mapService.Locations.Any(
                    x => x.Id == _selectedLocation.Id))
            {
                MessageBox.Show(
                    "Сначала выберите свободное место на карте.");
                return;
            }

            // Проверяем, что пользователь указал название.
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите название объекта.");
                return;
            }

            // Создаём объект с данными из формы и координатами точки.
            var location = new MapLocation
            {
                Name = NameBox.Text.Trim(),
                Category = SelectedCategory(),
                Description = DescriptionBox.Text.Trim(),
                X = _selectedLocation.X,
                Y = _selectedLocation.Y
            };

            // Сохраняем объект в сервисе.
            _mapService.Add(location);

            // Обновляем список и выделяем новый объект.
            RefreshLocations();
            SelectLocation(location);
        }

        // Сохраняет изменения выбранного объекта.
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли существующий объект.
            if (_selectedLocation == null ||
                !_mapService.Locations.Any(
                    x => x.Id == _selectedLocation.Id))
            {
                MessageBox.Show("Выберите существующий объект.");
                return;
            }

            // Название объекта не должно быть пустым.
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите название объекта.");
                return;
            }

            // Переносим изменённые данные из формы в объект.
            _selectedLocation.Name = NameBox.Text.Trim();
            _selectedLocation.Category = SelectedCategory();
            _selectedLocation.Description = DescriptionBox.Text.Trim();

            // Сохраняем ссылку, обновляем список и выделение.
            var updated = _selectedLocation;
            RefreshLocations();
            SelectLocation(updated);
        }

        // Удаляет выбранный объект после подтверждения пользователя.
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что выбранный объект существует.
            if (_selectedLocation == null ||
                !_mapService.Locations.Any(
                    x => x.Id == _selectedLocation.Id))
            {
                MessageBox.Show("Выберите объект для удаления.");
                return;
            }

            // Запрашиваем подтверждение удаления.
            if (MessageBox.Show(
                $"Удалить объект «{_selectedLocation.Name}»?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            // Удаляем объект и очищаем форму.
            _mapService.Remove(_selectedLocation);
            _selectedLocation = null;
            ClearFields();

            // Обновляем список и карту.
            RefreshLocations();
        }

        // Сбрасывает выбор объекта и очищает поля формы.
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedLocation = null;
            LocationsList.SelectedItem = null;

            ClearFields();
            DrawMap();
        }

        // Очищает поля названия, описания, категории и координат.
        private void ClearFields()
        {
            NameBox.Clear();
            DescriptionBox.Clear();
            CategoryBox.SelectedIndex = 0;
            CoordinatesLabel.Text = "Не выбрано";
        }

        // Сохраняет все объекты карты в JSON-файл.
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Открываем диалог выбора имени и расположения файла.
            var dialog = new SaveFileDialog
            {
                Filter = "JSON-файл (*.json)|*.json",
                FileName = "interactive-map.json"
            };

            // Если пользователь отменил сохранение, завершаем метод.
            if (dialog.ShowDialog() != true)
                return;

            try
            {
                // Передаём путь к файлу сервису.
                _mapService.Save(dialog.FileName);
                MessageBox.Show("Карта успешно сохранена.");
            }
            catch (Exception ex)
            {
                // Показываем сообщение, если сохранить файл не удалось.
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        // Загружает объекты карты из ранее сохранённого JSON-файла.
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // Открываем диалог выбора JSON-файла.
            var dialog = new OpenFileDialog
            {
                Filter = "JSON-файл (*.json)|*.json"
            };

            // Если пользователь отменил загрузку, ничего не меняем.
            if (dialog.ShowDialog() != true)
                return;

            try
            {
                // Загружаем данные в сервис.
                _mapService.Load(dialog.FileName);

                // Сбрасываем прежний выбор и очищаем форму.
                _selectedLocation = null;
                ClearFields();

                // Обновляем список и перерисовываем карту.
                RefreshLocations();

                MessageBox.Show("Карта успешно загружена.");
            }
            catch (Exception ex)
            {
                // Выводим ошибку при повреждённом файле
                // или проблеме чтения данных.
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }
    }
}