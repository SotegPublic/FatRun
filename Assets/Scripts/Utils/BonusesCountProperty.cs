using Runner.Core;
using System;

namespace Runner.BonusSystem
{
    public class BonusesCountProperty: SubscribableProperty<int>
    {
        private int _bonusMultipler;
        public BonusesCountProperty(int multipler)
        {
            _bonusMultipler = multipler;
        }

        public void AddHurdleBonus(int vlaue)
        {
            var newValue = _value + vlaue;
            SetValue(newValue);
        }

        public void AddCollectableBonus()
        {
            var newValue = _value + (1 * _bonusMultipler);
            SetValue(newValue);
        }
    }
}