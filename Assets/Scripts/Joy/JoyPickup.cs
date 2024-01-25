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
    public bool autoPickup = true;
    private void OnTriggerEnter(Collider other)
    {
        if (autoPickup)
            DoPickup(other.gameObject);
    }

    public void DoPickup(GameObject pickerUpper)
    {
        if (pickupRoutine == null)
        {
            pickerUpper.GetComponentInParent<Character>()?.CollectJoy();

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
