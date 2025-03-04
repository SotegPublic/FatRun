using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runner.Core
{
    [RequireComponent(typeof(RectTransform))]
    public class AnimatedButton : Button
    {
        public const string IdleSpriteFieldName = nameof(_idleSprite);
        public const string ButtonDownSpriteFieldName = nameof(_buttonDownSprite);
        public const string ButtonImageFieldName = nameof(_buttonImage);

        [SerializeField] protected Sprite _idleSprite;
        [SerializeField] protected Sprite _buttonDownSprite;
        [SerializeField] protected Image _buttonImage;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            ChangeSprite(true);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            ChangeSprite(false);
        }

        public void ChangeSprite(bool isDown)
        {
            _buttonImage.sprite = isDown ? _buttonDownSprite : _idleSprite;
        }
    }
}