using System;
using UnityEngine;

namespace Runner.BonusSystem
{
    [Serializable]
    public class BonusSpawnPoint
    {
        private readonly Vector3 _spawnPosition;

        public bool IsSpawned;
        public Vector3 SpawnPosition => _spawnPosition;

        public BonusSpawnPoint(Vector3 spawnPoint)
        {
            _spawnPosition = spawnPoint;
        }
    }
}