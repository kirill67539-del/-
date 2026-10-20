using System;
using System.Globalization;
using System.Windows;
namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        private double firstNumber = 0; // Первое число
        private string operation = ""; // Выбранная операция
        private bool startNewNumber = true; // Начать ввод заново
        private readonly CultureInfo culture = CultureInfo.InvariantCulture;
        public MainWindow()
        {
            InitializeComponent(); // Загружаем интерфейс
        }
        // Получаем число с экрана
        private double GetNumber() =>
            double.Parse(Display.Text, culture);
        // Ввод цифр
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            string n = ((System.Windows.Controls.Button)sender).Content.ToString();
            if (startNewNumber || Display.Text == "0")
                Display.Text = n;
            else
                Display.Text += n;

            startNewNumber = false;
        }
        // Ввод дробной точки
        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            if (startNewNumber)
                Display.Text = "0.";
            else if (!Display.Text.Contains("."))
                Display.Text += ".";
            startNewNumber = false;
        }
        // Выбор операции
        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            string op = ((System.Windows.Controls.Button)sender).Content.ToString();
            double current = GetNumber();
            if (operation != "" && !startNewNumber)
            {
                if (operation == "/" && current == 0)
                {
                    MessageBox.Show("На ноль делить нельзя!");
                    return;
                }
                firstNumber = Calculate(firstNumber, current, operation);
                Display.Text = firstNumber.ToString("G", culture);
            }
            else if (operation == "")
                firstNumber = current;
            operation = op;
            startNewNumber = true;
        }
        // Вычисление результата
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (operation == "")
                return;
            double second = GetNumber();
            if (operation == "/" && second == 0)
            {
                MessageBox.Show("На ноль делить нельзя!");
                return;
            }
            firstNumber = Calculate(firstNumber, second, operation);
            Display.Text = firstNumber.ToString("G", culture);
            operation = "";
            startNewNumber = true;
        }
        // Выполняем выбранную операцию
        private double Calculate(double a, double b, string op)
        {
            switch (op)
            {
                case "+": return a + b;
                case "-": return a - b;
                case "*": return a * b;
                case "/": return a / b;
                default: return b;
            }
        }
        // Очистка калькулятора
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            firstNumber = 0;
            operation = "";
            startNewNumber = true;
        }
        // Удаление последнего символа
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (startNewNumber)
                return;

            Display.Text = Display.Text.Length > 1
                ? Display.Text[..^1]
                : "0";
        }
        // Изменение знака числа
        private void Sign_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = (-GetNumber()).ToString("G", culture);
            startNewNumber = false;
        }
    }
}