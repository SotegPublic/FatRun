using System;
using System.Collections.Generic;
using Runner.Input;

namespace Runner.Core
{
    public class PauseHandler: IGameSystem, IDisposable
    {
        private InputSystemModel _inputModel;
        private LevelStatesManager _levelStatesManager;
        private bool _isOnPause;
        private List<IPausable> _pausables = new List<IPausable>(5);
        
        public PauseHandler(InputSystemModel inputModel, LevelStatesManager levelStatesManager)
        {
            _inputModel = inputModel;
            _levelStatesManager = levelStatesManager;
            _inputModel.OnPauseButtonClick += WhenPressPauseButton;
        }

        public void WhenPressPauseButton()
        {
            if (_levelStatesManager.CurrentState.IsFinish || _levelStatesManager.CurrentState.IsKick) return;

            _isOnPause = !_isOnPause;

            for(int i = 0; i < _pausables.Count; i++)
            {
                _pausables[i].ActivatePause(_isOnPause);
            }
        }

        public void Subscribe(IPausable pausable)
        {
            _pausables.Add(pausable);
        }

        public void Unsubscribe(IPausable pausable)
        {
            _pausables.Remove(pausable);
        }

        public void Dispose()
        {
            _pausables.Clear();
        }
    }
}
