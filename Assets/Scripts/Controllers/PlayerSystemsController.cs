using Runner.Animation;
using Runner.Core;
using Runner.Input;
using System;
using UnityEngine;

namespace Runner.Player
{
    public class PlayerSystemsController : MonoBehaviour, IUpdateble, IDisposable
    {
        [SerializeField] private MoveController _moveController;
        [SerializeField] private AnimationController _animationController;
        [SerializeField] private Transform _playerTransform;

        private PlayerGameModel _playerGameModel;

        public Transform PlayerTransform => _playerTransform;
        public MoveController MoveController => _moveController;
        public AnimationController AnimationController => _animationController;

        public void InitPlayerSystem(InputSystemModel inputSystemModelmodel, LevelStatesManager levelStatesManager, EndLevelController endLevelController,
            PlayerGameModel playerGameModel)
        {
            _playerGameModel = playerGameModel;
            Instantiate(_playerGameModel.RunnerModel, _playerTransform);
            
            _moveController.InitController(inputSystemModelmodel, levelStatesManager, endLevelController, playerGameModel);
            _animationController.InitController(playerGameModel);
        }

        public void LocalUpdate(float deltaTime)
        {
            _moveController.LocalUpdate(deltaTime);
        }

        public void Dispose()
        {
            _moveController.Dispose();
        }
    }
}