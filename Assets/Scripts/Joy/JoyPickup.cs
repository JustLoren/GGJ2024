using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JoyPickup : MonoBehaviour
{
    public GameObject displayObject;
    private Coroutine pickupRoutine = null;
    public float joyAmount = 1f;
    public float gainSpeed = 5f;
    public UnityEvent onPickup = null;
    private void OnTriggerEnter(Collider other)
    {
        if (pickupRoutine == null)
        {
            pickupRoutine = StartCoroutine(DoPickup());
        }
    }

    private IEnumerator DoPickup()
    {                
        onPickup?.Invoke();
        do
        {
            yield return null;
            JoyMeter.Instance.AddJoy(gainSpeed * Time.deltaTime);
        } while (JoyMeter.Instance.currentJoy < joyAmount);

        pickupRoutine = null;
    }
}
