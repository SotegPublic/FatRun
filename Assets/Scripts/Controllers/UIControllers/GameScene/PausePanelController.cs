using Runner.Core;
using Runner.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelController : MonoBehaviour, IUpdateble
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _speedModText;

    [SerializeField] private Button _speedUpButton;
    [SerializeField] private Button _speedDownButton;
    [SerializeField] private Button _speedUpX10Button;
    [SerializeField] private Button _speedDownX10Button;

    [SerializeField] private TMP_Text _upCost;
    [SerializeField] private TMP_Text _up5Cost;
    [SerializeField] private TMP_Text _downCost;
    [SerializeField] private TMP_Text _down5Cost;

    [SerializeField] private Image _upImage;
    [SerializeField] private Image _up5Image;
    [SerializeField] private Image _downImage;
    [SerializeField] private Image _down5Image;

    [SerializeField] private TMP_Text _upCurrencyCode;
    [SerializeField] private TMP_Text _up5CurrencyCode;
    [SerializeField] private TMP_Text _downCurrencyCode;
    [SerializeField] private TMP_Text _down5CurrencyCode;

    private PlayerGameModel _playerGameModel;
    private YandexFunctions _yandexFunctions;
    private bool _isAwaitCurTexture;

    public void InitController(PlayerGameModel playerGameModel, YandexFunctions yandexFunctions)
    {
        _playerGameModel = playerGameModel;
        _yandexFunctions = yandexFunctions;

        _upImage.enabled = false;
        _up5Image.enabled = false;
        _downImage.enabled = false;
        _down5Image.enabled = false;

        _playerGameModel.OnPurchaseDone += UpdateUIAfterPurchase;
        _speedUpButton.onClick.AddListener(SpeedUpClick);
        _speedDownButton.onClick.AddListener(SpeedDownClick);
        _speedUpX10Button.onClick.AddListener(SpeedUpX10Click);
        _speedDownX10Button.onClick.AddListener(SpeedDownX10Click);

        _upCost.text = _yandexFunctions.GetCost("speedUp");
        _up5Cost.text = _yandexFunctions.GetCost("speedUpx10");
        _downCost.text = _yandexFunctions.GetCost("slowDown");
        _down5Cost.text = _yandexFunctions.GetCost("slowDownx10");

        _upCurrencyCode.text = _yandexFunctions.GetCode("speedUp");
        _up5CurrencyCode.text = _yandexFunctions.GetCode("speedUpx10");
        _downCurrencyCode.text = _yandexFunctions.GetCode("slowDown");
        _down5CurrencyCode.text = _yandexFunctions.GetCode("slowDownx10");

        _isAwaitCurTexture = true;
    }

    public void LocalUpdate(float deltaTime)
    {
        if (_isAwaitCurTexture)
        {
            if (_yandexFunctions.IsTextureLoaded)
            {
                var curSprite = Sprite.Create(_yandexFunctions.CurrencyTexture, new Rect(0, 0, _yandexFunctions.CurrencyTexture.width, _yandexFunctions.CurrencyTexture.height), _upImage.rectTransform.pivot);

                _upImage.enabled = true;
                _upImage.sprite = curSprite;

                _up5Image.enabled = true;
                _up5Image.sprite = curSprite;

                _downImage.enabled = true;
                _downImage.sprite = curSprite;

                _down5Image.enabled = true;
                _down5Image.sprite = curSprite;

                _upCurrencyCode.enabled = false;
                _up5CurrencyCode.enabled = false;
                _downCurrencyCode.enabled = false;
                _down5CurrencyCode.enabled = false;

                _isAwaitCurTexture = false;
            }
        }
    }

    private void UpdateUIAfterPurchase()
    {
        _speedModText.text = "x" + Math.Round(_playerGameModel.SpeedModifier, 1).ToString();

        UpdateInteractableSpeedDownButton();
    }

    public void ShowCanvas()
    {
        _canvasGroup.ShowCanvasGroup();
    }
    
    public void HideCanvas()
    {
        _canvasGroup.HideCanvasGroup();
    }

    private void SpeedUpClick()
    {
        _yandexFunctions.Buy(true);
    }

    private void SpeedDownClick()
    {
        _yandexFunctions.Buy(false);
    }

    private void SpeedUpX10Click()
    {
        _yandexFunctions.BuyX10(true);
    }

    private void SpeedDownX10Click()
    {
        _yandexFunctions.BuyX10(false);
    }

    private void UpdateInteractableSpeedDownButton()
    {
        if (_playerGameModel.SpeedModifier == 1)
        {
            _speedDownButton.interactable = false;
            _speedDownX10Button.interactable = false;
        }
        else if (_playerGameModel.SpeedModifier > 1 && _playerGameModel.SpeedModifier < 2)
        {
            _speedDownButton.interactable = true;
            _speedDownX10Button.interactable = false;
        }
        else
        {
            _speedDownButton.interactable = true;
            _speedDownX10Button.interactable = true;
        }
    }

    public void Dispose()
    {
        _playerGameModel.OnPurchaseDone -= UpdateUIAfterPurchase;
        _speedUpButton.onClick.RemoveListener(SpeedUpClick);
        _speedDownButton.onClick.RemoveListener(SpeedDownClick);
        _speedUpX10Button.onClick.RemoveListener(SpeedUpX10Click);
        _speedDownX10Button.onClick.RemoveListener(SpeedDownX10Click);
    }
}
