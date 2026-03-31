using System;
using System.Data;
using System.Text.RegularExpressions;

namespace SimpleCalculatorFactory.Models
{
    public class CalculatorLogic
    {
        private bool _waitingForNewInput = false;
        private double _lastResult = 0;

        public double ComputeExpression(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return 0;
            try
            {
                string expr = expression.Replace(',', '.');
                var result = new DataTable().Compute(expr, null);
                return Convert.ToDouble(result);
            }
            catch
            {
                return double.NaN;
            }
        }

        public string ProcessInput(string currentDisplay, string buttonCommand)
        {
            // Полная очистка
            if (buttonCommand == "C")
            {
                _waitingForNewInput = false;
                return "0";
            }

            // Очистка текущего ввода (CE)
            if (buttonCommand == "CE")
            {
                if (string.IsNullOrEmpty(currentDisplay) || currentDisplay == "0")
                    return "0";
                int lastOpIndex = FindLastOperatorIndex(currentDisplay);
                if (lastOpIndex == -1)
                    return "0";
                else
                    return currentDisplay.Substring(0, lastOpIndex + 1);
            }

            // Вычисление результата
            if (buttonCommand == "=")
            {
                string expr = currentDisplay;
                if (!string.IsNullOrEmpty(expr) && expr != "Ошибка")
                {
                    double result = ComputeExpression(expr);
                    if (double.IsNaN(result))
                        return "Ошибка";
                    _lastResult = result;
                    _waitingForNewInput = true;
                    return result.ToString();
                }
                return currentDisplay;
            }

            // Квадратный корень
            if (buttonCommand == "√")
            {
                string expr = currentDisplay;
                if (string.IsNullOrEmpty(expr) || expr == "0" || expr == "Ошибка")
                {
                    _lastResult = 0;
                    _waitingForNewInput = true;
                    return "0";
                }
                double value = ComputeExpression(expr);
                if (double.IsNaN(value))
                    return "Ошибка";
                double sqrtResult = Math.Sqrt(value);
                _lastResult = sqrtResult;
                _waitingForNewInput = true;
                return sqrtResult.ToString();
            }

            // Обработка процента
            if (buttonCommand == "%")
            {
                string expr = currentDisplay;
                if (string.IsNullOrEmpty(expr) || expr == "0")
                    return "0";
                Match match = Regex.Match(expr, @"([-+]?\d*\.?\d+)$");
                if (match.Success)
                {
                    string lastNumber = match.Groups[1].Value;
                    double num = double.Parse(lastNumber.Replace(',', '.'));
                    double percent = num / 100.0;
                    string newExpr = expr.Substring(0, match.Index) + percent.ToString().Replace('.', ',');
                    return newExpr;
                }
                return currentDisplay;
            }

            // Смена знака (+/-)
            if (buttonCommand == "+/-")
            {
                string expr = currentDisplay;
                if (string.IsNullOrEmpty(expr) || expr == "0")
                    return "0";
                Match match = Regex.Match(expr, @"([-+]?\d*\.?\d+)$");
                if (match.Success)
                {
                    string lastNumber = match.Groups[1].Value;
                    double num = double.Parse(lastNumber.Replace(',', '.'));
                    num = -num;
                    string newExpr = expr.Substring(0, match.Index) + num.ToString().Replace('.', ',');
                    return newExpr;
                }
                return currentDisplay;
            }

            // Десятичная запятая
            if (buttonCommand == ",")
            {
                if (string.IsNullOrEmpty(currentDisplay) || IsOperator(currentDisplay[currentDisplay.Length - 1].ToString()))
                    return currentDisplay + "0,";
                Match match = Regex.Match(currentDisplay, @"(\d+)(,?\d*)$");
                if (match.Success && !match.Groups[2].Value.Contains(","))
                    return currentDisplay + ",";
                return currentDisplay;
            }

            // Скобки (просто добавляем символ)
            if (buttonCommand == "(" || buttonCommand == ")")
            {
                return currentDisplay + buttonCommand;
            }

            // Далее – цифры и операторы
            if (_waitingForNewInput && char.IsDigit(buttonCommand[0]))
            {
                _waitingForNewInput = false;
                return buttonCommand;
            }

            if (_waitingForNewInput && IsOperator(buttonCommand))
            {
                _waitingForNewInput = false;
                return _lastResult.ToString() + buttonCommand;
            }

            if (currentDisplay == "0" && !_waitingForNewInput && char.IsDigit(buttonCommand[0]))
            {
                return buttonCommand;
            }

            string currentExpr = currentDisplay;
            if (IsOperator(buttonCommand) && string.IsNullOrEmpty(currentExpr))
                return currentDisplay;

            if (IsOperator(buttonCommand) && currentExpr.Length > 0 && IsOperator(currentExpr[currentExpr.Length - 1].ToString()))
                return currentDisplay;

            return currentExpr + buttonCommand;
        }

        private bool IsOperator(string s)
        {
            return s == "+" || s == "-" || s == "*" || s == "/";
        }

        private int FindLastOperatorIndex(string expr)
        {
            for (int i = expr.Length - 1; i >= 0; i--)
            {
                char c = expr[i];
                if (c == '+' || c == '-' || c == '*' || c == '/')
                    return i;
            }
            return -1;
        }
    }
}