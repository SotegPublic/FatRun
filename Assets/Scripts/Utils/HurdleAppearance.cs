using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.BonusSystem
{
    public class HurdleAppearance: MonoBehaviour
    {
        [SerializeField] private Image _canvasImage;
        [SerializeField] private Color _positiveColor;
        [SerializeField] private Color _negativeColor;
        [SerializeField] private TMP_Text _bonusText;

        public void InitHurdle(bool isPositive, int bonusValue)
        {
            _canvasImage.color = isPositive ? _positiveColor : _negativeColor;
            _bonusText.text = bonusValue > 0 ? ("+" + bonusValue.ToString()) : bonusValue.ToString();
        }
    }
}