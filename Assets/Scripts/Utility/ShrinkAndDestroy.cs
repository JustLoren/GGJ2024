using UnityEngine;

public class ShrinkAndDestroy : MonoBehaviour
{
    public float shrinkDuration = .5f; // Duration for the object to fully shrink
    private Vector3 originalScale;
    private bool isShrinking = false;

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale
    } 

    void Update()
    {
        if (isShrinking)
        {
            float shrinkStep = Time.deltaTime / shrinkDuration;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkStep);

            if (transform.localScale.magnitude < 0.01f)
            {
                Destroy(gameObject); // Destroy the object when it's small enough
            }
        }
    }

    public void StartShrinking()
    {
        isShrinking = true;
    }
}
