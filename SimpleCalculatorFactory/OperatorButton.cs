namespace SimpleCalculatorFactory.Models
{
    public class OperatorButton : ButtonBase
    {
        private readonly string _operation;
        public OperatorButton(string operation) { _operation = operation; }
        public override string GetCommand() => _operation;
    }
}