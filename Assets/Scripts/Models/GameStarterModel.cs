using Runner.BonusSystem;
using Runner.CameraSystem;
using Runner.Player;
using Runner.UI;
using System;
using UnityEngine;

namespace Runner.Core
{
    [Serializable]
    public class GameStarterModel
    {
        [SerializeField] private PlayerSystemsController _playerSystemsController;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private BonusesUpdateController _bonusesController;
        [SerializeField] private BonusesPoolController _poolController;
        [SerializeField] private SpawnController _spawnController;
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private LevelScoreManager _scoreManager;
        [SerializeField] private FinishController _finishController;
        [SerializeField] private EndLevelController _endLevelController;
        [SerializeField] private UISystemController _uISystemController;
        [SerializeField] private TranslateController _translateController;

        public PlayerSystemsController PlayerSystemsController => _playerSystemsController;
        public CameraController CameraController => _cameraController;
        public BonusesUpdateController BonusesController => _bonusesController;
        public BonusesPoolController BonusesPoolController => _poolController;
        public SpawnController SpawnController => _spawnController;
        public MapGenerator MapGenerator => _mapGenerator;
        public LevelScoreManager ScoreManager => _scoreManager;
        public UISystemController UISystemController => _uISystemController;
        public FinishController FinishController => _finishController;
        public EndLevelController EndLevelController => _endLevelController;
        public TranslateController TranslateController => _translateController;
    }
}