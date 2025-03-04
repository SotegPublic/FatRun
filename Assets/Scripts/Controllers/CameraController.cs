using Runner.Core;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Runner.CameraSystem
{
    public class CameraController : MonoBehaviour, ILateUpdateble
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private float _cameraHeight;
        [SerializeField] private float _endGameRotationY;

        private bool _isPlayerOnKickPosition;

        private float _cameraLerpProgress;
        private Quaternion _startCameraRotation;
        private Quaternion _endCameraRotation;

        private bool _isGetEndPosition;

        public void LateLocalUpdate(float deltaTime)
        {
            if(_isPlayerOnKickPosition)
            {
                if(!_isGetEndPosition)
                {
                    CameraLerp(deltaTime);
                }
            }
            else
            {
                _cameraHolder.position = new Vector3(0, _cameraHeight, _playerTransform.position.z);
            }
        }

        private void CameraLerp(float deltaTime)
        {
            _cameraLerpProgress += deltaTime;

            var rotation = Quaternion.Lerp(_startCameraRotation, _endCameraRotation, _cameraLerpProgress);
            _cameraHolder.rotation = rotation;

            if (_cameraLerpProgress >= 1)
            {
                _isGetEndPosition = true;
                _cameraLerpProgress = 0;
            }
        }

        public void WhenPlayerOnKickPosition()
        {
            _isPlayerOnKickPosition = true;
            _startCameraRotation = _cameraHolder.rotation;
            _endCameraRotation = Quaternion.Euler(0, _endGameRotationY, 0);
        }
    }
}