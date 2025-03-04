using Runner.Animation;
using Runner.BonusSystem;
using Runner.CameraSystem;
using Runner.UI;
using System;
using UnityEngine;

namespace Runner.Core
{
    public class EndLevelController : MonoBehaviour, IUpdateble, IPausable
    {
        public Action OnPlayerOnKickPosition;

        [SerializeField] private float _restTime;
        [SerializeField] private float _prepearTime;
        [SerializeField] private float _whenLoseAwaitingTime;
        [SerializeField] private float _whenWinAwaitingTime;
        [SerializeField] private Transform _transform;
        [SerializeField] private GameObject _wallObject;
        [SerializeField] private LevelScoreManager _scoreManager;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private UISystemController _uiSystemController;
        [SerializeField] private ParticleSystem _wallCrushParticle;
        [SerializeField] private ParticleSystem _kickParticle;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _kickSoundClip;
        [SerializeField] private AudioClip _ldtSoundClip;
        [SerializeField][Range(0f,1f)] private float _kickVolume = 1f;
        [SerializeField][Range(0f, 1f)] private float _ldtVolume = 1f;

        private AnimationController _animationController;
        private LevelStatesManager _levelStatesManager;
        private float _currentRestTime;
        private float _currentPrepearTime;
        private float _currentEndTime;
        private PlayerGameModel _playerGameModel;
        private int _pointsForWin;
        private WallCrushStateTypes _currentStateType;

        private bool _isEndGamePanelShow;
        private bool _isOnPause;

        public Transform Transform => _transform;

        public void InitController(LevelStatesManager levelStatesManager, AnimationController playerAnimationController, PlayerGameModel playerGameModel)
        {
            _playerGameModel = playerGameModel;
            _animationController = playerAnimationController;
            _animationController.AnimationsEventManager.OnKickAnimationHit += WhenPlayerKickTheWall;
            _levelStatesManager = levelStatesManager;
            _currentStateType = WallCrushStateTypes.Rest;

            _pointsForWin = _scoreManager.GetPointsCountForWin();
        }

        public void LocalUpdate(float deltaTime)
        {
            if (_isOnPause) return;

            if (_levelStatesManager.CurrentState.IsKick)
            {
                switch (_currentStateType)
                {
                    case WallCrushStateTypes.Rest:
                        Rest(deltaTime);
                        break;
                    case WallCrushStateTypes.Prepear:
                        Prepear(deltaTime);
                        break;
                    case WallCrushStateTypes.Kick:
                        Kick();
                        break;
                    case WallCrushStateTypes.Dance:
                        WinGame(deltaTime);
                        break;
                    case WallCrushStateTypes.Sad:
                        LoseGame(deltaTime);
                        break;
                    default:
                    case WallCrushStateTypes.None:
                        break;
                }
            }
        }

        private void LoseGame(float deltaTime)
        {
            ShowLosePanel(deltaTime);
        }

        private void WinGame(float deltaTime)
        {
            ShowWinPanel(deltaTime);
        }

        private void ShowLosePanel(float deltaTime)
        {
            _currentEndTime += deltaTime;

            if (_currentEndTime >= _whenLoseAwaitingTime && !_isEndGamePanelShow)
            {
                _uiSystemController.ShowLosePanel(_scoreManager.BonusesCountProperty.Value, _pointsForWin);
                _isEndGamePanelShow = true;

                _playerGameModel.ResetWinStreak();
                _playerGameModel.ResetSpeed();
            }
        }

        private void ShowWinPanel(float deltaTime)
        {
            _currentEndTime += deltaTime;

            if(_currentEndTime >= _whenWinAwaitingTime && !_isEndGamePanelShow)
            {
                var reward = (int)((_scoreManager.BonusesCountProperty.Value - _pointsForWin) * _playerGameModel.SpeedModifier);

                _uiSystemController.ShowWinPanel(_scoreManager.BonusesCountProperty.Value, _pointsForWin, reward);

                _playerGameModel.PlayerCurrency.AddCurrency(reward);
                _playerGameModel.UpSpeedWhenWin();
                _playerGameModel.IncreaseWinStreak();
                _playerGameModel.SavePlayerData();

                _isEndGamePanelShow = true;
            }
        }

        private void Kick()
        {
            _animationController.ActivateKick();
        }

        private void WhenPlayerKickTheWall()
        {
            _audioSource.clip = _kickSoundClip;
            _audioSource.volume = _kickVolume;
            _audioSource.Play();

            if (_scoreManager.IsWallWillBeCrush())
            {
                _wallObject.SetActive(false);
                _wallCrushParticle.Play();
                if(_playerGameModel.DeviceID == "desktop")
                {
                    _kickParticle.Play();
                }
                _animationController.ActivateDance();
                _currentStateType = WallCrushStateTypes.Dance;
            }
            else
            {
                _animationController.ActivateSad();
                _currentStateType = WallCrushStateTypes.Sad;
            }
        }

        private void Rest(float deltaTime)
        {
            _currentRestTime += deltaTime;

            if (_currentRestTime >= _restTime)
            {
                _audioSource.clip = _ldtSoundClip;
                _audioSource.volume = _ldtVolume;
                _audioSource.Play();

                _currentStateType = WallCrushStateTypes.Prepear;
            }
        }

        private void Prepear(float deltaTime)
        {
            _animationController.ActivateFightIdle();
            _currentPrepearTime += deltaTime;

            if (_currentPrepearTime > _prepearTime)
            {
                _currentStateType = WallCrushStateTypes.Kick;
            }
        }

        public void WhenPlayerOnKickPosition()
        {
            OnPlayerOnKickPosition?.Invoke();
            _cameraController.WhenPlayerOnKickPosition();
        }

        public void ActivatePause(bool isPaused)
        {
            _isOnPause = isPaused;
        }
    }
}