using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEnter : MonoBehaviour
{
    public UnityEvent onTrigger;


    private void OnTriggerEnter(Collider other)
    {
        onTrigger?.Invoke();
    }
}
