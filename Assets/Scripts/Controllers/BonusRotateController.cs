using Runner.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.BonusSystem
{
    public class BonusRotateController : MonoBehaviour
    {
        [SerializeField] Transform _bonusObjectTransform;

        public void LocalUpdate(Vector3 rotationVector)
        {
            _bonusObjectTransform.localRotation = Quaternion.Euler(rotationVector);
        }
    }
}