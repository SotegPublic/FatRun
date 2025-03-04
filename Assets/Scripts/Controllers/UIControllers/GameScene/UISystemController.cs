using Runner.BonusSystem;
using Runner.Core;
using Runner.Player;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.UI
{
    public class UISystemController : MonoBehaviour, IPausable, IDisposable, IGameSystem
    {
        public Action OnStartGameButtonClick;

        [SerializeField] private FinishController _finishController;
        [SerializeField] private AnimatedPauseButton _pauseButton;
        [SerializeField] private AnimatedButton _startGameButton;
        [SerializeField] private Canvas _startGamePanelCanvas;
        [SerializeField] private Canvas _topPanelCanvas;
        [SerializeField] private Canvas _navigationPanelCanvas;
        [SerializeField] private WinPanelController _winPanelController;
        [SerializeField] private LosePanelController _losePanelController;
        [SerializeField] private PausePanelController _pausePanelController;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private TMP_Text _winsText;

        private YandexFunctions _yandexFunctions;
        private PlayerGameModel _playerGameModel;
        private SoundController _soundController;
        private int _doubleReward;
        private bool _isRewardDoubled = false;

        public PausePanelController PausePanelController => _pausePanelController;

        public void InitController(YandexFunctions yandex, PlayerGameModel playerGameModel, SoundController soundController)
        {
            _playerGameModel = playerGameModel;
            _yandexFunctions = yandex;
            _soundController = soundController;

            _winPanelController.Init();
            _losePanelController.Init();

            _startGameButton.onClick.AddListener(WhenStartGameButtonClicked);
            _finishController.OnPlayerFinished += DeactivateNavigationButtons;
            _winPanelController.RestartButton.onClick.AddListener(RestartLevelClick);
            _winPanelController.ReturnButton.onClick.AddListener(ReturnToMainMenuClick);
            _losePanelController.RestartButton.onClick.AddListener(RestartLevelClick);
            _losePanelController.ReturnButton.onClick.AddListener(ReturnToMainMenuClick);
            _winPanelController.DoubleButton.onClick.AddListener(ShowDoubleAdv);
            _playerGameModel.OnRewardDoubled += UpdateRewardAfterDouble;
            _playerGameModel.OnRewardVideoClosed += WhenRewardVideoClosed;
            _playerGameModel.OnAdvBeforeMainMenuClosed += OpenMainMenu;
            _pausePanelController.InitController(playerGameModel, yandex);

            _speedText.text = "x" + Math.Round(_playerGameModel.SpeedModifier, 1).ToString();
            _winsText.text = playerGameModel.WinStreak.ToString();
        }

        private void WhenStartGameButtonClicked()
        {
            if (_playerGameModel.DeviceID != "desktop")
            {
                _navigationPanelCanvas.enabled = true;
            }

            _topPanelCanvas.enabled = true;
            _startGamePanelCanvas.enabled = false;
            OnStartGameButtonClick?.Invoke();
        }

        private void ChangePauseButton(bool isOnPause)
        {
            _pauseButton.SwitchSprites(isOnPause);
        }

        private void ReturnToMainMenuClick()
        {
            var currentTime = DateTime.Now;
            var timeDiff = currentTime - _playerGameModel.LastAdvTime;

            if (timeDiff.Minutes >= 1 )
            {
                AudioListener.volume = 0f;
                _yandexFunctions.ShowAdvertisingBeforeMainMenu();
                _playerGameModel.SetLastAdvTime(currentTime);
            }
            else
            {
                OpenMainMenu();
            }
        }

        private void RestartLevelClick()
        {
            var currentTime = DateTime.Now;
            var timeDiff = currentTime - _playerGameModel.LastAdvTime;

            if (timeDiff.Minutes >= 1)
            {
                AudioListener.volume = 0f;
                _yandexFunctions.ShowAdvertisingBetweenLevels();
                _playerGameModel.SetLastAdvTime(currentTime);
            }

            _soundController.IsLevelRestarted = true;
            SceneManager.LoadScene("GameScene");
        }

        private void OpenMainMenu()
        {
            _soundController.Stop();
            _soundController.IsLevelRestarted = false;
            SceneManager.LoadScene("StartScene");
        }

        private void UpdateRewardAfterDouble()
        {
            _winPanelController.SetBonusText(_doubleReward * 2);
            _isRewardDoubled = true;
        }

        private void WhenRewardVideoClosed()
        {
            _winPanelController.ReturnInteractableControllerButtons(_isRewardDoubled);
        }

        private void ShowDoubleAdv()
        {
            AudioListener.volume = 0f;
            _yandexFunctions.ShowDoubleAdvertising(_doubleReward);
            var currentTime = DateTime.Now;
            _playerGameModel.SetLastAdvTime(currentTime);
        }

        public void ShowWinPanel(int playerPoints, int pointsForWin, int reward)
        {
            _winPanelController.SetGotPointText(playerPoints, pointsForWin);
            _winPanelController.SetBonusText(reward);
            _doubleReward = reward;

            _winPanelController.ShowCanvas();
        }

        public void ShowLosePanel(int playerPoints, int pointsForWin)
        {
            _losePanelController.SetGotPointText(playerPoints, pointsForWin);
            _losePanelController.ShowCanvas();
        }

        public void DeactivateNavigationButtons()
        {
            _navigationPanelCanvas.enabled = false;
        }

        public void ActivatePause(bool isOnPause)
        {
            ChangePauseButton(isOnPause);

            if(isOnPause)
            {
                _pausePanelController.ShowCanvas();
            }
            else
            {
                _pausePanelController.HideCanvas();
            }
        }

        public void Dispose()
        {
            _startGameButton.onClick.RemoveAllListeners();
            _finishController.OnPlayerFinished -= DeactivateNavigationButtons;
            _winPanelController.RestartButton.onClick.RemoveAllListeners();
            _winPanelController.ReturnButton.onClick.RemoveAllListeners();
            _losePanelController.RestartButton.onClick.RemoveAllListeners();
            _losePanelController.ReturnButton.onClick.RemoveAllListeners();
            _winPanelController.DoubleButton.onClick.RemoveAllListeners();
            _playerGameModel.OnRewardDoubled -= UpdateRewardAfterDouble;
            _playerGameModel.OnRewardVideoClosed -= WhenRewardVideoClosed;
            _playerGameModel.OnAdvBeforeMainMenuClosed -= OpenMainMenu;
            _isRewardDoubled = false;

            _winPanelController.Dispose();
            _losePanelController.Dispose();
            _pausePanelController.Dispose();
        }
    }
}