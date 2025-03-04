using Runner.Core;
using System;
using UnityEngine;

namespace Runner.BonusSystem
{
    public class BonusController : MonoBehaviour, IBonus
    {
        public Action<BonusController> OnObjectCollected { get; set; }
        public Action<BonusController> OnObjectNeedToRemove { get; set; }

        [SerializeField] private TriggerController _triggerController;
        [SerializeField] private BonusRotateController _rotateController;
        [SerializeField] private float _unloadDistance;
        [SerializeField] private Transform _transform;
        [SerializeField] private MeshRenderer _armMeshRenderer;
        [SerializeField] private ParticleSystem _collectParticle;
        [SerializeField] private AudioSource _bonusAudioSource;

        private Transform _playerTransform;

        private void Awake()
        {
            _triggerController.OnTriggerActivate += CollectObject;
        }

        public void InitObject(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public void LocalUpdate(Vector3 rotateVector)
        {
            _rotateController.LocalUpdate(rotateVector);

            if (_transform.position.z < _playerTransform.position.z && Math.Abs(_transform.position.z - _playerTransform.position.z) > _unloadDistance)
            {
                RemoveObject();
            }
        }

        private void RemoveObject()
        {
            OnObjectNeedToRemove?.Invoke(this);
            _armMeshRenderer.enabled = true;
        }

        private void CollectObject()
        {
            OnObjectCollected?.Invoke(this);
            _collectParticle.Play();
            _bonusAudioSource.Play();
            _armMeshRenderer.enabled = false;
            Invoke(nameof(RemoveObject), 0.33f);
        }
    }
}