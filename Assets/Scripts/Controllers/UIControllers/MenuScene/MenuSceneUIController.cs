using Runner.Core;
using Runner.Player;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Runner.UI
{
    public class MenuSceneUIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
        [SerializeField] private CanvasGroup _creditsRuCanvasGroup;
        [SerializeField] private CanvasGroup _creditsEnCanvasGroup;
        [SerializeField] private CanvasGroup _creditsTrCanvasGroup;
        [SerializeField] private CanvasGroup _loginCanvasGroup;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _realShopButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _creditsRuBackButton;
        [SerializeField] private Button _creditsEnBackButton;
        [SerializeField] private Button _creditsTrBackButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _showLoginPanelButton;
        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _loginPanelBackButton;
        [SerializeField] private ShopUIController _shopUIController;
        [SerializeField] private RealShopUIController _realShopUIController;
        [SerializeField] private SettingsUIController _settingsUIController;
        [SerializeField] private SoundController _soundController;
        [SerializeField] private TranslateController _translateController;

        private PlayerGameModel _playerModel;
        private YandexFunctions _yandexFunctions;
        private bool _isInitialized = false;
        private bool _isSendLoadInPlayerModel = false;

        private void Start()
        {
            _playerModel = FindObjectOfType<PlayerGameModel>();
            _yandexFunctions = FindObjectOfType<YandexFunctions>();

            _playerModel.OnLanguageChange += _translateController.TranslateTexts;
            _playerModel.OnAuthChange += ChangeLoginButtonAccess;
            _playerModel.OnReloadModel += ReInitControllers;

            _startGameButton.onClick.AddListener(StartGame);
            _shopButton.onClick.AddListener(ShowShop);
            _shopUIController.BackButton.onClick.AddListener(CloseShop);
            _realShopButton.onClick.AddListener(ShowRealShop);
            _realShopUIController.BackButton.onClick.AddListener(CloseRealShop);
            _creditsButton.onClick.AddListener(ShowCredits);
            _creditsRuBackButton.onClick.AddListener(CloseRuCredits);
            _creditsEnBackButton.onClick.AddListener(CloseEnCredits);
            _creditsTrBackButton.onClick.AddListener(CloseTrCredits);
            _showLoginPanelButton.onClick.AddListener(ShowLoginPanel);
            _loginButton.onClick.AddListener(Login);
            _loginPanelBackButton.onClick.AddListener(CloseLoginPanel);
            _settingsButton.onClick.AddListener(ShowSettings);
            _settingsUIController.ExitButton.onClick.AddListener(CloseSettings);
            _settingsUIController.SaveButton.onClick.AddListener(CloseSettings);
        }

        private void Update()
        {
            if (_isInitialized) return;

            if (_playerModel != null)
            {
                if(_playerModel.IsLoaded)
                {
                    if (_playerModel.IsAuthentificated)
                    {
                        _showLoginPanelButton.interactable = false;
                    }

                    InitControllers();

                    _soundController.PlayInMenu();
                    _mainMenuCanvasGroup.ShowCanvasGroup();
                    _translateController.TranslateTexts(_playerModel.LanguageID);

                    _isInitialized = true;
                }
                else
                {
                    if (!_playerModel.IsYNDXLoad)
                    {
                        _playerModel.CheckYandex();
                    }

                    if (_playerModel.IsYNDXLoad && !_isSendLoadInPlayerModel)
                    {
                        _playerModel.CheckAuthentification();

                        _isSendLoadInPlayerModel = true;
                    }
                }
            } 
        }

        private void InitControllers()
        {
            _shopUIController.InitController(_playerModel, true);
            _settingsUIController.InitController(_playerModel, true);
            _realShopUIController.InitController(_playerModel, _yandexFunctions, true);

            _yandexFunctions.CheckPurchases();
        }

        private void ReInitControllers()
        {
            _shopUIController.InitController(_playerModel, false);
            _settingsUIController.InitController(_playerModel, false);
            _realShopUIController.InitController(_playerModel, _yandexFunctions, false);

            _yandexFunctions.CheckPurchases();
        }

        public void ChangeLoginButtonAccess(bool isAuth)
        {
            if(isAuth)
            {
                _showLoginPanelButton.interactable = false;
            }
            else
            {
                _showLoginPanelButton.interactable = true;
            }
        }

        private void ShowLoginPanel()
        {
            _mainMenuCanvasGroup.HideCanvasGroup();
            _loginCanvasGroup.ShowCanvasGroup();
        }

        private void Login()
        {
            if(_playerModel != null)
            {
                _playerModel.Authentification();
                CloseLoginPanel();
            }
        }

        private void CloseLoginPanel()
        {
            _loginCanvasGroup.HideCanvasGroup();
            _mainMenuCanvasGroup.ShowCanvasGroup();
        }

        private void ShowSettings()
        {
            _mainMenuCanvasGroup.HideCanvasGroup();
            _settingsUIController.ShowSettingsCanvas();
        }

        private void CloseSettings()
        {
            _settingsUIController.HideSettingsCanvas();
            _mainMenuCanvasGroup.ShowCanvasGroup();
        }

        private void StartGame()
        {
            _soundController.Stop();
            _soundController.IsLevelRestarted = false;
            SceneManager.LoadScene("GameScene");
        }

        private void CloseShop()
        {
            _shopUIController.HideCanvas();
            _mainMenuCanvasGroup.ShowCanvasGroup();
        }

        private void CloseRealShop()
        {
            _realShopUIController.HideCanvas();
            _mainMenuCanvasGroup.ShowCanvasGroup();
        }

        private void ShowShop()
        {
            _mainMenuCanvasGroup.HideCanvasGroup();
            _shopUIController.ShowCanvas();
        }

        private void ShowRealShop()
        {
            _mainMenuCanvasGroup.HideCanvasGroup();
            _realShopUIController.ShowCanvas();
        }

        private void CloseRuCredits()
        {
            _mainMenuCanvasGroup.ShowCanvasGroup();
            _creditsRuCanvasGroup.HideCanvasGroup();
        }

        private void CloseEnCredits()
        {
            _mainMenuCanvasGroup.ShowCanvasGroup();
            _creditsEnCanvasGroup.HideCanvasGroup();
        }

        private void CloseTrCredits()
        {
            _mainMenuCanvasGroup.ShowCanvasGroup();
            _creditsTrCanvasGroup.HideCanvasGroup();
        }

        private void ShowCredits()
        {
            _mainMenuCanvasGroup.HideCanvasGroup();

            switch (_playerModel.LanguageID)
            {
                case "ru":
                case "be":
                case "kk":
                case "uk":
                case "uz":
                    _creditsRuCanvasGroup.ShowCanvasGroup();
                    break;
                case "tr":
                    _creditsTrCanvasGroup.ShowCanvasGroup();
                    break;
                case "en":
                default:
                    _creditsEnCanvasGroup.ShowCanvasGroup();
                    break;
            }
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveListener(StartGame);
            _shopButton.onClick.RemoveListener(ShowShop);
            _shopUIController.BackButton.onClick.RemoveListener(CloseShop);
            _realShopButton.onClick.RemoveListener(ShowRealShop);
            _realShopUIController.BackButton.onClick.RemoveListener(CloseRealShop);
            _creditsButton.onClick.RemoveListener(ShowCredits);
            _creditsRuBackButton.onClick.RemoveListener(CloseRuCredits);
            _creditsEnBackButton.onClick.RemoveListener(CloseEnCredits);
            _creditsTrBackButton.onClick.RemoveListener(CloseTrCredits);
            _settingsButton.onClick.RemoveListener(ShowSettings);
            _settingsUIController.ExitButton.onClick.RemoveListener(CloseSettings);
            _settingsUIController.SaveButton.onClick.RemoveListener(CloseSettings);
            _loginButton.onClick.RemoveListener(Login);
            _loginPanelBackButton.onClick.RemoveListener(CloseLoginPanel);
            _settingsButton.onClick.RemoveListener(ShowSettings);

            _playerModel.OnLanguageChange -= _translateController.TranslateTexts;
            _playerModel.OnAuthChange -= ChangeLoginButtonAccess;
            _playerModel.OnReloadModel -= ReInitControllers;

            _shopUIController.Dispose();
            _settingsUIController.Dispose();
            _realShopUIController.Dispose();

            _playerModel.IsYNDXLoad = false;
        }
    }
}