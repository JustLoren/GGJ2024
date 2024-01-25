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
    private Camera playerCamera;
    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    public void CollectJoy()
    {
        animator.SetTrigger("Collect");
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

        // Check if the current animator state has the tag "Stationary"
        bool isStationary = animator.GetCurrentAnimatorStateInfo(0).IsTag("Stationary");
        if (!isStationary)
        {
            // Move the character
            var realSpeed = Mathf.Lerp(depressedSpeed, happySpeed, JoyMeter.Instance.currentJoy);
            controller.Move(direction * realSpeed * Time.deltaTime);

            // Rotate the character to face the moving direction
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            DoJump();
        }

        animator.SetFloat("Speed", direction.magnitude);
        animator.SetFloat("Joy", JoyMeter.Instance.currentJoy);
        animator.SetBool("OnGround", controller.isGrounded);
    }

    public float jumpForce = 8f; // Initial force of the jump
    
    private bool isGrounded;
    private float verticalVelocity;
    public float stickToGroundForce;
    public LayerMask groundLayers;
    public float ledgeRadius = .08f;

    private int ledgeDuration = 0;
    private int maxLedgeDuration = 5;
    void DoJump()
    {
        #region Jumping And Ground Check

        //Phsyics Check To Ground        
        controller.Move(new Vector3(0, -.01f, 0f));

        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            RaycastHit hit;
            var secondaryCheck = Physics.SphereCast(controller.transform.position, ledgeRadius, Vector3.down, out hit, (controller.height / 2f) + .1f, groundLayers.value);
            if (!secondaryCheck)
            {
                Debug.Log($"[{Time.frameCount}] Fuck, we're on a ledge");
                if (++ledgeDuration >= maxLedgeDuration)
                {
                    if (ledgeShrinker == null)
                        ledgeShrinker = StartCoroutine(ShrinkController());
                }                
            } else
            {
                ledgeDuration = 0;
            }
        } else
        {
            ledgeDuration = 0;
        }

        //If The Player Is On The Ground Stick To Ground And Reset Vertical Velocity
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -stickToGroundForce;
        }

        //Jump If The Player Is Grounded
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity += jumpForce * stickToGroundForce;
        }

        //Gravity
        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        //Apply These Calculations To The Actual Player Controller
        Vector3 fallVector = new Vector3(0, verticalVelocity, 0);
        controller.Move(fallVector * Time.deltaTime);


        #endregion
    }

    private Coroutine ledgeShrinker = null;
    private IEnumerator ShrinkController()
    {
        var currentRadius = controller.radius;
        controller.radius = ledgeRadius;        
        yield return new WaitForSeconds(.25f);
        while (controller.radius != currentRadius)
        {
            controller.radius = Mathf.MoveTowards(controller.radius, currentRadius, currentRadius * 4f * Time.deltaTime);
            yield return null;
        }
        controller.radius = currentRadius;
        ledgeShrinker = null;
        ledgeDuration = 0;
    }
}
