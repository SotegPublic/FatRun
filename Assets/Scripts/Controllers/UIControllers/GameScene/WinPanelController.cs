using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI
{
    public class WinPanelController : EndGamePanelController
    {

        [SerializeField] private TMP_Text _bonusText;
        [SerializeField] private Button _doubleButton;

        public Button DoubleButton => _doubleButton;

        public override void Init()
        {
            base.Init();
            _doubleButton.onClick.AddListener(SetNotInteractableControllerButtons);
        }

        public void SetBonusText(int bonusPoints)
        {
            _bonusText.text = bonusPoints.ToString();
        }

        public override void SetNotInteractableControllerButtons()
        {
            base.SetNotInteractableControllerButtons();
            _doubleButton.interactable = false;
        }

        public void ReturnInteractableControllerButtons(bool isRewardDoubled)
        {
            if(!isRewardDoubled)
            {
                _doubleButton.interactable = true;
            }
            _restartButton.interactable = true;
            _returnButton.interactable = true;
        }

        public override void Dispose()
        {
            base.Dispose();
            _doubleButton.onClick.RemoveListener(SetNotInteractableControllerButtons);
        }
    }
}