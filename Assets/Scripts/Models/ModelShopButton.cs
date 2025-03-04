using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runner.UI
{
    public class ModelShopButton : ShopItemButton, IPointerClickHandler
    {
        public Action<ModelShopButton> OnModelShopButtonClick;

        [SerializeField] private GameObject _modelPrefab;


        public GameObject ModelPrefab => _modelPrefab;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnModelShopButtonClick?.Invoke(this);
        }
    }
}