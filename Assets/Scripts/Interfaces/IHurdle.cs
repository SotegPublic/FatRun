using System;

namespace Runner.BonusSystem
{
    public interface IHurdle
    {
        public Action<HurdleController> OnObjectCollected { get; set; }
        public int BonusValue { get; }
    }
}