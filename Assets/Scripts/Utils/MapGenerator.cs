using Runner.BonusSystem;
using UnityEngine;

namespace Runner.Core
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private float _roadLenth;
        [SerializeField] private int _generationStartPosition;
        [SerializeField] private int _bonusSpawnStep;
        [SerializeField] private int _hurdleSpawnStep;
        [SerializeField] private float _bonusPositionY;
        [SerializeField] private SpawnController _spawnController;
        [SerializeField] private int _minCapPointsForHurdles;
        [SerializeField] private int _maxCapPointsForHurdles;

        private int _currentCapPointsForHurdles;
        private int _currentPointsBalance;
        private double _halfPointsDividor;
        private double _medianBonusValue;
        private bool _isLastBonusNotSpawned;

        public int CurrentCapPointsForHurdles => _currentCapPointsForHurdles;

        public void GenerateMap()
        {
            _currentCapPointsForHurdles = Random.Range(_minCapPointsForHurdles, _maxCapPointsForHurdles);

            GenerateBonusesAndHurdles();
            GenerateHurglesValues();
        }

        private void GenerateBonusesAndHurdles()
        {
            for (int i = _generationStartPosition; i < _roadLenth; i++)
            {
                if (i % _bonusSpawnStep == 0 && i % _hurdleSpawnStep != 0)
                {
                    var randomX = Random.Range(-1, 2);

                    if (randomX != 0)
                    {
                        _spawnController.AddSpawnPoint(new Vector3(randomX, _bonusPositionY, i));
                    }
                    else
                    {
                        if(_isLastBonusNotSpawned)
                        {
                            randomX = Random.Range(0, 2);
                            var x = randomX > 0 ? randomX : randomX - 1;

                            _spawnController.AddSpawnPoint(new Vector3(x, _bonusPositionY, i));
                            _isLastBonusNotSpawned = false;
                        }
                        else
                        {
                            _isLastBonusNotSpawned = true;
                        }
                    }
                }

                if (i % _hurdleSpawnStep == 0)
                {
                    var bonusRandom = Random.Range(0, 2);

                    if (bonusRandom == 0)
                    {
                        _spawnController.AddHurdleSpawnPoint(i, true, false);
                    }
                    else
                    {
                        _spawnController.AddHurdleSpawnPoint(i, false, true);
                    }
                }
            }
        }

        private void GenerateHurglesValues()
        {
            var currentHalfPointsBalance = (int)(_currentCapPointsForHurdles * 0.5);
            var halfPointsIndex = (int)(_spawnController.HurdleSpawnPoints.Count * 0.5);

            _medianBonusValue = _currentCapPointsForHurdles / _spawnController.HurdleSpawnPoints.Count;
            _halfPointsDividor = 1 / (_spawnController.HurdleSpawnPoints.Count * 0.5);
            _currentPointsBalance = _currentCapPointsForHurdles - currentHalfPointsBalance;

            for (int i = _spawnController.HurdleSpawnPoints.Count - 1; i >= 0; i--)
            {
                var hurdleSpawnPoint = _spawnController.HurdleSpawnPoints[i];

                if (i >= halfPointsIndex)
                {
                    CalculateAndSetHurgleValues(ref currentHalfPointsBalance, i, hurdleSpawnPoint, halfPointsIndex);
                }
                else
                {
                    CalculateAndSetHurgleValues(ref _currentPointsBalance, i, hurdleSpawnPoint, 0);
                }
            }
        }

        private void CalculateAndSetHurgleValues(ref int currentBalance, int hurdlePosition, HurdleSpawnPoint hurdleSpawnPoint, int calculatedRangeEndPointIndex)
        {
            var bonusValue = hurdlePosition == calculatedRangeEndPointIndex ? currentBalance : CalculateBonusValue(currentBalance, hurdlePosition, calculatedRangeEndPointIndex);

            currentBalance -= bonusValue;
            var negativeValue = -(bonusValue - Random.Range(0, bonusValue));

            SetHurgleValues(hurdleSpawnPoint, bonusValue, negativeValue);
        }

        private int CalculateBonusValue(int halfPointsBalance, int hurglePosition, int endPointIndex)
        {
            double currentMedianBalance;
            var value = Random.Range(0, (int)(halfPointsBalance * _halfPointsDividor));

            currentMedianBalance = halfPointsBalance / (hurglePosition - endPointIndex);

            value = currentMedianBalance > _medianBonusValue ? value + (int)(_medianBonusValue * 0.7) : value;

            return value;
        }

        private static void SetHurgleValues(HurdleSpawnPoint hurdleSpawnPoint, int bonusValue, int negativeValue)
        {
            if (hurdleSpawnPoint.IsLeftBonusHurdle)
            {
                hurdleSpawnPoint.SetBonusValue(bonusValue, negativeValue);
            }
            else
            {
                hurdleSpawnPoint.SetBonusValue(negativeValue, bonusValue);
            }
        }
    }
}