using System;
using UnityEngine;

namespace Runner.BonusSystem
{
    public class HurdleController : MonoBehaviour, IHurdle
    {
        public Action<HurdleController> OnObjectCollected { get; set; }
        public Action<HurdleController> OnObjectNeedToRemove { get; set; }

        [SerializeField] private TriggerController _triggerController;
        [SerializeField] private float _unloadDistance;
        [SerializeField] private Transform _transform;
        [SerializeField] private HurdleAppearance _hurdleApearence;
        [SerializeField] private Canvas _hurdleCanvas;
        //[SerializeField] private AudioSource _hurdleAudioSourse;

        private Transform _playerTransform;
        private bool _isPositive;
        private int _bonusValue;

        public bool IsPositive => _isPositive;
        public int BonusValue => _bonusValue;

        private void Awake()
        {
            _triggerController.OnTriggerActivate += CollectObject;
        }

        public void InitObject(Transform playerTransform, bool isPositive, int bonusValue, Camera worldCamera)
        {
            _hurdleCanvas.worldCamera = worldCamera;
            _playerTransform = playerTransform;
            _isPositive = isPositive;
            _bonusValue = bonusValue;
            _hurdleApearence.InitHurdle(isPositive, bonusValue);
        }

        public void LocalUpdate()
        {
            if (_transform.position.z < _playerTransform.position.z && Math.Abs(_transform.position.z - _playerTransform.position.z) > _unloadDistance)
            {
                RemoveObject();
            }
        }

        private void RemoveObject()
        {
            OnObjectNeedToRemove?.Invoke(this);
        }

        private void CollectObject()
        {
            OnObjectCollected?.Invoke(this);
            //_hurdleAudioSourse.Play();
            RemoveObject();
        }
    }
}