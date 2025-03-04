using System.Collections.Generic;
using UnityEngine;

namespace Runner.UI
{
    public class TranslateController : MonoBehaviour
    {
        [SerializeField] private List<TranslatableText> _translatableTexts = new List<TranslatableText>();

        public void TranslateTexts(string languageID)
        {
            for(int i = 0; i < _translatableTexts.Count; i++)
            {
                _translatableTexts[i].SetText(languageID);
            }
        }
    }
}