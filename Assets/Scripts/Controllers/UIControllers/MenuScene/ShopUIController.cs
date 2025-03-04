using Runner.Core;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI
{
    public class ShopUIController : MonoBehaviour
    {
        private Action OnSelectedButtonChange;

        [SerializeField] private CanvasGroup _shopCanvasGroup;
        [SerializeField] private Transform _itemsHolder;
        [SerializeField] private Transform _svContentTransform;

        [Header("Buttons")]
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _useButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _modelsButton;
        //[SerializeField] private Button _danceAnimatonsButton;
        [SerializeField] private Button _runAnimationsButton;
        [SerializeField] private Button _kickAnimationsButton;

        [Header("Currensy panel")]
        [SerializeField] private TMP_Text _currensyText;

        [Header("Preview Panel")]
        [SerializeField] private TMP_Text _previewTitle;
        [SerializeField] private TMP_Text _previewDiscription;
        [SerializeField] private Color _noSelectItemColor;
        [SerializeField] private Color _selectItemColor;

        [Header("Lists")]
        [SerializeField] private List<ModelShopButton> _modelShopButtons = new List<ModelShopButton>();
        [SerializeField] private List<AnimationShopButton> _danceAnimationsShopButtons = new List<AnimationShopButton>();
        [SerializeField] private List<AnimationShopButton> _runAnimationsShopButtons = new List<AnimationShopButton>();
        [SerializeField] private List<AnimationShopButton> _kickAnimationsShopButtons = new List<AnimationShopButton>();

        private PlayerGameModel _playerGameModel;
        private ModelShopButton _selectedModelButton;
        private AnimationShopButton _selectedAnimationShopButton;
        private List<ShopItemButton> _currentShopList = new List<ShopItemButton>();

        public Button BackButton => _backButton;

        public List<ModelShopButton> ModelShopButtons => _modelShopButtons;
        public List<AnimationShopButton> DanceAnimationsShopButtons => _danceAnimationsShopButtons;
        public List<AnimationShopButton> RunAnimationsShopButtons => _runAnimationsShopButtons;
        public List<AnimationShopButton> KickAnimationsShopButtons => _kickAnimationsShopButtons;

        public void HideCanvas()
        {
            _shopCanvasGroup.HideCanvasGroup();
            ChangeSelectedModelButton(null);
            ChangeSelectedAnimationButton(null);

            _previewDiscription.text = "";
            _previewTitle.text = "";
        }

        public void ShowCanvas()
        {
            _shopCanvasGroup.ShowCanvasGroup();
            ShowModelsStore();
        }

        public void InitController(PlayerGameModel playerModel, bool isFirstInit)
        {
            _playerGameModel = playerModel;
            _currensyText.text = _playerGameModel.PlayerCurrency.Value.ToString();

            _useButton.interactable = false;
            _buyButton.interactable = false;

            InitAndSubscribeOnModelsShopButtons(isFirstInit);
            InitAndSubscribeOnAnimationsShopButtons(_danceAnimationsShopButtons, AnimationTypes.Dance, isFirstInit);
            InitAndSubscribeOnAnimationsShopButtons(_kickAnimationsShopButtons, AnimationTypes.Kick, isFirstInit);
            InitAndSubscribeOnAnimationsShopButtons(_runAnimationsShopButtons, AnimationTypes.Run, isFirstInit);

            _previewDiscription.text = "";
            _previewTitle.text = "";

            if(isFirstInit)
            {
                Subscribe();
            }
        }

        private void Subscribe()
        {
            _playerGameModel.PlayerCurrency.Subscribe(UpdateCurrencyCount);

            OnSelectedButtonChange += CheckButtonsInterectability;

            _modelsButton.onClick.AddListener(ShowModelsStore);
            //_danceAnimatonsButton.onClick.AddListener(ShowDanceAnimationsStore);
            _runAnimationsButton.onClick.AddListener(ShowRunAnimationsStore);
            _kickAnimationsButton.onClick.AddListener(ShowKickAnimationsStore);

            _useButton.onClick.AddListener(ApplyShopItem);
            _buyButton.onClick.AddListener(BuyItem);
        }

        private void BuyItem()
        {
            int itemCost;
            int itemID;

            if (_selectedModelButton != null)
            {
                itemCost = _selectedModelButton.BuyItem();
                itemID = _selectedModelButton.ItemID;
            }
            else
            {
                itemCost = _selectedAnimationShopButton.BuyItem();
                itemID = _selectedAnimationShopButton.ItemID;
            }

            _playerGameModel.PlayerCurrency.SubtractCurrency(itemCost);
            _playerGameModel.AddBuyedItem(itemID);

            ApplyShopItem();
            CheckButtonsInterectability();
        }

        private void ApplyShopItem()
        {
            if(_selectedModelButton != null)
            {
                for(int i = 0; i < _modelShopButtons.Count; i++)
                {
                    if(_playerGameModel.RunnerModelID == _modelShopButtons[i].ItemID)
                    {
                        _modelShopButtons[i].SetUsed(false);
                    }
                }
                
                _playerGameModel.SetNewModel(_selectedModelButton.ModelPrefab, _selectedModelButton.ItemID);
                _selectedModelButton.SetUsed(true);
            }
            if(_selectedAnimationShopButton != null)
            {
                switch (_selectedAnimationShopButton.AnimationType)
                {
                    case AnimationTypes.Run:
                        SwitchOffUsedMarkOnOldButton(_runAnimationsShopButtons, _playerGameModel.RunAnimationID);
                        break;
                    case AnimationTypes.Kick:
                        SwitchOffUsedMarkOnOldButton(_kickAnimationsShopButtons, _playerGameModel.KickAnimationID);
                        break;
                    case AnimationTypes.Dance:
                    case AnimationTypes.None:
                    default:
                        break;
                }

                _playerGameModel.SetAnimationID(_selectedAnimationShopButton.AnimationID, _selectedAnimationShopButton.AnimationType);
                _selectedAnimationShopButton.SetUsed(true);
            }

            _playerGameModel.SavePlayerData();
        }

        private void SwitchOffUsedMarkOnOldButton(List<AnimationShopButton> animationShopButtons, int id)
        {
            for (int i = 0; i < animationShopButtons.Count; i++)
            {
                if (id == animationShopButtons[i].AnimationID)
                {
                    animationShopButtons[i].SetUsed(false);
                }
            }
        }

        private void InitAndSubscribeOnModelsShopButtons(bool isFirstInit)
        {
            for (int i = 0; i < _modelShopButtons.Count; i++)
            {
                if (_modelShopButtons[i].IsBaseItem)
                {
                    _modelShopButtons[i].InitItem(true);
                }
                else
                {
                    _modelShopButtons[i].InitItem(false);
                }

                if(isFirstInit)
                {
                    _modelShopButtons[i].OnModelShopButtonClick += WhenSelectModelButton;
                }

                if(_playerGameModel.IsItemBuyed(_modelShopButtons[i].ItemID))
                {
                    _modelShopButtons[i].BuyItem();
                }

                if(_playerGameModel.RunnerModelID == _modelShopButtons[i].ItemID)
                {
                    _playerGameModel.SetNewModel(_modelShopButtons[i].ModelPrefab);
                    _modelShopButtons[i].SetUsed(true);
                }
            }
        }

        private void InitAndSubscribeOnAnimationsShopButtons(List<AnimationShopButton> animationShopButtons, AnimationTypes animationType, bool isFirstInit)
        {
            for (int i = 0; i < animationShopButtons.Count; i++)
            {
                if (animationShopButtons[i].IsBaseItem)
                {
                    animationShopButtons[i].InitItem(true);
                }
                else
                {
                    animationShopButtons[i].InitItem(false);
                }

                if (isFirstInit)
                {
                    animationShopButtons[i].OnAnimationShopButtonClick += WhenSelectAnimationButton;
                }

                if (_playerGameModel.IsItemBuyed(animationShopButtons[i].ItemID))
                {
                    animationShopButtons[i].BuyItem();
                }

                switch (animationType)
                {
                    case AnimationTypes.Kick:

                        if (_playerGameModel.KickAnimationID == animationShopButtons[i].AnimationID)
                        {
                            animationShopButtons[i].SetUsed(true);
                        }

                        break;
                    case AnimationTypes.Run:

                        if(_playerGameModel.RunAnimationID == animationShopButtons[i].AnimationID)
                        {
                            animationShopButtons[i].SetUsed(true);
                        }

                        break;
                    case AnimationTypes.Dance:
                    case AnimationTypes.None:
                    default:
                        break;
                }
            }
        }

        private void UpdateCurrencyCount(int value)
        {
            _currensyText.text = value.ToString();
        }

        private void ShowKickAnimationsStore()
        {
            ChangeSelectedModelButton(null);
            ChangeSelectedAnimationButton(null);

            ClearCurrentShopItems();
            UpdateAnimationsShopItems(_kickAnimationsShopButtons);
        }

        private void ShowRunAnimationsStore()
        {
            ChangeSelectedModelButton(null);
            ChangeSelectedAnimationButton(null);

            ClearCurrentShopItems();
            UpdateAnimationsShopItems(_runAnimationsShopButtons);
        }

        private void ShowDanceAnimationsStore()
        {
            ChangeSelectedModelButton(null);
            ChangeSelectedAnimationButton(null);

            ClearCurrentShopItems();
            UpdateAnimationsShopItems(_danceAnimationsShopButtons);
        }

        private void UpdateAnimationsShopItems(List<AnimationShopButton> animationsShopButtons)
        {
            for (int i = 0; i < animationsShopButtons.Count; i++)
            {
                animationsShopButtons[i].Transform.position = Vector3.zero;
                animationsShopButtons[i].Transform.SetParent(_svContentTransform);
                animationsShopButtons[i].Transform.localScale = Vector3.one;
                _currentShopList.Add(animationsShopButtons[i]);
            }
        }

        private void ShowModelsStore()
        {
            ChangeSelectedModelButton(null);
            ChangeSelectedAnimationButton(null);

            ClearCurrentShopItems();

            for (int i = 0; i < _modelShopButtons.Count; i++)
            {
                _modelShopButtons[i].Transform.position = Vector3.zero;
                _modelShopButtons[i].Transform.SetParent(_svContentTransform);
                _modelShopButtons[i].Transform.localScale = Vector3.one;
                _currentShopList.Add(_modelShopButtons[i]);
            }
        }

        private void ClearCurrentShopItems()
        {
            for (int i = 0; i < _currentShopList.Count; i++)
            {
                _currentShopList[i].Transform.position = Vector3.zero;
                _currentShopList[i].Transform.SetParent(_itemsHolder);
            }

            _currentShopList.Clear();
            _previewDiscription.text = "";
            _previewTitle.text = "";
        }

        private void WhenSelectModelButton(ModelShopButton button)
        {
            if (_selectedModelButton != null && _selectedModelButton == button)
            {
                button.DeselectButton();
                ClearPeviewPanel();
                ChangeSelectedModelButton(null);
            }
            else
            {
                button.SelectButton();
                UpdatePreviewPanel(button);
                ChangeSelectedModelButton(button);
            }
        }

        private void WhenSelectAnimationButton(AnimationShopButton button)
        {
            if (_selectedAnimationShopButton != null && _selectedAnimationShopButton == button)
            {
                button.DeselectButton();
                ClearPeviewPanel();
                ChangeSelectedAnimationButton(null);
            }
            else
            {
                button.SelectButton();
                UpdatePreviewPanel(button);
                ChangeSelectedAnimationButton(button);
            }
        }

        private void ClearPeviewPanel()
        {
            _previewDiscription.text = "";
            _previewTitle.text = "";
        }

        private void UpdatePreviewPanel(ShopItemButton button)
        {
            switch (_playerGameModel.LanguageID)
            {
                case "ru":
                case "be":
                case "kk":
                case "uk":
                case "uz":
                    _previewDiscription.text = button.RuItemDiscription;
                    _previewTitle.text = button.RuItemName;
                    break;
                case "tr":
                    _previewDiscription.text = button.TrItemDiscription;
                    _previewTitle.text = button.TrItemName;
                    break;
                case "en":
                default:
                    _previewDiscription.text = button.EnItemDiscription;
                    _previewTitle.text = button.EnItemName;
                    break;
            }
        }


        private void ChangeSelectedModelButton(ModelShopButton button)
        {
            _selectedModelButton = button;
            OnSelectedButtonChange?.Invoke();
        }

        private void ChangeSelectedAnimationButton(AnimationShopButton button)
        {
            _selectedAnimationShopButton = button;
            OnSelectedButtonChange?.Invoke();
        }

        private void CheckButtonsInterectability()
        {
            if(_selectedAnimationShopButton != null || _selectedModelButton != null)
            {
                if(_selectedAnimationShopButton == null)
                {
                    CheckShopItemParameters(_selectedModelButton);
                }
                else
                {
                    CheckShopItemParameters(_selectedAnimationShopButton);
                }
            }
            else
            {
                _buyButton.interactable = false;
                _useButton.interactable = false;
            }
        }

        private void CheckShopItemParameters(ShopItemButton shopItem)
        {
            if (shopItem.IsBuyed)
            {
                _buyButton.interactable = false;
                _useButton.interactable = !_buyButton.interactable;
            }
            else
            {
                if (shopItem.ItemPrice <= _playerGameModel.PlayerCurrency.Value)
                {
                    _buyButton.interactable = true;
                    _useButton.interactable = !_buyButton.interactable;
                }
                else
                {
                    _buyButton.interactable = false;
                    _useButton.interactable = false;
                }
            }
        }

        private void UnsubscribeOnAnimationShopButton(List<AnimationShopButton> animationShopButtons)
        {
            for (int i = 0; i < animationShopButtons.Count; i++)
            {
                animationShopButtons[i].OnAnimationShopButtonClick -= WhenSelectAnimationButton;
            }
        }

        public void Dispose()
        {
            _playerGameModel.PlayerCurrency.Unsubscribe(UpdateCurrencyCount);

            for (int i = 0; i < _modelShopButtons.Count; i++)
            {
                _modelShopButtons[i].OnModelShopButtonClick -= WhenSelectModelButton;
            }

            UnsubscribeOnAnimationShopButton(_danceAnimationsShopButtons);
            UnsubscribeOnAnimationShopButton(_runAnimationsShopButtons);
            UnsubscribeOnAnimationShopButton(_kickAnimationsShopButtons);

            OnSelectedButtonChange -= CheckButtonsInterectability;

            _modelsButton.onClick.RemoveListener(ShowModelsStore);
            //_danceAnimatonsButton.onClick.RemoveListener(ShowDanceAnimationsStore);
            _runAnimationsButton.onClick.RemoveListener(ShowRunAnimationsStore);
            _kickAnimationsButton.onClick.RemoveListener(ShowKickAnimationsStore);

            _useButton.onClick.RemoveListener(ApplyShopItem);
            _buyButton.onClick.RemoveListener(BuyItem);
        }
    }
}