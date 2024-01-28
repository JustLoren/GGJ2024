using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class FadeToBlack : MonoBehaviour
{
    public Image imageToFade; // Assign this in the inspector
    public float fadeDuration = 2.0f; // Duration in seconds
    public bool fadeComplete = false;
    public bool fadeOut = false;
    public void Start()
    {
        // Start the fade in effect
        if (fadeOut)
            StartCoroutine(FadeImageToZeroAlpha());
        else
            StartCoroutine(FadeImageToFullAlpha());
    }
     
    IEnumerator FadeImageToFullAlpha()
    {
        float elapsedTime = 0.0f;
        Color c = imageToFade.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            imageToFade.color = c;
            yield return null;
        }
        fadeComplete = true;
    }

    IEnumerator FadeImageToZeroAlpha()
    {
        float elapsedTime = 0.0f;
        Color c = imageToFade.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            imageToFade.color = c;
            yield return null;
        }
        fadeComplete = true;
        Destroy(this.gameObject);
    }
}
