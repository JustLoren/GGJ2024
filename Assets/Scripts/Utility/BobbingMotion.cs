using UnityEngine;

public class BobbingMotion : MonoBehaviour
{
    // Variables for bobbing motion
    public float bobbingSpeed = 0.5f;
    public float bobbingAmplitude = 0.5f;

    // Variable for rotation speed
    public float rotationSpeed = 30.0f; // Degrees per second

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Bobbing motion
        Vector3 newPos = startPos;
        newPos.y += bobbingAmplitude * Mathf.Sin(bobbingSpeed * Time.time);
        transform.position = newPos;

        // Rotating motion
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }
}
