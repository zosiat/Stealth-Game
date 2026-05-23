using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    public enum GuardState
    {
        Patrolling,
        Investigating,
        Chasing
    }

    [Header("patrol")]
    public Transform[] waypoints;
    public float waypointStoppingDistance = 0.5f;

    [Header("detection")]
    public Transform player;
    public float visionRange = 10f;
    public float visionAngle = 60f;
    public LayerMask obstacleMask;

    [Header("state settings")]
    public float investigateTime = 3f;
    public float chaseLoseTime = 2f;

    [Header("investigation looking")]
    public float lookAroundSpeed = 90f;
    public float lookAroundAngle = 60f;

    [Header("visuals")]
    public Renderer guardRenderer;
    public Color patrolColor = Color.green;
    public Color investigateColor = Color.yellow;
    public Color chaseColor = Color.red;

    private NavMeshAgent agent;
    private GuardState currentState;
    private int currentWaypointIndex;
    private Vector3 lastKnownPlayerPosition;
    private float investigateTimer;
    private float chaseLoseTimer;
    private float lookAroundTimer;
    private Quaternion investigateStartRotation;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        ChangeState(GuardState.Patrolling);
    }

    void Update()
    {
        bool canSeePlayer = CanSeePlayer();
        bool playerIsCrouching = IsPlayerCrouching();

        if (playerIsCrouching && currentState == GuardState.Chasing)
        {
            lastKnownPlayerPosition = player.position;
            ChangeState(GuardState.Investigating);
            return;
        }

        if (canSeePlayer && !playerIsCrouching)
        {
            lastKnownPlayerPosition = player.position;
            ChangeState(GuardState.Chasing);
        }

        switch (currentState)
        {
            case GuardState.Patrolling:
                Patrol();
                break;

            case GuardState.Investigating:
                Investigate();
                break;

            case GuardState.Chasing:
                Chase(canSeePlayer);
                break;
        }
    }

    bool IsPlayerCrouching()
    {
        if (player == null) return false;

        FirstPersonPlayer playerMovement = player.GetComponent<FirstPersonPlayer>();

        if (playerMovement == null) return false;

        return playerMovement.IsCrouching();
    }

    void ChangeState(GuardState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        if (currentState == GuardState.Patrolling)
        {
            agent.updateRotation = true;
            SetGuardColor(patrolColor);
        }

        if (currentState == GuardState.Investigating)
        {
            agent.updateRotation = false;
            investigateTimer = investigateTime;
            lookAroundTimer = 0f;
            investigateStartRotation = transform.rotation;

            agent.SetDestination(lastKnownPlayerPosition);
            SetGuardColor(investigateColor);
        }

        if (currentState == GuardState.Chasing)
        {
            agent.updateRotation = true;
            chaseLoseTimer = chaseLoseTime;
            SetGuardColor(chaseColor);
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < waypointStoppingDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }

            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Investigate()
    {
        if (!agent.pathPending && agent.remainingDistance < waypointStoppingDistance)
        {
            investigateTimer -= Time.deltaTime;

            lookAroundTimer += Time.deltaTime * lookAroundSpeed;

            float lookAngle = Mathf.Sin(lookAroundTimer * Mathf.Deg2Rad) * lookAroundAngle;

            transform.rotation = investigateStartRotation * Quaternion.Euler(0, lookAngle, 0);

            if (investigateTimer <= 0)
            {
                ChangeState(GuardState.Patrolling);

                if (waypoints.Length > 0)
                {
                    agent.SetDestination(waypoints[currentWaypointIndex].position);
                }
            }
        }
    }

    void Chase(bool canSeePlayer)
    {
        if (IsPlayerCrouching())
        {
            lastKnownPlayerPosition = player.position;
            ChangeState(GuardState.Investigating);
            return;
        }

        if (canSeePlayer)
        {
            chaseLoseTimer = chaseLoseTime;
            agent.SetDestination(player.position);
        }
        else
        {
            chaseLoseTimer -= Time.deltaTime;

            if (chaseLoseTimer <= 0)
            {
                ChangeState(GuardState.Investigating);
            }
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 eyePosition = transform.position + Vector3.up;
        Vector3 playerPosition = player.position + Vector3.up * 0.5f;
        Vector3 directionToPlayer = playerPosition - eyePosition;

        if (directionToPlayer.magnitude > visionRange)
        {
            return false;
        }

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer > visionAngle / 2f)
        {
            return false;
        }

        if (Physics.Raycast(eyePosition, directionToPlayer.normalized, out RaycastHit hit, visionRange))
        {
            if (hit.transform == player || hit.transform.IsChildOf(player))
            {
                return true;
            }
        }

        return false;
    }

    void SetGuardColor(Color color)
    {
        if (guardRenderer != null)
        {
            guardRenderer.material.color = color;
            return;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            rend.material.color = color;
        }
    }

    // void OnDrawGizmos()
    // {
    //     Vector3 eyePosition = transform.position + Vector3.up;

    //     //range sphere
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(eyePosition, visionRange);

    //     //vision cone lines
    //     Vector3 leftEdge = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward;
    //     Vector3 rightEdge = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward;

    //     Gizmos.color = Color.cyan;

    //     Gizmos.DrawRay(eyePosition, leftEdge * visionRange);
    //     Gizmos.DrawRay(eyePosition, transform.forward * visionRange);
    //     Gizmos.DrawRay(eyePosition, rightEdge * visionRange);
    // }
}