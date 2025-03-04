using Runner.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.BonusSystem
{
    public class SpawnController : MonoBehaviour, IUpdateble, IDisposable
    {
        [SerializeField] private List<BonusSpawnPoint> _bonusSpawnPoints = new List<BonusSpawnPoint>(100);
        [SerializeField] private List<HurdleSpawnPoint> _hurdleSpawnPoints = new List<HurdleSpawnPoint>(30);
        [SerializeField] private float _loadDistance;
        [SerializeField] private Transform _bonusesHolderTransform;
        [SerializeField] private Transform _hurdlesHolderTransform;
        [SerializeField] private BonusesUpdateController _bonusesUpdateController;
        [SerializeField] private LevelScoreManager _levelScoreManager;
        [SerializeField] private Camera _worldCamera;

        private Transform _playerTransform;
        private BonusesPoolController _bonusesPool;
        private List<BonusController> _activeBonusControllers = new List<BonusController>(15);
        private List<HurdleController> _activeHurdleControllers = new List<HurdleController>(15);

        public List<HurdleSpawnPoint> HurdleSpawnPoints => _hurdleSpawnPoints;

        public void InitController(Transform playerTransform, BonusesPoolController bonusesPool)
        {
            _playerTransform = playerTransform;
            _bonusesPool = bonusesPool;
        }

        public void AddSpawnPoint(Vector3 spawnPosition)
        {
            var spawnPoint = new BonusSpawnPoint(spawnPosition);
            _bonusSpawnPoints.Add(spawnPoint);
        }

        public void AddHurdleSpawnPoint(float zSpawnPosition, bool isLeftBonus, bool isRightBonus)
        {
            var spawnPoint = new HurdleSpawnPoint(zSpawnPosition, isLeftBonus, isRightBonus);
            _hurdleSpawnPoints.Add(spawnPoint);
        }

        public void LocalUpdate(float deltaTime)
        {
            for(int i = 0; i < _bonusSpawnPoints.Count; i++)
            {
                if (!_bonusSpawnPoints[i].IsSpawned && _bonusSpawnPoints[i].SpawnPosition.z - _playerTransform.position.z < _loadDistance)
                {
                    SpawnBonus(_bonusSpawnPoints[i]);
                }
            }

            for (int i = 0; i < _hurdleSpawnPoints.Count; i++)
            {
                if (!_hurdleSpawnPoints[i].IsSpawned && _hurdleSpawnPoints[i].ZSpawnPosition - _playerTransform.position.z < _loadDistance)
                {
                    SpawnHurdle(_hurdleSpawnPoints[i]);
                }
            }
        }

        private void SpawnBonus(BonusSpawnPoint spawnPoint)
        {
            var bonus = _bonusesPool.GetBonus();
            bonus.InitObject(_playerTransform);
            bonus.transform.parent = _bonusesHolderTransform;
            bonus.transform.localPosition = spawnPoint.SpawnPosition;
            bonus.OnObjectNeedToRemove += RemoveBonus;

            spawnPoint.IsSpawned = true;

            _bonusesUpdateController.AddBonusController(bonus);
            _levelScoreManager.AddBonus(bonus);
            _activeBonusControllers.Add(bonus);
        }

        private void SpawnHurdle(HurdleSpawnPoint spawnPoint)
        {
            var leftHurdle = _bonusesPool.GetHurdle();
            leftHurdle.InitObject(_playerTransform, spawnPoint.IsLeftBonusHurdle, spawnPoint.HurdleLeftValue, _worldCamera);
            leftHurdle.transform.parent = _hurdlesHolderTransform;
            leftHurdle.transform.localPosition = new Vector3(-1, 0, spawnPoint.ZSpawnPosition);
            leftHurdle.OnObjectNeedToRemove += RemoveHurdle;

            var rightHurdle = _bonusesPool.GetHurdle();
            rightHurdle.InitObject(_playerTransform, spawnPoint.IsRightBonusHurdle, spawnPoint.HurdleRightValue, _worldCamera);
            rightHurdle.transform.parent = _hurdlesHolderTransform;
            rightHurdle.transform.localPosition = new Vector3(1, 0, spawnPoint.ZSpawnPosition);
            rightHurdle.OnObjectNeedToRemove += RemoveHurdle;

            spawnPoint.IsSpawned = true;

            _activeHurdleControllers.Add(leftHurdle);
            _activeHurdleControllers.Add(rightHurdle);
            _levelScoreManager.AddHurdle(leftHurdle);
            _levelScoreManager.AddHurdle(rightHurdle);
            _bonusesUpdateController.AddHurdleController(leftHurdle);
            _bonusesUpdateController.AddHurdleController(rightHurdle);
        }

        private void RemoveBonus(BonusController bonus)
        {
            bonus.OnObjectNeedToRemove -= RemoveBonus;
            _bonusesPool.ReturnBonus(bonus);
            _activeBonusControllers.Remove(bonus);
        }

        private void RemoveHurdle(HurdleController hurdle)
        {
            hurdle.OnObjectNeedToRemove -= RemoveHurdle;
            _bonusesPool.ReturnHurdle(hurdle);
            _activeHurdleControllers.Remove(hurdle);
        }

        public void Dispose()
        {
            for(int i = 0; i < _activeBonusControllers.Count; i++)
            {
                _activeBonusControllers[i].OnObjectNeedToRemove -= RemoveBonus;
            }

            for (int i = 0; i < _activeHurdleControllers.Count; i++)
            {
                _activeHurdleControllers[i].OnObjectNeedToRemove -= RemoveHurdle;
            }

            _activeBonusControllers.Clear();
            _activeHurdleControllers.Clear();
            _bonusSpawnPoints.Clear();
            _hurdleSpawnPoints.Clear();
        }
    }
}