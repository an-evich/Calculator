using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SimpleCalculatorFactory.Factories;
using SimpleCalculatorFactory.Models;

namespace SimpleCalculatorFactory
{
    public partial class MainWindow : Window
    {
        private CalculatorLogic _logic;
        private IButtonFactory _factory;

        public MainWindow()
        {
            InitializeComponent();
            _logic = new CalculatorLogic();
            _factory = new ButtonFactory();
            CreateButtons();
        }

        private void CreateButtons()
        {
            var buttonsOrder = new List<(string type, string value)>
    {
        ("leftparen", "("), ("rightparen", ")"), ("ce", "CE"), ("clear", "C"), ("operator", "/"),
        ("digit", "7"), ("digit", "8"), ("digit", "9"), ("operator", "*"), ("sqrt", "√"),
        ("digit", "4"), ("digit", "5"), ("digit", "6"), ("operator", "-"), ("percent", "%"),
        ("digit", "1"), ("digit", "2"), ("digit", "3"), ("operator", "+"), ("sign", "+/-"),
        ("digit", "0"), ("comma", ","), ("equals", "="), ("", ""), ("", "")
    };

            foreach (var (type, value) in buttonsOrder)
            {
                // Пустая кнопка-заглушка
                if (string.IsNullOrEmpty(type))
                {
                    ButtonsGrid.Children.Add(new Button { Visibility = Visibility.Collapsed });
                    continue;
                }

                // Создание модели кнопки через фабрику
                ButtonBase btnModel = null;
                switch (type)
                {
                    case "digit": btnModel = _factory.CreateButton("digit", value); break;
                    case "operator": btnModel = _factory.CreateButton("operator", value); break;
                    case "equals": btnModel = _factory.CreateButton("equals"); break;
                    case "clear": btnModel = _factory.CreateButton("clear"); break;
                    case "percent": btnModel = _factory.CreateButton("percent"); break;
                    case "sign": btnModel = _factory.CreateButton("sign"); break;
                    case "comma": btnModel = _factory.CreateButton("comma"); break;
                    case "leftparen": btnModel = _factory.CreateButton("leftparen"); break;
                    case "rightparen": btnModel = _factory.CreateButton("rightparen"); break;
                    case "sqrt": btnModel = _factory.CreateButton("sqrt"); break;
                    case "ce": btnModel = _factory.CreateButton("ce"); break;
                }

                if (btnModel == null) continue;

                // Определение текста кнопки
                string content = value; // сначала берём значение из кортежа
                if (string.IsNullOrEmpty(content)) // если нет – задаём по типу
                {
                    switch (type)
                    {
                        case "clear": content = "C"; break;
                        case "ce": content = "CE"; break;
                        case "equals": content = "="; break;
                        case "percent": content = "%"; break;
                        case "sign": content = "+/-"; break;
                        case "comma": content = ","; break;
                        case "leftparen": content = "("; break;
                        case "rightparen": content = ")"; break;
                        case "sqrt": content = "√"; break;
                        default: content = ""; break;
                    }
                }

                var button = new Button
                {
                    Content = content,
                    FontSize = 20,
                    Margin = new Thickness(2)
                };
                button.Click += (s, e) => OnButtonClick(btnModel);
                ButtonsGrid.Children.Add(button);
            }
        }

        private void OnButtonClick(ButtonBase button)
        {
            string command = button.GetCommand();
            string newDisplay = _logic.ProcessInput(DisplayTextBox.Text, command);
            DisplayTextBox.Text = newDisplay;
        }
    }
}