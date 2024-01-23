using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AdjustSaturation : JoyReceiverBase
{
    public Volume Volume;
    public float depressedValue = 0f, happyValue = 1f;
    private ColorAdjustments colorAdjustments;

    protected override void JoyChanged(float amount)
    {
        if (Volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.saturation.value = Mathf.Lerp(depressedValue, happyValue, amount);
        }
    }
}
