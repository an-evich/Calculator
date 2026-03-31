namespace SimpleCalculatorFactory.Models
{
    public class DigitButton : ButtonBase
    {
        private readonly int _digit;
        public DigitButton(int digit) { _digit = digit; }
        public override string GetCommand() => _digit.ToString();
    }
}