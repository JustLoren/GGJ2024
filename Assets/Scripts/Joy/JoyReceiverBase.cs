using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class JoyReceiverBase : MonoBehaviour
{
    private void OnEnable()
    {
        JoyMeter.Subscribe(this, JoyChanged);
    }

    private void OnDisable()
    {
        JoyMeter.Unsubscribe(this);
    }

    protected virtual void JoyChanged(float amount)
    {
        //Do nothing
    }
}
