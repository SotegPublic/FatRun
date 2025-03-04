using Runner.BonusSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    public Action OnPlayerFinished;
    
    [SerializeField] private TriggerController _finishTriggerController;

    private void Awake()
    {
        _finishTriggerController.OnTriggerActivate += FinishStateActivate;
    }

    private void FinishStateActivate()
    {
        OnPlayerFinished?.Invoke();
    }
}
