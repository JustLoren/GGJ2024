using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class JoyMeter : MonoBehaviour
{    
    public static JoyMeter Instance { get; private set; }
    private Dictionary<MonoBehaviour, Action<float>> watchers = new();

    public float currentJoy = .5f;
    public AnimationCurve joyDrainRate;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("You should not have two JoyMeters, homie");
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void Update()
    {
        var amt = joyDrainRate.Evaluate(currentJoy);
        AddJoy(-amt * Time.deltaTime);
    }

    public void AddJoy(float amt)
    {
        currentJoy = Mathf.Clamp01(currentJoy + amt);
        BroadcastJoy();
    }

    private void BroadcastJoy()
    {
        foreach (var action in watchers.Values)
            action(currentJoy);        
    }

    public static void Subscribe(MonoBehaviour owner, Action<float> action) => Instance?.InternalSubscribe(owner, action);
    public void InternalSubscribe(MonoBehaviour owner, Action<float> action)
    {
        if (watchers.ContainsKey(owner))
            throw new Exception($"Double joy subscription by {owner} (arg)");

        watchers.Add(owner, action);
    }

    public static void Unsubscribe(MonoBehaviour owner) => Instance?.InternalUnsubscribe(owner);

    private void InternalUnsubscribe(MonoBehaviour owner)
    {
        watchers.Remove(owner);
    }
}
