using TMPro;
using UnityEngine;

namespace Runner.UI
{
    public class TranslatableText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _rusText;
        [SerializeField] private string _enText;
        [SerializeField] private string _trText;

        public void SetRusText(string text)
        {
            _rusText = text;
        }

        public void SetEnText(string text)
        {
            _enText = text;
        }

        public void SetText(string languageID)
        {
            switch (languageID)
            {
                case "ru":
                case "be":
                case "kk":
                case "uk":
                case "uz":
                    _text.text = _rusText;
                    break;
                case "tr":
                    _text.text = _trText;
                    break;
                case "en":
                default:
                    _text.text = _enText;
                    break;
            }
        }
    }
}