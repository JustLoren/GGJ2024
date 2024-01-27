using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechController : MonoBehaviour
{
    public float minTalkTime = 7f, maxTalkTime = 12f;
    public FMODUnity.StudioEventEmitter niceEmitter, neutralEmitter, meanEmitter, insultEmitter;

    private void Start()
    {
        SetNextTalkTime();
    }

    public void Yell()
    {
        //No need to yell if we're already yelling...
        if (insultEmitter.IsPlaying()) return;

        //Stop any and all fun speech and deliver some hate speech.
        niceEmitter.Stop();
        neutralEmitter.Stop();
        meanEmitter.Stop();
        insultEmitter.Play();

        SetNextTalkTime();
    }

    private void Update()
    {
        if (Time.time >= nextTalkTime)
        {
            Talk();
        }
    }

    private float nextTalkTime = float.MinValue;
    public void Talk()
    {
        //We don't allow non-insults to interrupt

        switch (JoyMeter.Instance.currentJoy)
        {
            case >= .8f:
                niceEmitter.Play();
                break;
            case >= .2f:
                neutralEmitter.Play();
                break;
            default:
                meanEmitter.Play();
                break;            
        }

        SetNextTalkTime();
    }

    private void SetNextTalkTime()
    {

        nextTalkTime = Time.time + Random.Range(minTalkTime, maxTalkTime);
    }
}

