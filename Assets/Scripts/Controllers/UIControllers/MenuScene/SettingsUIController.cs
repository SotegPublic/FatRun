using Runner.Core;
using Runner.UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIController : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Button _rusButton;
    [SerializeField] private Button _enButton;
    [SerializeField] private Button _trButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private CanvasGroup _settingsCanvasGroup;
    [SerializeField] private Color _notEnebledColor;
    [SerializeField] private Image _rusButtonImage;
    [SerializeField] private Image _enButtonImage;
    [SerializeField] private Image _trButtonImage;

    private PlayerGameModel _playerGameModel;
    private float _newVolume;
    private string _newLanguage;

    public Button ExitButton => _exitButton;
    public Button SaveButton => _saveButton;

    public void InitController(PlayerGameModel playerGameModel, bool isFirstInit)
    {
        _playerGameModel = playerGameModel;

        _newLanguage = playerGameModel.LanguageID;
        _newVolume = playerGameModel.SoundVolume;

        _volumeSlider.value = playerGameModel.SoundVolume;
        AudioListener.volume = playerGameModel.SoundVolume;
        SetLanguage(_playerGameModel.LanguageID);

        if(isFirstInit)
        {
            Subscribe();
        }
    }

    private void Subscribe()
    {
        _rusButton.onClick.AddListener(SetRuLanguage);
        _enButton.onClick.AddListener(SetEnLanguage);
        _trButton.onClick.AddListener(SetTrLanguage);
        _volumeSlider.onValueChanged.AddListener(SetSoundVolume);

        _saveButton.onClick.AddListener(SaveSettings);
        _exitButton.onClick.AddListener(ResetSettings);
    }

    private void ResetSettings()
    {
        AudioListener.volume = _playerGameModel.SoundVolume;

        _newLanguage = _playerGameModel.LanguageID;
        _newVolume = _playerGameModel.SoundVolume;

        SetLanguage(_playerGameModel.LanguageID);
    }

    private void SaveSettings()
    {
        _playerGameModel.SetNewLanguage(_newLanguage);
        _playerGameModel.SetNewSoundVolume(_newVolume);
        _playerGameModel.SavePlayerData();
    }



    private void SetSoundVolume(float newVolumeValue)
    {
        _newVolume = newVolumeValue;
        AudioListener.volume = newVolumeValue;
    }

    private void SetEnLanguage()
    {
        SetLanguage("en");
    }

    private void SetRuLanguage()
    {
        SetLanguage("ru");
    }

    private void SetTrLanguage()
    {
        SetLanguage("tr");
    }

    public void ShowSettingsCanvas()
    {
        _settingsCanvasGroup.ShowCanvasGroup();
    }

    public void HideSettingsCanvas()
    {
        _settingsCanvasGroup.HideCanvasGroup();
    }

    private void SetLanguage(string langageString)
    {
        _newLanguage = langageString;

        switch (langageString)
        {
            case "ru":
            case "be":
            case "kk":
            case "uk":
            case "uz":
                _enButtonImage.color = _notEnebledColor;
                _rusButtonImage.color = Color.white;
                _trButtonImage.color = _notEnebledColor;
                break;
            case "tr":
                _enButtonImage.color = _notEnebledColor;
                _rusButtonImage.color = _notEnebledColor;
                _trButtonImage.color = Color.white;
                break;
            case "en":
            default:
                _rusButtonImage.color = _notEnebledColor;
                _enButtonImage.color = Color.white;
                _trButtonImage.color = _notEnebledColor;
                break;
        }
    }

    public void Dispose()
    {
        _rusButton.onClick.RemoveListener(SetRuLanguage);
        _enButton.onClick.RemoveListener(SetEnLanguage);
        _trButton.onClick.RemoveListener(SetTrLanguage);
        _volumeSlider.onValueChanged.RemoveListener(SetSoundVolume);

        _saveButton.onClick.RemoveListener(SaveSettings);
        _exitButton.onClick.RemoveListener(ResetSettings);
    }
}
