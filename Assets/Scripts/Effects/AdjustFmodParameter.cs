using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustFmodParameter : JoyReceiverBase
{
    protected override void JoyChanged(float amount)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Joy Level", amount);
    }
}
