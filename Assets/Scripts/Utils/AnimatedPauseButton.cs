using Runner.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI
{
    public class AnimatedPauseButton : Button
    {
        public const string PlaySpriteFieldName = nameof(_playSprite);
        public const string PauseSpriteFieldName = nameof(_pauseSprite);
        public const string ButtonImageFieldName = nameof(_buttonImage);

        [SerializeField] private Sprite _playSprite;
        [SerializeField] private Sprite _pauseSprite;
        [SerializeField] private Image _buttonImage;

        public void SwitchSprites(bool isOnPause)
        {
            if (isOnPause)
            {
                _buttonImage.sprite = _playSprite;
            }
            else
            {
                _buttonImage.sprite = _pauseSprite;
            }
        }
    }
}