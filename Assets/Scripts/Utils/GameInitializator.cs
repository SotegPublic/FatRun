using Runner.Input;

namespace Runner.Core
{
    public class GameInitializator
    {
        public GameInitializator(GameController gameController, GameStarterModel gameStarterModel, PlayerGameModel playerGameModel, YandexFunctions yandex, Player.SoundController soundController)
        {
            yandex.SetFocusOnGame();
            gameStarterModel.TranslateController.TranslateTexts(playerGameModel.LanguageID);

            var inputSystemController = new InputSystemController();
            var levelStatesManager = new LevelStatesManager(gameStarterModel.UISystemController, gameStarterModel.FinishController, gameStarterModel.EndLevelController);
            var pauseHandler = new PauseHandler(inputSystemController.InputModel, levelStatesManager);

            var poolController = gameStarterModel.BonusesPoolController;
            poolController.InitPool();

            gameStarterModel.MapGenerator.GenerateMap();

            gameStarterModel.PlayerSystemsController.InitPlayerSystem(inputSystemController.InputModel, levelStatesManager, gameStarterModel.EndLevelController,
                playerGameModel);
            gameStarterModel.EndLevelController.InitController(levelStatesManager, gameStarterModel.PlayerSystemsController.AnimationController, playerGameModel);
            gameStarterModel.ScoreManager.InitManager();
            gameStarterModel.UISystemController.InitController(yandex, playerGameModel, soundController);
            gameStarterModel.SpawnController.InitController(gameStarterModel.PlayerSystemsController.PlayerTransform, poolController);

            pauseHandler.Subscribe(gameStarterModel.PlayerSystemsController.MoveController);
            pauseHandler.Subscribe(gameStarterModel.PlayerSystemsController.AnimationController);
            pauseHandler.Subscribe(gameStarterModel.UISystemController);
            pauseHandler.Subscribe(gameStarterModel.EndLevelController);

            gameController.AddController(gameStarterModel.PlayerSystemsController);
            gameController.AddController(inputSystemController);
            gameController.AddController(pauseHandler);
            gameController.AddController(gameStarterModel.EndLevelController);
            gameController.AddController(gameStarterModel.CameraController);
            gameController.AddController(gameStarterModel.BonusesController);
            gameController.AddController(gameStarterModel.BonusesPoolController);
            gameController.AddController(gameStarterModel.SpawnController);
            gameController.AddController(gameStarterModel.ScoreManager);
            gameController.AddController(levelStatesManager);
            gameController.AddController(gameStarterModel.UISystemController);
            gameController.AddController(gameStarterModel.UISystemController.PausePanelController);
        }
    }
}