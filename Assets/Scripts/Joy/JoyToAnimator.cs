using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyToAnimator : JoyReceiverBase
{
    public Animator animator;
    public string propertyName;
    public float depressionValue = 0f, happinessValue = 1f;
    protected override void JoyChanged(float amount)
    {
        animator.SetFloat(propertyName, Mathf.Lerp(depressionValue, happinessValue, amount));
    }
}
