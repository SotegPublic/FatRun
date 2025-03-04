using Runner.Player;
using UnityEngine;

namespace Runner.Core
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private GameStarterModel _gameStarterModel;

        private GameController _gameController;
        private SoundController _soundController;
        private bool _isStarted;

        private void Awake()
        {
            _soundController = FindObjectOfType<SoundController>();
            _soundController.PlayInLevel();
            _gameController = new GameController();

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            if(_soundController.IsPlayed() && !_isStarted)
            {
                var yandex = FindObjectOfType<YandexFunctions>();
                var playerGameModel = FindObjectOfType<PlayerGameModel>();
                new GameInitializator(_gameController, _gameStarterModel, playerGameModel, yandex, _soundController);
                _isStarted = true;
            }

            var deltaTime = Time.deltaTime;
            _gameController.Update(deltaTime);
        }

        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            _gameController.LateUpdate(deltaTime);
        }

        private void OnDestroy()
        {
            _gameController.Dispose();
        }
    }
}