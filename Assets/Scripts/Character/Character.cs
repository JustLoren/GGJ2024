using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    private CharacterController controller;
    public float depressedSpeed = 2f;
    public float happySpeed = 5f;
    public float rotationSpeed = 10f; // Speed of rotation
    private Camera playerCamera; // Assign this in the Inspector
    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    void Update()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical);

        // Convert the direction from local space to world space relative to the camera
        direction = playerCamera.transform.TransformDirection(direction);
        direction.y = 0; // Keep the movement horizontal

        //Speed limit of 1 unit
        if (direction.magnitude > 1f)
            direction = direction.normalized;

        // Move the character
        var realSpeed = Mathf.Lerp(depressedSpeed, happySpeed, JoyMeter.Instance.currentJoy);
        controller.Move(direction * realSpeed * Time.deltaTime);

        // Rotate the character to face the moving direction
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        animator.SetFloat("Speed", direction.magnitude);
        animator.SetFloat("Joy", JoyMeter.Instance.currentJoy);
    }
}
