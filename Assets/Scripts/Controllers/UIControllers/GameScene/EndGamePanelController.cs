using Runner.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI
{
    public class EndGamePanelController: MonoBehaviour
    {
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected TMP_Text _getPointsText;
        [SerializeField] protected Button _restartButton;
        [SerializeField] protected Button _returnButton;

        public Button RestartButton => _restartButton;
        public Button ReturnButton => _returnButton;

        public virtual void Init()
        {
            _restartButton.onClick.AddListener(SetNotInteractableControllerButtons);
            _returnButton.onClick.AddListener(SetNotInteractableControllerButtons);
        }

        public void SetGotPointText(int pointsGot, int pointsNeed)
        {
            _getPointsText.text = pointsGot.ToString() + "/" + pointsNeed.ToString();
        }

        public void ShowCanvas()
        {
            _canvas.enabled = true;
        }

        public virtual void SetNotInteractableControllerButtons()
        {
            _returnButton.interactable = false;
            _restartButton.interactable = false;
        }

        public virtual void Dispose()
        {
            _restartButton.onClick.RemoveListener(SetNotInteractableControllerButtons);
            _returnButton.onClick.RemoveListener(SetNotInteractableControllerButtons);
        }
    }
}