using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AdjustVignette : JoyReceiverBase
{
    public Volume Volume;
    public float depressedValue = 0f, happyValue = 1f;
    private Vignette vignette;

    protected override void JoyChanged(float amount)
    {        
        if (Volume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = Mathf.Lerp(depressedValue, happyValue, amount);
        }
    }
}
