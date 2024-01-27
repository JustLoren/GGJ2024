using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
    public PathNode[] waypoints; // Array of waypoints
    private PathNode activeWaypoint = null;
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 4.0f;
    public float rotationRate = 90f;
    public float detectionRange = 10.0f;
    public float yellRange = 5f;
    public float anger = 1f;
    public float angerBuildRate = .1f;
    public Transform eyes;
    private Animator animator;
    private NavMeshAgent agent;
    private SpeechController speech;
    private int waypointIndex = 0;
    public Character player; // Assign the player's transform in the editor or via code

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        speech = GetComponent<SpeechController>();
        agent.updateRotation = false;

        GoToNextWaypoint();
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        agent.destination = waypoints[waypointIndex].transform.position;
        activeWaypoint = waypoints[waypointIndex];
        waypointIndex = (waypointIndex + 1) % waypoints.Length;

        agent.speed = patrolSpeed;
        agent.isStopped = false;
        animator.SetBool("Chasing", false);
    }

    void Update()
    {
        if (!IsStationary())
        {
            //Only when fully angry will she chase the player
            if (PlayerInSight() && anger == 1f)
            {
                ChasePlayer();
            }
            else if (agent.remainingDistance < .25f)
            {
                if (activeWaypoint != null)
                {
                    //We trigger this waypoint
                    if (activeWaypoint.forceRotation)
                    {
                        transform.rotation = activeWaypoint.transform.rotation;
                    }

                    if (activeWaypoint.idleAnimation > 0)
                    {
                        animator.SetInteger("Idle Action", activeWaypoint.idleAnimation);
                        animator.Update(Time.deltaTime);
                        animator.SetInteger("Idle Action", 0);
                        activeWaypoint = null;
                    }
                }

                if (!IsIdle())
                {
                    GoToNextWaypoint();
                }
            }

            if (!IsIdle())
            {
                var startingRotation = transform.rotation;
                var desiredRotation = Quaternion.LookRotation(agent.steeringTarget - transform.position);
                var newRotation = Quaternion.RotateTowards(startingRotation, desiredRotation, rotationRate * Time.deltaTime);
                transform.rotation = newRotation;
            }

            //While she walks around, she gets angry
            anger = Mathf.Clamp01(anger + (angerBuildRate * Time.deltaTime));
        }

        animator.SetFloat("Speed", agent.speed / chaseSpeed);
    }

    bool PlayerInSight()
    {
        if (player == null) return false;

        // Simple distance check
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer > detectionRange)
            return false;

        RaycastHit hit;
        var targetPosition = player.transform.position + (Vector3.up * (player.height / 2f));
        var direction = targetPosition - eyes.position;
        var ray = new Ray(eyes.position, direction);
        if (Physics.Raycast(ray, out hit, detectionRange))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            return hit.collider.gameObject.layer == LayerMask.NameToLayer("Character");
        }

        Debug.DrawRay(ray.origin, ray.direction * yellRange, Color.red);

        return false;
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.destination = player.transform.position;
        activeWaypoint = null;
        animator.SetBool("Chasing", true);

        if (Vector3.Distance(transform.position, player.transform.position) < yellRange)
        {
            agent.isStopped = true;
            agent.ResetPath();
            animator.SetTrigger("Yell");
            speech.Yell();
            animator.Update(Time.deltaTime);
            transform.LookAt(player.transform.position);
            player.Cower(this.gameObject);

            //She yelled. It was very cathartic. No more anger.
            anger = 0f;
        }
    }

    private bool IsStationary()
    {
        if (animator.IsInTransition(0))
        {
            AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextStateInfo = animator.GetNextAnimatorStateInfo(0);
            if (nextStateInfo.IsTag("Stationary") || currentStateInfo.IsTag("Stationary"))
            {
                return true;
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Stationary"))
        {
            return true;
        }
        return false;
    }

    private bool IsIdle()
    {
        if (animator.IsInTransition(0))
        {
            AnimatorStateInfo nextStateInfo = animator.GetNextAnimatorStateInfo(0);
            if (nextStateInfo.IsTag("Idle"))
            {
                return true;
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            return true;
        }
        return false;
    }

}
