using System;

namespace Runner.BonusSystem
{
    public interface IBonus
    {
        public Action<BonusController> OnObjectCollected { get; set; }
    }
}