using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runner.UI
{
    public class ShopItemButton : MonoBehaviour
    {
        [SerializeField] protected int _itemID;
        [SerializeField] protected Transform _transform;
        [SerializeField] protected TMP_Text _priceText;
        [SerializeField] protected Image _itemImage;
        [SerializeField] protected int _itemPrice;
        [SerializeField] protected Sprite _itemSprite;
        [SerializeField] protected string _ruItemName;
        [SerializeField] protected string _ruItemDiscription;
        [SerializeField] protected string _enItemName;
        [SerializeField] protected string _enItemDiscription;
        [SerializeField] protected string _trItemName;
        [SerializeField] protected string _trItemDiscription;
        [SerializeField] protected Image _backGroundImage;
        [SerializeField] protected float _selectedAlpha;
        [SerializeField] protected bool _isBaseItem;
        [SerializeField] protected Image _usedImage;

        protected float _idleAlpha;

        public bool IsSelected { get; private set; }
        public bool IsBuyed { get; private set; }
        public int ItemPrice => _itemPrice;
        public bool IsBaseItem => _isBaseItem;
        public Transform Transform => _transform;
        public string RuItemName => _ruItemName;
        public string RuItemDiscription => _ruItemDiscription;
        public string EnItemName => _enItemName;
        public string EnItemDiscription => _enItemDiscription;
        public string TrItemName => _trItemName;
        public string TrItemDiscription => _trItemDiscription;
        public int ItemID => _itemID;

        public void InitItem(bool isBuyed)
        {
            _idleAlpha = _backGroundImage.color.a;
            IsBuyed = isBuyed;

            if(isBuyed)
            {
                _priceText.text = "";
            }
            else
            {
                _priceText.text = _itemPrice.ToString();
            }

            _itemImage.sprite = _itemSprite;
            SetUsed(false);
        }

        public void SelectButton()
        {
            IsSelected = true;
            var color = _backGroundImage.color;
            color.a = _selectedAlpha;
            _backGroundImage.color = color;
        }

        public void DeselectButton()
        {
            IsSelected = false;
            var color = _backGroundImage.color;
            color.a = _idleAlpha;
            _backGroundImage.color = color;
        }

        public int BuyItem()
        {
            IsBuyed = true;
            _priceText.text = "";
            return _itemPrice;
        }

        public void SetUsed(bool isUsed)
        {
            _usedImage.enabled = isUsed;
        }
    }
}