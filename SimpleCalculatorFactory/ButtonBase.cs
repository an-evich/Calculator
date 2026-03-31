namespace SimpleCalculatorFactory.Models
{
    public abstract class ButtonBase
    {
        // Возвращает строку, которую нужно добавить к выражению
        public abstract string GetCommand();
    }
}