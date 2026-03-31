using SimpleCalculatorFactory.Models;
using System;
using System.Windows.Controls;

namespace SimpleCalculatorFactory.Factories
{
    public interface IButtonFactory
    {
        ButtonBase CreateButton(string type, string value = null);
    }

    public class ButtonFactory : IButtonFactory
    {
        public ButtonBase CreateButton(string type, string value = null)
        {
            switch (type.ToLower())
            {
                case "digit":
                    if (int.TryParse(value, out int digit))
                        return new DigitButton(digit);
                    throw new ArgumentException("Invalid digit");
                case "operator":
                    return new OperatorButton(value);
                case "equals":
                    return new EqualsButton();
                case "clear":
                    return new ClearButton();
                case "percent":
                    return new PercentButton();
                case "sign":
                    return new SignButton();
                case "comma":
                    return new CommaButton();
                case "leftparen":
                    return new LeftParenButton();
                case "rightparen":
                    return new RightParenButton();
                case "sqrt":
                    return new SqrtButton();
                case "ce":
                    return new CEButton();
                default:
                    throw new ArgumentException("Unknown button type");
            }
        }
    }
}