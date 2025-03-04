using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsEventManager : MonoBehaviour
{
    public Action OnKickAnimationHit;

    public void WhenAnimationEvent()
    {
        OnKickAnimationHit?.Invoke();
    }
}
