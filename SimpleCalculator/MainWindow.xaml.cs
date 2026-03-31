using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        private double currentValue = 0;      // Текущее значение (результат или первое число)
        private double memoryValue = 0;        // Сохранённое значение для операций
        private string currentOperation = "";  // Текущая операция (+, -, ×, ÷)
        private bool isNewInput = true;        // Флаг: нужно ли начинать новый ввод
        private bool isDecimalEntered = false; // Флаг: уже введена десятичная точка

        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик для цифровых кнопок
        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string number = button.Content.ToString();

            if (isNewInput)
            {
                DisplayTextBox.Text = number;
                isNewInput = false;
                isDecimalEntered = false;
            }
            else
            {
                // Предотвращаем ведущие нули (если текущий текст "0" и не десятичная дробь)
                if (DisplayTextBox.Text == "0" && !isDecimalEntered)
                    DisplayTextBox.Text = number;
                else
                    DisplayTextBox.Text += number;
            }
        }

        // Обработчик для десятичной точки
        private void DecimalButton_Click(object sender, RoutedEventArgs e)
        {
            if (isNewInput)
            {
                DisplayTextBox.Text = "0.";
                isNewInput = false;
                isDecimalEntered = true;
            }
            else if (!isDecimalEntered)
            {
                DisplayTextBox.Text += ".";
                isDecimalEntered = true;
            }
        }

        // Обработчик для арифметических операций
        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string operation = button.Content.ToString();

            // Если уже есть ожидающая операция, выполняем её перед сохранением новой
            if (!isNewInput && currentOperation != "")
            {
                CalculateResult();
            }

            // Сохраняем текущее значение из дисплея
            if (double.TryParse(DisplayTextBox.Text, out currentValue))
            {
                memoryValue = currentValue;
                currentOperation = operation;
                isNewInput = true;
                isDecimalEntered = false;
            }
            else
            {
                DisplayTextBox.Text = "Ошибка";
                isNewInput = true;
            }
        }

        // Обработчик кнопки "="
        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentOperation != "" && !isNewInput)
            {
                CalculateResult();
                currentOperation = "";  // Сбрасываем операцию после вычисления
            }
        }

        // Выполнение вычисления
        private void CalculateResult()
        {
            double secondNumber;
            if (!double.TryParse(DisplayTextBox.Text, out secondNumber))
            {
                DisplayTextBox.Text = "Ошибка";
                isNewInput = true;
                return;
            }

            double result = 0;
            bool error = false;

            switch (currentOperation)
            {
                case "+":
                    result = memoryValue + secondNumber;
                    break;
                case "-":
                    result = memoryValue - secondNumber;
                    break;
                case "×":
                    result = memoryValue * secondNumber;
                    break;
                case "÷":
                    if (secondNumber == 0)
                    {
                        DisplayTextBox.Text = "Деление на 0";
                        error = true;
                    }
                    else
                    {
                        result = memoryValue / secondNumber;
                    }
                    break;
                default:
                    return;
            }

            if (!error)
            {
                // Отображаем результат, убирая лишние десятичные знаки при необходимости
                DisplayTextBox.Text = result.ToString("0.##########");
                isNewInput = true;
                isDecimalEntered = false;
            }
            else
            {
                isNewInput = true;
            }
        }

        // Кнопка полной очистки (C)
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayTextBox.Text = "0";
            currentValue = 0;
            memoryValue = 0;
            currentOperation = "";
            isNewInput = true;
            isDecimalEntered = false;
        }

        // Кнопка очистки текущего ввода (CE)
        private void ClearEntryButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayTextBox.Text = "0";
            isNewInput = true;
            isDecimalEntered = false;
        }

        // Кнопка смены знака (+/-)
        private void SignButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(DisplayTextBox.Text, out double value))
            {
                value = -value;
                DisplayTextBox.Text = value.ToString("0.##########");
                // После смены знака продолжаем текущий ввод, флаги не сбрасываем
            }
            else
            {
                DisplayTextBox.Text = "Ошибка";
                isNewInput = true;
            }
        }
    }
}