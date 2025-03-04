using Runner.Core;
using System;
using UnityEngine.InputSystem;

namespace Runner.Input
{
    public class InputSystemController : IGameSystem, IDisposable
    {
        private UserInput _userInputSystem;
        private InputSystemModel _model;

        public InputSystemModel InputModel => _model;

        public InputSystemController()
        {
            _model = new InputSystemModel();

            _userInputSystem = new UserInput();
            _userInputSystem.Enable();

            _userInputSystem.Player.LeftButton.performed += OnLeftButtonClick;
            _userInputSystem.Player.RightButton.performed += OnRightButtonClick;
            _userInputSystem.Player.PauseButton.performed += OnPauseButtonClick;
        }

        private void OnPauseButtonClick(InputAction.CallbackContext context)
        {
            _model.OnPauseButtonClick?.Invoke();
        }

        private void OnLeftButtonClick(InputAction.CallbackContext context)
        {
            _model.OnLeftButtonClick?.Invoke();
        }

        private void OnRightButtonClick(InputAction.CallbackContext context)
        {
            _model.OnRightButtonClick?.Invoke();
        }

        public void Dispose()
        {
            _userInputSystem.Player.LeftButton.performed -= OnLeftButtonClick;
            _userInputSystem.Player.RightButton.performed -= OnRightButtonClick;
            _userInputSystem.Player.PauseButton.performed -= OnPauseButtonClick;
            _userInputSystem.Disable();
        }
    }
}