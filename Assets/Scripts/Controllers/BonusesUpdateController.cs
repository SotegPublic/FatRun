using Runner.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.BonusSystem
{
    public class BonusesUpdateController : MonoBehaviour, IUpdateble, IDisposable
    {
        [SerializeField] private int _bonusesRotationSpeed;

        private List<BonusController> _bonusControllers = new List<BonusController>(15);
        private List<HurdleController> _hurdleControllers = new List<HurdleController>(8);
        private float _currentRotateZ;

        public void LocalUpdate(float deltaTime)
        {
            if(_currentRotateZ >= 360 )
            {
                _currentRotateZ = 0;
            }

            _currentRotateZ += _bonusesRotationSpeed * deltaTime;
            var rotateVector = new Vector3(-90f, 0f, _currentRotateZ);

            for (int i = 0; i < _bonusControllers.Count; i++)
            {
                _bonusControllers[i].LocalUpdate(rotateVector);
            }

            for(int i = 0; i < _hurdleControllers.Count; i++)
            {
                _hurdleControllers[i].LocalUpdate();
            }
        }

        public void AddBonusController(BonusController controller)
        {
            controller.OnObjectNeedToRemove += DeleteController;
            _bonusControllers.Add(controller);
        }

        public void AddHurdleController(HurdleController controller)
        {
            controller.OnObjectNeedToRemove += DeleteController;
            _hurdleControllers.Add(controller);
        }

        private void DeleteController(BonusController controller)
        {
            controller.OnObjectNeedToRemove -= DeleteController;
            _bonusControllers.Remove(controller);
        }

        private void DeleteController(HurdleController controller)
        {
            controller.OnObjectNeedToRemove -= DeleteController;
            _hurdleControllers.Remove(controller);
        }

        public void Dispose()
        {
            for(int i = 0; i < _bonusControllers.Count; i++)
            {
                _bonusControllers[i].OnObjectNeedToRemove -= DeleteController;
            }

            for (int i = 0; i < _hurdleControllers.Count; i++)
            {
                _hurdleControllers[i].OnObjectNeedToRemove -= DeleteController;
            }

            _bonusControllers.Clear();
            _hurdleControllers.Clear();
        }
    }
}