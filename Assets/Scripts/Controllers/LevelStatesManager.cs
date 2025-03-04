using Runner.UI;
using System;

namespace Runner.Core
{
    public class LevelStatesManager: IGameSystem, IDisposable
    {
        private IGameState _currentState;

        private StartState _startState;
        private GameState _gameState;
        private FinishState _finishState;
        private KickState _kickState;

        private UISystemController _uiSystemController;
        private FinishController _finishController;
        private EndLevelController _endLevelController;

        public IGameState CurrentState => _currentState;
        
        public LevelStatesManager(UISystemController uiSystemController, FinishController finishController, EndLevelController endLevelController)
        {
            _uiSystemController = uiSystemController;
            _uiSystemController.OnStartGameButtonClick += SetGameState;

            _finishController = finishController;
            _finishController.OnPlayerFinished += SetFinishState;

            _endLevelController = endLevelController;
            _endLevelController.OnPlayerOnKickPosition += SetKickState;

            _startState = new StartState();
            _finishState = new FinishState();
            _gameState = new GameState();
            _kickState = new KickState();

            SetStartState();
        }

        private void SetStartState()
        {
            _currentState = _startState;
        }
        
        private void SetGameState()
        {
            _currentState = _gameState;
        }

        private void SetFinishState() 
        { 
            _currentState = _finishState;
        }

        private void SetKickState()
        {
            _currentState = _kickState;
        }

        public void Dispose()
        {
            _uiSystemController.OnStartGameButtonClick -= SetGameState;
            _finishController.OnPlayerFinished -= SetFinishState;
            _endLevelController.OnPlayerOnKickPosition -= SetKickState;
        }
    }
}