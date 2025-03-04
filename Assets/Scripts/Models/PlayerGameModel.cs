using Runner.UI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Runner.Core
{
    public class PlayerGameModel : MonoBehaviour
    {
        public Action OnRewardDoubled;
        public Action OnRewardVideoClosed;
        public Action OnAdvBeforeMainMenuClosed;
        public Action OnPurchaseDone;
        public Action OnReloadModel;
        public Action<bool> OnAuthChange;
        public Action<string> OnLanguageChange;

        [SerializeField] private GameObject _modelHolder;
        [SerializeField] private int _runAnimationID;
        [SerializeField] private int _kickAnimationID;
        [SerializeField] private int _danceAnimationID;
        [SerializeField] private GameObject _runnerModel;
        [SerializeField] private int _runnerModelID;
        [SerializeField] private float _soundVolume;
        [SerializeField] private string _languageID;
        [SerializeField] private float _speedModifierStep = 0.2f;

        private bool _isNeedReinitControllers = false;
        private bool _isAuthentificated = false;
        private bool _isLoaded = false;
        private bool _isModelInited = false;
        private PlayerCurrency _playerCurrency;
        private List<int> _buyedItemsIDs = new List<int>();
        private DateTime _lastAdvTime;
        private float _speedModifier = 1f;
        private int _winStreak;
        private int _maxWinStreak;
        private string _deviceID;

        public bool IsYNDXLoad { get; set; }
        public bool IsAuthentificated => _isAuthentificated;
        public PlayerCurrency PlayerCurrency => _playerCurrency;
        public int RunAnimationID => _runAnimationID;
        public int KickAnimationID => _kickAnimationID;
        public int DanceAnimationID => _danceAnimationID;
        public GameObject RunnerModel => _runnerModel;
        public int RunnerModelID => _runnerModelID;
        public float SoundVolume => _soundVolume;
        public string LanguageID => _languageID;
        public List<int> BuyedItemsIDs => _buyedItemsIDs;
        public bool IsModelInited => _isModelInited;
        public bool IsLoaded => _isLoaded;
        public DateTime LastAdvTime => _lastAdvTime;
        public float SpeedModifier => _speedModifier;
        public int WinStreak => _winStreak;
        public int MaxWinStreak => _maxWinStreak;
        public string DeviceID => _deviceID;

        [DllImport("__Internal")]
        private static extern void SaveExtern(string playerDate);

        [DllImport("__Internal")]
        private static extern void SaveToLocalStorage(string key, string playerDate);

        [DllImport("__Internal")]
        private static extern int HasKey(string key);

        [DllImport("__Internal")]
        public static extern string LoadFromLocalStorage(string key);

        [DllImport("__Internal")]
        private static extern void LoadExtern();

        [DllImport("__Internal")]
        private static extern int CheckYandexSDK();

        [DllImport("__Internal")]
        private static extern void Loaded(string language, string device);

        [DllImport("__Internal")]
        private static extern void SetLeaderboardInfo(int winstreak);

        [DllImport("__Internal")]
        private static extern string GetLanguage();

        [DllImport("__Internal")]
        private static extern string GetDeviceType();

        [DllImport("__Internal")]
        private static extern void SendReady();

        [DllImport("__Internal")]
        private static extern void YaAuth();

        [DllImport("__Internal")]
        private static extern void CheckAuth();


        private void Awake()
        {
            DontDestroyOnLoad(_modelHolder);
            _playerCurrency = new PlayerCurrency();
            _lastAdvTime = new DateTime(2000, 1, 1, 1, 1, 1, 1);
        }

        #region Auth
        public void Authentification()
        {
            YaAuth();
            _isNeedReinitControllers = true;
        }

        public void AfterAuthCheck(int value)
        {
            _isAuthentificated = value == 1 ? true : false;

            OnAuthChange?.Invoke(_isAuthentificated);

            if (_isAuthentificated)
            {
                LoadExtern();
            }
            else
            {
                LoadLocal();
            }
        }

        public void CheckAuthentification()
        {
            CheckAuth();
        }

        public void CheckYandex()
        {
            IsYNDXLoad = CheckYandexSDK() == 1 ? true : false;
        }

        #endregion

        #region SaveAndLoad
        private void LoadLocal()
        {
            if (HasKey("localSave") == 1)
            {
                var saveModelStr = LoadFromLocalStorage("localSave");
                LoadPlayerData(saveModelStr);
            }
            else
            {
                _soundVolume = 1f;
                _languageID = GetLanguage();
                _speedModifier = 1f;

                _isModelInited = true;

                SavePlayerData();
                EndLoad();
            }
        }

        private void LoadPlayerData(string jsonString)
        {
            var playerSaveModel = JsonUtility.FromJson<PlayerSaveModel>(jsonString);

            if (!playerSaveModel.IsModelInited)
            {
                _soundVolume = 1f;
                _languageID = GetLanguage();
                _speedModifier = 1f;

                _isModelInited = true;

                SavePlayerData();
            }
            else
            {
                _soundVolume = playerSaveModel.SoundVolume;
                _isModelInited = playerSaveModel.IsModelInited;
                _languageID = playerSaveModel.LanguageID;
                _speedModifier = playerSaveModel.SpeedModifier == 0 ? 1f : playerSaveModel.SpeedModifier;
                _winStreak = playerSaveModel.WinStreak;
                _maxWinStreak = playerSaveModel.MaxWinStreak;
                _playerCurrency.SetCurrency(playerSaveModel.PlayerCurrency);
                _runAnimationID = playerSaveModel.RunAnimationID;
                _danceAnimationID = playerSaveModel.DanceAnimationID;
                _kickAnimationID = playerSaveModel.KickAnimationID;
                _runnerModelID = playerSaveModel.RunnerModelID;
                _buyedItemsIDs = playerSaveModel.BuyedItemsIDs;

                OnLanguageChange?.Invoke(playerSaveModel.LanguageID);
            }

            EndLoad();
        }

        private void EndLoad()
        {
            _deviceID = GetDeviceType();

            if (!_isLoaded)
            {
                SendReady();
                SetQuality();
            }

            _isLoaded = true;
            Loaded(_languageID, _deviceID);

            if(_isNeedReinitControllers)
            {
                OnReloadModel?.Invoke();
                _isNeedReinitControllers = false;
            }
        }

        public void SavePlayerData()
        {
            var newSaveModel = new PlayerSaveModel(this);

            string jsonString = JsonUtility.ToJson(newSaveModel);

            if (_isAuthentificated)
            {
                SaveExtern(jsonString);
            }
            else
            {
                SaveToLocalStorage("localSave", jsonString);
            }
        }

        #endregion

        public void SetQuality()
        {
            if (_deviceID == "desktop")
            {
                Screen.SetResolution(1080, 1920, true);

                string[] names = QualitySettings.names;

                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] == "High Fidelity")
                    {
                        QualitySettings.SetQualityLevel(i, true);
                    }
                }
            }
            else
            {
                Screen.SetResolution(720, 1280, true);
                string[] names = QualitySettings.names;

                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] == "Mobile")
                    {
                        QualitySettings.SetQualityLevel(i, true);
                    }
                }
            }
        }

        public void SetNewModel(GameObject runnerModel, int itemID = -1)
        {
            _runnerModel = runnerModel;
            
            if(itemID != -1)
            {
                _runnerModelID = itemID;
            }
        }

        public void UpSpeedWhenWin()
        {
            _speedModifier = (float)Math.Round(_speedModifier + _speedModifierStep, 1);
            SavePlayerData();
        }

        public void UpSpeed()
        {
            _speedModifier = (float)Math.Round(_speedModifier + _speedModifierStep, 1);
            SavePlayerData();
            OnPurchaseDone?.Invoke();
        }

        public void DownSpeed()
        {
            _speedModifier = (float)Math.Round(_speedModifier - _speedModifierStep, 1) < 1 ? 1 : (float)Math.Round(_speedModifier - _speedModifierStep, 1);
            SavePlayerData();
            OnPurchaseDone?.Invoke();
        }

        public void UpSpeedX5()
        {
            _speedModifier = (float)Math.Round(_speedModifier + (_speedModifierStep * 5), 1);
            SavePlayerData();
            OnPurchaseDone?.Invoke();
        }

        public void DownSpeedX5()
        {
            _speedModifier = (float)Math.Round(_speedModifier - (_speedModifierStep * 5), 1) < 1 ? 1 : (float)Math.Round(_speedModifier - (_speedModifierStep * 5), 1);
            SavePlayerData();
            OnPurchaseDone?.Invoke();
        }

        public void Get1000FatCoins()
        {
            _playerCurrency.AddCurrency(1000);
            SavePlayerData();
            OnPurchaseDone?.Invoke();
        }

        public void IncreaseWinStreak()
        {
            _winStreak++;

            if(_winStreak >= _maxWinStreak)
            {
                _maxWinStreak = _winStreak;
                SetLeaderboardInfo(_maxWinStreak);
            }
        }

        public void ResetWinStreak()
        {
            _winStreak = 0;
        }

        public void ResetSpeed()
        {
            _speedModifier = 1f;
            SavePlayerData();
        }

        public void SetAnimationID(int animationID, AnimationTypes animationType)
        {
            switch (animationType)
            {
                case AnimationTypes.Run:
                    _runAnimationID = animationID;
                    break;
                case AnimationTypes.Kick:
                    _kickAnimationID = animationID;
                    break;
                case AnimationTypes.Dance:
                    _danceAnimationID = animationID;
                    break;
                case AnimationTypes.None:
                default:
                    break;
            }
        }

        public void SetLastAdvTime(DateTime dateTime)
        {
            _lastAdvTime = dateTime;
        }

        public void AddBuyedItem(int itemID)
        {
            BuyedItemsIDs.Add(itemID);
        }

        public bool IsItemBuyed(int itemID)
        {
            var idIndex = BuyedItemsIDs.FindIndex(x => x == itemID);

            if(idIndex == -1) return false;
            else return true;
        }

        public void SetNewSoundVolume(float volume)
        {
            _soundVolume = volume;
        }

        public void SetSettingsModelInited()
        {
            _isModelInited = true;
        }

        public void SetNewLanguage(string languageID)
        {
            _languageID = languageID;
            OnLanguageChange?.Invoke(languageID);
        }

        private void ReturnSound()
        {
            AudioListener.volume = _soundVolume;
        }

        private void AdvBeforeMainMenuClosed()
        {
            ReturnSound();
            OnAdvBeforeMainMenuClosed?.Invoke();
        }

        private void DoubleReward(int reward)
        {
            _playerCurrency.AddCurrency(reward);
            SavePlayerData();
            OnRewardDoubled?.Invoke();
        }

        private void WhenRewardVideoClosed()
        {
            ReturnSound();
            OnRewardVideoClosed?.Invoke();
        }

        private void OnDestroy()
        {
            SavePlayerData();
        }
    }
}