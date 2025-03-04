using Runner.Animation;
using Runner.Core;
using Runner.Input;
using System;
using UnityEngine;

namespace Runner.Player
{
    public class MoveController : MonoBehaviour, IDisposable, IUpdateble, IPausable
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _finishSpeed;
        [SerializeField] private float _jumpWeight;
        [SerializeField] private Transform _transform;
        [SerializeField] private float _leftRoadPositionX;
        [SerializeField] private float _rightRoadPositionX;
        [SerializeField] private AnimationController _animationController;

        private InputSystemModel _inputModel;
        private PlayerGameModel _playerGameModel;
        private LevelStatesManager _levelStatesManager;
        private EndLevelController _EndLevelController;
        private float _jumpProgress;
        private bool _isOnPause;

        private Vector3 _playerEndPosition;
        private float _finishMoveProgress;

        private bool _isOnRightRoad;
        private bool _isOnLeftRoad;
        private bool _isOnJump;

        private bool _isLeftToMidle => _transform.position.x >= _leftRoadPositionX && _transform.position.x < 0;
        private bool _isMidleToRight => _transform.position.x >= 0 && _transform.position.x <= _rightRoadPositionX;
        private bool _isRightToMidle => _transform.position.x <= _rightRoadPositionX && _transform.position.x > 0;
        private bool _isMidleToLeft => _transform.position.x <= 0 && _transform.position.x >= _leftRoadPositionX;

        private void Awake()
        {
            _isOnLeftRoad = true;
        }

        public void InitController(InputSystemModel inputModel, LevelStatesManager levelStatesManager, EndLevelController endLevelController,
            PlayerGameModel playerGameModel)
        {
            _playerGameModel = playerGameModel;
            _inputModel = inputModel;
            _inputModel.OnLeftButtonClick += JumpOnLeftRoad;
            _inputModel.OnRightButtonClick += JumpOnRightRoad;

            _levelStatesManager = levelStatesManager;
            _EndLevelController = endLevelController;
        }

        private void JumpOnLeftRoad()
        {
            if (!_levelStatesManager.CurrentState.IsGame) return;
            if (_isOnPause) return;
            if (_isOnJump) return;
            if (_isOnLeftRoad) return;

            _isOnLeftRoad = true;
            _isOnRightRoad = false;
            _isOnJump = true;
        }

        private void JumpOnRightRoad()
        {
            if (!_levelStatesManager.CurrentState.IsGame) return;
            if (_isOnPause) return;
            if (_isOnJump) return;
            if (_isOnRightRoad) return;

            _isOnRightRoad = true;
            _isOnLeftRoad = false;
            _isOnJump = true;
        }

        public void LocalUpdate(float deltaTime)
        {
            if (_isOnPause) return;

            if (_levelStatesManager.CurrentState.IsGame)
            {
                _transform.position += _transform.forward * Time.deltaTime * (_speed * _playerGameModel.SpeedModifier);

                _animationController.ActivateRun();

                if (_isOnJump)
                {
                    if (_isOnLeftRoad)
                    {
                        _animationController.ActivateLJump();
                        JumpOnLeft(Time.deltaTime);
                    }

                    if (_isOnRightRoad)
                    {
                        _animationController.ActivateRJump();
                        JumpOnRight(Time.deltaTime);
                    }
                }
            }
            else if(_levelStatesManager.CurrentState.IsFinish)
            {
                if(_playerEndPosition == Vector3.zero)
                {
                    _playerEndPosition = _transform.position;
                }

                _finishMoveProgress += deltaTime / (1 / (_finishSpeed * _playerGameModel.SpeedModifier));

                var direction = Vector3.Lerp(_playerEndPosition, _EndLevelController.Transform.position, _finishMoveProgress);
                _transform.position = direction;

                if (_finishMoveProgress >= 1)
                {
                    _animationController.ActivateIdle();
                    _EndLevelController.WhenPlayerOnKickPosition();
                }
            }
        }

        private void JumpOnLeft(float deltaTime)
        {
            Jump(_rightRoadPositionX, _leftRoadPositionX, deltaTime, _isRightToMidle, _isMidleToLeft);
        }

        private void JumpOnRight(float deltaTime)
        {
            Jump(_leftRoadPositionX, _rightRoadPositionX, deltaTime, _isLeftToMidle, _isMidleToRight);
        }

        private void Jump(float startX, float endX, float deltaTime, bool toMidlePredicte, bool toEndPredicate)
        {
            _jumpProgress += deltaTime / (1 / (_jumpSpeed * _playerGameModel.SpeedModifier));

            if (toMidlePredicte)
            {
                JumpToMidle(startX);

                if (_jumpProgress >= 1)
                {
                    _jumpProgress = 0;
                }
            }
            else if (toEndPredicate)
            {
                JumpToEnd(endX);

                if (_jumpProgress >= 1)
                {
                    _jumpProgress = 0;
                    _isOnJump = false;

                    _animationController.ActivateRun();
                }
            }
        }

        private void JumpToMidle(float startX)
        {
            var currentTargetPosition = new Vector3(0, _jumpWeight, _transform.position.z);
            var lastPositionWithOffset = new Vector3(startX, 0, _transform.position.z);

            var direction = Vector3.Lerp(lastPositionWithOffset, currentTargetPosition, _jumpProgress);

            _transform.position = direction;
        }

        private void JumpToEnd(float endX)
        {
            var currentTargetPosition = new Vector3(endX, 0, _transform.position.z);
            var lastPositionWithOffset = new Vector3(0, _jumpWeight, _transform.position.z);

            var direction = Vector3.Lerp(lastPositionWithOffset, currentTargetPosition, _jumpProgress);

            _transform.position = direction;
        }

        public void ActivatePause(bool isPaused)
        {
            _isOnPause = isPaused;
        }

        public void Dispose()
        {
            _inputModel.OnLeftButtonClick -= JumpOnLeftRoad;
            _inputModel.OnRightButtonClick -= JumpOnRightRoad;
            _inputModel = null;
        }
    }
}