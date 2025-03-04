using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runner.UI
{
    public class AnimationShopButton : ShopItemButton, IPointerClickHandler
    {
        public Action<AnimationShopButton> OnAnimationShopButtonClick;

        public const string AnimationIDFieldName = nameof(_animationID);

        [SerializeField] private AnimationTypes _animationType;
        [SerializeField] private int _animationID;

        public AnimationTypes AnimationType => _animationType;
        public int AnimationID => _animationID;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnAnimationShopButtonClick?.Invoke(this);
        }
    }
}