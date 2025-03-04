using System;

namespace Runner.Core
{
    public class PlayerCurrency : SubscribableProperty<int>
    {
        public void AddCurrency(int currency)
        {
            SetValue(_value + currency);
        }

        public void SubtractCurrency(int currency)
        {
            SetValue(_value - currency);
        }

        public void SetCurrency(int currency)
        {
            SetValue(currency);
        }
    }
}