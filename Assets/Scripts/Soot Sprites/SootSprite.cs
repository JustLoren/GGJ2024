using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class SootSprite : MonoBehaviour
{
    private Rigidbody rb;
    public float minThrust = 3f, maxThrust = 6f;
    public float maxErrorAngle = 30f;
    public float minTimeBetweenThrusts = 3f, maxTimeBetweenThrusts = 8f;
    public float maxDistance = 8f;
    public float minVelocityToRubberband = .1f;
    public float happinessToRob = .05f;

    public List<Transform> targetList = new();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private float nextThrust = 0f;
    private void FixedUpdate()
    {
        //Dead sprites don't thrust. Giggity.
        if (isDead) return;

        if (JoyMeter.Instance?.currentJoy > .8f)
        {
            Kill();
            return;
        }

        if (Time.time > nextThrust)
        {
            ApplyThrust(Color.green);
        } else if (RubberbandToPlayer())
        {
            ApplyThrust(Color.yellow);
        }

        RotateTowardsTarget(GetTargetCoordinates());
    }

    private int targetIdx;
    private Vector3 GetTargetCoordinates()
    {
        if (!targetList.Any())
            return Character.Instance.center;
        else
            return targetList[targetIdx].position;
    }

    private bool isDead = false;
    public UnityEvent OnDeath;
    private void Kill()
    {
        if (isDead)
        {
            return;
        }
        rb.isKinematic = true;
        isDead = true;
        OnDeath?.Invoke();
    }

    private bool RubberbandToPlayer()
    {
        return GetThrustDirection().magnitude > maxDistance && rb.velocity.magnitude < minVelocityToRubberband;
    }
    private Vector3 GetThrustDirection()
    {
        return (GetTargetCoordinates()) - transform.position;
    }

    private void ApplyThrust(Color thrustColor)
    {
        var desiredDirection = GetThrustDirection();

        var distance = desiredDirection.magnitude;

        var randomAngle = Random.onUnitSphere * Random.Range(-maxErrorAngle, maxErrorAngle);

        var thrustDirection = Quaternion.Euler(randomAngle) * desiredDirection;
        thrustDirection.Normalize();

        var thrustForce = Random.Range(minThrust, maxThrust);

        Debug.DrawRay(transform.position, thrustDirection * thrustForce, thrustColor, .5f);

        rb.AddForce(thrustDirection * thrustForce, ForceMode.Impulse);
        
        nextThrust = Time.time + Random.Range(minTimeBetweenThrusts, maxTimeBetweenThrusts);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetList.Any())
        {
            targetIdx = (targetIdx + 1) % targetList.Count;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            Kill();

            JoyMeter.Instance.AddJoy(-happinessToRob);            
        }
    }

    public float lookForce = 10f;
    public void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - rb.position;
        directionToTarget.Normalize();

        // Calculate the rotation vector to the target
        Vector3 rotationVector = Vector3.Cross(transform.forward, directionToTarget);
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        // Apply a torque based on the angle to the target
        rb.AddTorque(rotationVector * angle * lookForce, ForceMode.Force);
    }
}
