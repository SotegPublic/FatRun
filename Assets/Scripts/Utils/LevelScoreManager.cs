using Runner.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Runner.BonusSystem
{
    public class LevelScoreManager : MonoBehaviour, IGameSystem, IDisposable
    {
        [SerializeField] private int _collectableBonusMultiplier;
        [SerializeField] private BonusesCountProperty _bonusesCountProperty;
        [SerializeField] private TMP_Text _bonusesCountText;
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField][Range(0.1f,1f)] private float _crushWallMultiplier = 0.1f;

        private List<IBonus> _bonuses = new List<IBonus>(100);
        private List<IHurdle> _hurdles = new List<IHurdle>(100);
        private int _levelScoreForWin;

        public BonusesCountProperty BonusesCountProperty => _bonusesCountProperty;

        public int GetPointsCountForWin()
        {
            return (int)(_mapGenerator.CurrentCapPointsForHurdles * _crushWallMultiplier);
        }

        public bool IsWallWillBeCrush()
        {
            var pointsCount = (int)(_mapGenerator.CurrentCapPointsForHurdles * _crushWallMultiplier);
            var isCrush = _bonusesCountProperty.Value - pointsCount  > 0 ? true : false;
            return isCrush;
        }

        public void InitManager()
        {
            _bonusesCountProperty = new BonusesCountProperty(_collectableBonusMultiplier);
            _bonusesCountProperty.Subscribe(UpdateBonusesCountText);
            _levelScoreForWin = (int)(_mapGenerator.CurrentCapPointsForHurdles * _crushWallMultiplier);
            _bonusesCountText.text = "0/" + _levelScoreForWin.ToString();
        }

        private void UpdateBonusesCountText(int newValue)
        {
            _bonusesCountText.text = newValue.ToString() + "/" + _levelScoreForWin.ToString();
        }

        public void AddBonus(IBonus bonus)
        {
            _bonuses.Add(bonus);
            bonus.OnObjectCollected += AddCollectedBonus;
        }

        public void AddHurdle(IHurdle hurdle)
        {
            _hurdles.Add(hurdle);
            hurdle.OnObjectCollected += AddHurdleBonus;
        }

        private void AddHurdleBonus(IHurdle hurdle)
        {
            _bonusesCountProperty.AddHurdleBonus(hurdle.BonusValue);
            hurdle.OnObjectCollected -= AddHurdleBonus;
        }

        private void AddCollectedBonus(IBonus bonus)
        {
            _bonusesCountProperty.AddCollectableBonus();
            bonus.OnObjectCollected -= AddCollectedBonus;
        }

        public void Dispose()
        {
            _bonusesCountProperty.Unsubscribe(UpdateBonusesCountText);
            for(int i = 0; i < _bonuses.Count; i++)
            {
                _bonuses[i].OnObjectCollected -= AddCollectedBonus;
            }
            _bonuses.Clear();
        }
    }
}