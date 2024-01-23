using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputManagerEntry;

public class JoyReceiver : MonoBehaviour
{
    public UnityEvent<float> OnJoyReceivedArg = null;

    public float depressionValue = 0f, happinessValue = 1f;

    private void OnEnable()
    {
        JoyMeter.Subscribe(this, FireEvent);
    }

    private void FireEvent(float amount)
    {
        OnJoyReceivedArg?.Invoke(Mathf.Lerp(depressionValue, happinessValue, amount));
    }

    private void OnDisable()
    {
        JoyMeter.Unsubscribe(this);
    }
}
