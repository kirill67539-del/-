using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PlayfairCipherCSharp;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // Инициализация WinForms-приложения и запуск главной формы.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}

internal sealed class MainForm : Form
{
    // Алфавит без J: в шифре Плейфера J объединяется с I, поэтому в матрице 25 ячеек.
    private const string Alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";

    // Элементы интерфейса, создаваемые программно (без дизайнера).
    private readonly TextBox keyBox = new();
    private readonly TextBox inputBox = new();
    private readonly TextBox resultBox = new();
    private readonly DataGridView matrixGrid = new();
    private readonly DataGridView detailsGrid = new();
    private readonly Label statusLabel = new();

    // Матрица шифрования 5×5 и массив позиций: positions[индекс буквы] = (строка, столбец).
    // Индексы — это код буквы минус 'A'; для J хранится копия позиции I.
    private char[,] matrix = new char[5, 5];
    private readonly int[,] positions = new int[26, 2];

    public MainForm()
    {
        // Базовые настройки окна: заголовок, размеры, шрифт, старт по центру экрана.
        Text = "Шифр Плейфера";
        MinimumSize = new Size(900, 620);
        ClientSize = new Size(1180, 820);
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 10);

        BuildUi();

        // Пустой ключ => матрица строится по одному алфавиту (без ключевого слова).
        BuildMatrix("");
        keyBox.Text = "";
        statusLabel.Text = "Введите ключ и текст, затем выберите операцию.";
    }

    private void BuildUi()
    {
        // Главное меню: «Файл» (открыть/сохранить/очистить/выход) и «Справка».
        var menu = new MenuStrip { Dock = DockStyle.Top };
        var fileMenu = new ToolStripMenuItem("Файл");
        fileMenu.DropDownItems.Add("Открыть текст…", null, (_, _) => OpenText());
        fileMenu.DropDownItems.Add("Сохранить результат…", null, (_, _) => SaveText());
        fileMenu.DropDownItems.Add(new ToolStripSeparator());
        fileMenu.DropDownItems.Add("Очистить", null, (_, _) => ClearAll());
        fileMenu.DropDownItems.Add(new ToolStripSeparator());
        fileMenu.DropDownItems.Add("Выход", null, (_, _) => Close());
        var helpMenu = new ToolStripMenuItem("Справка");
        helpMenu.DropDownItems.Add("Инструкция", null, (_, _) => ShowHelp());
        helpMenu.DropDownItems.Add("О программе", null, (_, _) =>
            MessageBox.Show("Шифр Плейфера\nРеализация на C# / Windows Forms.", "О программе",
                MessageBoxButtons.OK, MessageBoxIcon.Information));
        menu.Items.Add(fileMenu);
        menu.Items.Add(helpMenu);
        MainMenuStrip = menu;
        Controls.Add(menu);

        // Корневая сетка: 2 столбца (текст/матрица слева-справа) и 4 строки
        // (ключ, исходный текст+матрица, результат+детали, статус).
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(10, 18, 10, 10),
            BackColor = Color.FromArgb(242, 245, 249),
            Margin = Padding.Empty
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 43));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 57));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        Controls.Add(root);

        // Панель ключа. Поле ключа приводится к верхнему регистру и ограничено 26 символами.
        // При изменении ключа (если введены только A–Z) матрица перестраивается «на лету».
        var keyPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(2, 8, 2, 2),
            Margin = Padding.Empty,
            BackColor = Color.FromArgb(242, 245, 249)
        };
        keyPanel.Controls.Add(MakeLabel("Ключевое слово:"));
        keyBox.Width = 220;
        keyBox.Height = 30;
        keyBox.CharacterCasing = CharacterCasing.Upper;
        keyBox.MaxLength = 26;
        keyBox.TabIndex = 0;
        keyBox.TextChanged += (_, _) =>
        {
            // Перестраиваем матрицу только когда ключ корректен (буквы A–Z).
            if (keyBox.Text.Length > 0 && keyBox.Text.All(c => c is >= 'A' and <= 'Z'))
                BuildMatrix(keyBox.Text);
        };
        keyPanel.Controls.Add(keyBox);
        root.Controls.Add(keyPanel, 0, 0);
        root.SetColumnSpan(keyPanel, 2);

        // Левая верхняя область — ввод исходного текста.
        var inputPanel = MakePanel("Исходный текст");
        ConfigureTextBox(inputBox, true);
        inputBox.TabIndex = 1;
        inputPanel.Controls.Add(inputBox);
        inputBox.BringToFront();
        root.Controls.Add(inputPanel, 0, 1);

        // Правая верхняя область — таблица шифрования 5×5.
        var matrixPanel = MakePanel("Таблица шифрования 5 × 5");
        ConfigureMatrixGrid();
        matrixPanel.Controls.Add(matrixGrid);
        matrixGrid.BringToFront();
        root.Controls.Add(matrixPanel, 1, 1);

        // Левая нижняя область — результат (только для чтения).
        var resultPanel = MakePanel("Результат");
        ConfigureTextBox(resultBox, true);
        resultBox.ReadOnly = true;
        resultBox.TabStop = false;
        resultPanel.Controls.Add(resultBox);
        resultBox.BringToFront();
        root.Controls.Add(resultPanel, 0, 2);

        // Правая нижняя область: кнопки операций + пошаговые детали.
        var rightPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 1,
            Margin = new Padding(4),
            Padding = Padding.Empty
        };
        rightPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
        rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = true, AutoScroll = true, Margin = Padding.Empty };
        buttons.Controls.Add(MakeButton("Зашифровать", (_, _) => Process(true)));
        buttons.Controls.Add(MakeButton("Расшифровать", (_, _) => Process(false)));
        buttons.Controls.Add(MakeButton("Очистить", (_, _) => ClearAll()));
        rightPanel.Controls.Add(buttons, 0, 0);

        ConfigureDetailsGrid();
        rightPanel.Controls.Add(detailsGrid, 0, 1);
        root.Controls.Add(rightPanel, 1, 2);

        // Строка статуса внизу.
        statusLabel.ForeColor = Color.FromArgb(45, 55, 72);
        statusLabel.Dock = DockStyle.Fill;
        statusLabel.Margin = new Padding(4, 4, 4, 0);
        root.Controls.Add(statusLabel, 0, 3);
        root.SetColumnSpan(statusLabel, 2);
        menu.BringToFront();
    }

    // Общая настройка многострочных текстовых полей (Consolas удобен для шифротекста).
    private static void ConfigureTextBox(TextBox box, bool multiline)
    {
        box.Multiline = multiline;
        box.ScrollBars = ScrollBars.Vertical;
        box.Dock = DockStyle.Fill;
        box.AcceptsReturn = true;
        box.AcceptsTab = true;
        box.WordWrap = true;
        box.Font = new Font("Consolas", 11);
        box.BackColor = Color.White;
        box.ForeColor = Color.FromArgb(25, 35, 50);
        box.BorderStyle = BorderStyle.FixedSingle;
        box.Margin = new Padding(8, 30, 8, 8);
        box.ReadOnly = false;
        box.Enabled = true;
    }

    // Настройка сетки матрицы: 5×5, без заголовков, только чтение, крупный шрифт.
    private void ConfigureMatrixGrid()
    {
        matrixGrid.Dock = DockStyle.Fill;
        matrixGrid.ReadOnly = true;
        matrixGrid.AllowUserToAddRows = false;
        matrixGrid.AllowUserToDeleteRows = false;
        matrixGrid.AllowUserToResizeRows = false;
        matrixGrid.AllowUserToResizeColumns = false;
        matrixGrid.RowHeadersVisible = false;
        matrixGrid.ColumnHeadersVisible = false;
        matrixGrid.ScrollBars = ScrollBars.None;
        matrixGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        matrixGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        matrixGrid.RowTemplate.Height = 48;
        matrixGrid.BackgroundColor = Color.White;
        matrixGrid.BorderStyle = BorderStyle.FixedSingle;
        matrixGrid.Margin = new Padding(8, 30, 8, 8);
        matrixGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        matrixGrid.DefaultCellStyle.Font = new Font("Consolas", 16, FontStyle.Bold);
        matrixGrid.DefaultCellStyle.BackColor = Color.White;
        matrixGrid.DefaultCellStyle.ForeColor = Color.FromArgb(25, 35, 50);
        matrixGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 225, 250);
        matrixGrid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 55, 90);
        for (int i = 0; i < 5; i++) matrixGrid.Columns.Add("c" + i, "");
        matrixGrid.Rows.Add(5);
        // При изменении размера панели ячейки пересчитываются, чтобы матрица оставалась квадратной.
        matrixGrid.Resize += (_, _) => ResizeMatrixCells();
        ResizeMatrixCells();
    }

    // Подгоняем размер ячеек так, чтобы 5×5 занимало доступную область и было квадратным.
    private void ResizeMatrixCells()
    {
        if (matrixGrid.Columns.Count != 5 || matrixGrid.Rows.Count != 5) return;

        int availableWidth = Math.Max(100, matrixGrid.ClientSize.Width - 2);
        int availableHeight = Math.Max(100, matrixGrid.ClientSize.Height - 2);
        int cellSize = Math.Max(20, Math.Min(availableWidth / 5, availableHeight / 5));

        for (int i = 0; i < 5; i++)
        {
            matrixGrid.Columns[i].Width = cellSize;
            matrixGrid.Rows[i].Height = cellSize;
        }
    }

    // Настройка таблицы пошагового анализа: № пары, пара, координаты, правило, результат.
    private void ConfigureDetailsGrid()
    {
        detailsGrid.Dock = DockStyle.Fill;
        detailsGrid.ReadOnly = true;
        detailsGrid.AllowUserToAddRows = false;
        detailsGrid.AllowUserToDeleteRows = false;
        detailsGrid.AllowUserToResizeRows = false;
        detailsGrid.RowHeadersVisible = false;
        detailsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        detailsGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        detailsGrid.BackgroundColor = Color.White;
        detailsGrid.BorderStyle = BorderStyle.FixedSingle;
        detailsGrid.DefaultCellStyle.BackColor = Color.White;
        detailsGrid.DefaultCellStyle.ForeColor = Color.FromArgb(25, 35, 50);
        detailsGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 225, 250);
        detailsGrid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 55, 90);
        detailsGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 232, 242);
        detailsGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(20, 45, 70);
        detailsGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        detailsGrid.EnableHeadersVisualStyles = false;
        detailsGrid.Columns.Add("n", "№");
        detailsGrid.Columns.Add("pair", "Пара");
        detailsGrid.Columns.Add("coords", "Координаты");
        detailsGrid.Columns.Add("rule", "Правило");
        detailsGrid.Columns.Add("out", "Результат");
        detailsGrid.Columns[0].FillWeight = 35;
        detailsGrid.Columns[1].FillWeight = 45;
        detailsGrid.Columns[2].FillWeight = 100;
        detailsGrid.Columns[3].FillWeight = 150;
        detailsGrid.Columns[4].FillWeight = 65;
    }

    // Создаёт панель с заголовком (используется для блоков «Исходный текст», «Результат» и т.п.).
    private static Panel MakePanel(string title)
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
            BackColor = Color.White,
            Margin = new Padding(4)
        };
        var label = new Label
        {
            Text = title,
            Dock = DockStyle.Top,
            Height = 26,
            ForeColor = Color.FromArgb(20, 91, 140),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.White
        };
        panel.Controls.Add(label);
        return panel;
    }

    private static Label MakeLabel(string text) => new() { Text = text, AutoSize = true, ForeColor = Color.FromArgb(25, 45, 70), Margin = new Padding(4, 7, 8, 0) };

    private static Button MakeButton(string text, EventHandler handler)
    {
        var b = new Button
        {
            Text = text,
            AutoSize = true,
            Height = 32,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(35, 105, 165),
            ForeColor = Color.White,
            Margin = new Padding(4)
        };
        b.FlatAppearance.BorderSize = 0;
        b.Click += handler;
        return b;
    }

    // Перестраивает матрицу по текущему ключу (с предварительной проверкой ключа).
    private void RebuildMatrix()
    {
        if (!ValidateKey()) return;
        BuildMatrix(keyBox.Text);
        statusLabel.Text = "Таблица построена по введённому ключу.";
    }

    // Проверка ключа: только латинские буквы A–Z, не пустой. Приводит к верхнему регистру.
    private bool ValidateKey()
    {
        string key = keyBox.Text.Trim().ToUpperInvariant();
        if (key.Length == 0 || key.Any(c => c < 'A' || c > 'Z'))
        {
            MessageBox.Show("Введите ключ из латинских букв A–Z без пробелов и знаков.", "Некорректный ключ",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        keyBox.Text = key;
        return true;
    }

    // Строит матрицу 5×5: сначала уникальные символы ключа, затем остаток алфавита.
    // J заменяется на I; повторяющиеся символы пропускаются.
    private void BuildMatrix(string key)
    {
        var letters = new StringBuilder();
        foreach (char raw in (key + Alphabet))
        {
            char c = char.ToUpperInvariant(raw);
            if (c == 'J') c = 'I';
            if (c >= 'A' && c <= 'Z' && !letters.ToString().Contains(c)) letters.Append(c);
        }
        string chars = letters.ToString();

        // Сбрасываем позиции и заполняем матрицу + массив координат.
        for (int i = 0; i < 26; i++) { positions[i, 0] = -1; positions[i, 1] = -1; }
        for (int i = 0; i < 25; i++)
        {
            int r = i / 5, c = i % 5;
            matrix[r, c] = chars[i];
            positions[matrix[r, c] - 'A', 0] = r;
            positions[matrix[r, c] - 'A', 1] = c;
        }
        // Для J дублируем позицию I — это позволяет единообразно искать обе буквы.
        positions['J' - 'A', 0] = positions['I' - 'A', 0];
        positions['J' - 'A', 1] = positions['I' - 'A', 1];

        // Отображаем матрицу в DataGridView (если она уже создана).
        if (matrixGrid.Columns.Count == 5 && matrixGrid.Rows.Count == 5)
            for (int r = 0; r < 5; r++)
                for (int c = 0; c < 5; c++) matrixGrid[c, r].Value = matrix[r, c].ToString();
    }

    // Подготовка текста:
    //  - шифрование: очистка (A–Z, J→I), разбиение на пары, вставка X между одинаковыми
    //    соседними буквами и дополнение X при нечётной длине;
    //  - расшифрование: только очистка (пары уже должны быть корректными).
    private string PrepareText(string text, bool decrypt)
    {
        var clean = new StringBuilder();
        foreach (char raw in text.ToUpperInvariant())
        {
            if (raw is >= 'A' and <= 'Z') clean.Append(raw == 'J' ? 'I' : raw);
        }
        if (decrypt) return clean.ToString();

        var prepared = new StringBuilder();
        for (int i = 0; i < clean.Length; i++)
        {
            char a = clean[i];
            char b = i + 1 < clean.Length ? clean[i + 1] : '\0';
            prepared.Append(a);
            if (b == '\0') prepared.Append('X');       // нечётный «хвост» — добавляем X
            else if (a == b) prepared.Append('X');     // одинаковые соседние — разделяем X
            else { prepared.Append(b); i++; }          // обычная пара — берём обе буквы
        }
        if (prepared.Length % 2 != 0) prepared.Append('X');
        return prepared.ToString();
    }

    // Возвращает координаты буквы в матрице; J трактуется как I.
    private (int row, int col) Find(char ch)
    {
        if (ch == 'J') ch = 'I';
        return (positions[ch - 'A', 0], positions[ch - 'A', 1]);
    }

    // Преобразование одной пары по правилам Плейфера.
    // coords — строка с координатами для таблицы деталей, rule — описание применённого правила.
    private string TransformPair(char a, char b, bool encrypt, out string coords, out string rule)
    {
        var p1 = Find(a); var p2 = Find(b);
        coords = $"{a}:({p1.row + 1},{p1.col + 1}); {b}:({p2.row + 1},{p2.col + 1})";
        int r1 = p1.row, c1 = p1.col, r2 = p2.row, c2 = p2.col;

        // Правило 1: буквы в одной строке — сдвиг по столбцам (вправо при шифровании, влево при расшифровании).
        if (r1 == r2)
        {
            rule = encrypt ? "Одна строка: сдвиг вправо" : "Одна строка: сдвиг влево";
            int shift = encrypt ? 1 : 4; // 4 == -1 по модулю 5
            return $"{matrix[r1, (c1 + shift) % 5]}{matrix[r2, (c2 + shift) % 5]}";
        }

        // Правило 2: буквы в одном столбце — сдвиг по строкам (вниз при шифровании, вверх при расшифровании).
        if (c1 == c2)
        {
            rule = encrypt ? "Один столбец: сдвиг вниз" : "Один столбец: сдвиг вверх";
            int shift = encrypt ? 1 : 4;
            return $"{matrix[(r1 + shift) % 5, c1]}{matrix[(r2 + shift) % 5, c2]}";
        }

        // Правило 3: прямоугольник — каждая буква берётся из своего столбца и строки другой буквы.
        rule = "Прямоугольник: обмен столбцов";
        return $"{matrix[r1, c2]}{matrix[r2, c1]}";
    }

    // Основная операция: шифрование (encrypt = true) или расшифрование (encrypt = false).
    private void Process(bool encrypt)
    {
        if (!ValidateKey()) return;

        // При шифровании источник — поле ввода, при расшифровании — поле результата.
        string source = encrypt ? inputBox.Text : resultBox.Text;
        if (string.IsNullOrWhiteSpace(source))
        {
            MessageBox.Show(encrypt ? "Введите исходный текст." : "Поле результата пустое — нечего расшифровывать.",
                "Нет текста", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        BuildMatrix(keyBox.Text);
        string prepared = PrepareText(source, !encrypt);
        if (prepared.Length == 0)
        {
            MessageBox.Show("В тексте не найдены латинские буквы A–Z.", "Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!encrypt && prepared.Length % 2 != 0)
        {
            MessageBox.Show("Для расшифрования длина текста после очистки должна быть чётной.", "Некорректный текст",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Обрабатываем пары и заполняем таблицу пошагового анализа.
        var output = new StringBuilder();
        detailsGrid.Rows.Clear();
        for (int i = 0; i < prepared.Length; i += 2)
        {
            string transformed = TransformPair(prepared[i], prepared[i + 1], encrypt, out string coords, out string rule);
            output.Append(transformed);
            detailsGrid.Rows.Add((i / 2 + 1).ToString(), $"{prepared[i]}{prepared[i + 1]}", coords, rule, transformed);
        }
        resultBox.Text = output.ToString();
        statusLabel.Text = $"{(encrypt ? "Шифрование" : "Расшифрование")} завершено. Обработано пар: {prepared.Length / 2}.";
    }

    // Загрузка исходного текста из файла (UTF-8).
    private void OpenText()
    {
        using var dialog = new OpenFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*", Title = "Открыть исходный текст" };
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            inputBox.Text = File.ReadAllText(dialog.FileName, Encoding.UTF8);
            statusLabel.Text = "Файл загружен: " + Path.GetFileName(dialog.FileName);
        }
    }

    // Сохранение результата в текстовый файл (UTF-8).
    private void SaveText()
    {
        if (string.IsNullOrWhiteSpace(resultBox.Text))
        {
            MessageBox.Show("Сначала получите результат шифрования или расшифрования.", "Нет результата",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using var dialog = new SaveFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*", DefaultExt = "txt", FileName = "result.txt" };
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            File.WriteAllText(dialog.FileName, resultBox.Text, Encoding.UTF8);
            statusLabel.Text = "Результат сохранён: " + Path.GetFileName(dialog.FileName);
        }
    }

    // Очистка полей ввода/результата и таблицы деталей. Ключ и матрица сохраняются.
    private void ClearAll()
    {
        inputBox.Clear();
        resultBox.Clear();
        detailsGrid.Rows.Clear();
        inputBox.Focus();
        statusLabel.Text = "Поля очищены.";
    }

    // Краткая справка по работе с программой и правилам подготовки текста.
    private void ShowHelp()
    {
        MessageBox.Show(
            "1. Введите ключ из латинских букв.\n" +
            "2. Введите исходный текст и нажмите «Зашифровать».\n" +
            "3. Для расшифрования вставьте шифротекст в поле «Результат» и нажмите «Расшифровать».\n" +
            "4. Таблица и пошаговый анализ показывают правила для каждой пары.\n\n" +
            "Используются латинские буквы; J объединяется с I. При шифровании текст очищается от пробелов и знаков, одинаковые соседние буквы разделяются X, а нечётная длина дополняется X.",
            "Инструкция", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}