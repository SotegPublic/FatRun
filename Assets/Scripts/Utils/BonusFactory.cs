using UnityEngine;

namespace Runner.BonusSystem
{
    public class BonusFactory<T> where T : class
    {
        private GameObject _bonusPrefab;

        public BonusFactory(GameObject bonusPrefab)
        {
            _bonusPrefab = bonusPrefab;
        }

        public T CreateBonus(Transform poolTransform)
        {
            var bonusGameObject = Object.Instantiate(_bonusPrefab, poolTransform);
            var bonus = bonusGameObject.GetComponent<T>();

            return bonus;
        }
    }
}