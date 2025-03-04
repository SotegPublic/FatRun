using System;
using UnityEngine;

namespace Runner.BonusSystem
{
    public class TriggerController : MonoBehaviour
    {
        public Action OnTriggerActivate;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerActivate?.Invoke();
        }
    }
}