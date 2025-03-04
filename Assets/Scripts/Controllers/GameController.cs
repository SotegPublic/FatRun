using System;

namespace Runner.Core
{
    public class GameController : IGameSystem, IDisposable
    {
        private GameControllerModel _model;

        public GameController()
        {
            _model = new GameControllerModel();
        }

        public void AddController(IGameSystem controller)
        {
            if (controller is IUpdateble updateController)
            {
                _model.UpdateControllers.Add(updateController);
            }

            if (controller is ILateUpdateble lateUpdateController)
            {
                _model.LateUpdateControllers.Add(lateUpdateController);
            }

            if (controller is IDisposable disposable)
            {
                AddDisposable(disposable);
            }
        }

        public void AddDisposable(IDisposable disposable)
        {
            _model.Disposables.Add(disposable);
        }

        public void Update(float deltaTime)
        {
            for (var element = 0; element < _model.UpdateControllers.Count; ++element)
            {
                _model.UpdateControllers[element].LocalUpdate(deltaTime);
            }
        }

        public void LateUpdate(float deltaTime)
        {
            for (var element = 0; element < _model.LateUpdateControllers.Count; ++element)
            {
                _model.LateUpdateControllers[element].LateLocalUpdate(deltaTime);
            }
        }

        public void Dispose()
        {
            for (var element = 0; element < _model.Disposables.Count; ++element)
            {
                _model.Disposables[element].Dispose();
            }
        }
    }
}