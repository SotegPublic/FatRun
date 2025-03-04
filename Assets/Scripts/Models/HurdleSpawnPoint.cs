using System;
using UnityEngine;

namespace Runner.BonusSystem
{
    [Serializable]
    public class HurdleSpawnPoint
    {
        [SerializeField] private readonly float _zSpawnPosition;
        [SerializeField] private readonly bool _isLeftBonusHurdle;
        [SerializeField] private readonly bool _isRightBonusHurdle;
        [SerializeField] private int _leftValue;
        [SerializeField] private int _rightValue;

        public bool IsSpawned;
        public float ZSpawnPosition => _zSpawnPosition;
        public bool IsLeftBonusHurdle => _isLeftBonusHurdle;
        public bool IsRightBonusHurdle => _isRightBonusHurdle;
        public int HurdleLeftValue => _leftValue;
        public int HurdleRightValue => _rightValue;

        public HurdleSpawnPoint(float zSpawnPoint, bool isLeftBonusHurdle, bool isRightBonusHurdle)
        {
            _zSpawnPosition = zSpawnPoint;
            _isLeftBonusHurdle = isLeftBonusHurdle;
            _isRightBonusHurdle = isRightBonusHurdle;
        }

        public void SetBonusValue(int valueLeft, int valueRight)
        {
            _leftValue = valueLeft;
            _rightValue = valueRight;
        }
    }
}