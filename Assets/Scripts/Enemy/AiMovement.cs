using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AiMovement : MonoBehaviour
{
    public AiSensor sensor;
    public float updateRate = 0.5f;

    [Header("Patrol Settings")]
    public Vector3 patrolCenter;
    public Vector3 patrolSize = new Vector3(10f, 0f, 10f);
    public float patrolPointReachedThreshold = 1f;

    [Header("Search Settings")]
    public float searchDuration = 3f;
    public float rotationSpeed = 120f;

    [Header("Forced State")]
    public bool forceState = false;
    public State forcedState = State.Patrol;

    private NavMeshAgent agent;
    private float updateTimer;

    public enum State { Patrol, Chase, Search, Returning }
    public State currentState = State.Patrol;

    private Vector3 currentPatrolTarget;
    private GameObject currentChaseTarget;
    private Vector3 lastSeenPosition;

    private float searchTimer;
    private bool isRotating = false;
    private bool hasStartedSearch = false;
    private bool hasSetReturnDestination = false;

    private bool isPaused = false;

    public bool isChasing => currentState == State.Chase;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        patrolCenter = transform.position;
        updateTimer = updateRate;
        SetRandomPatrolPoint();
    }

    void Update()
    {
        if (isPaused) return;

        if (forceState)
        {
            HandleForcedState();
            return;
        }

        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0)
        {
            updateTimer = updateRate;
            GameObject target = GetNearestTarget();

            if (target != null)
            {
                currentChaseTarget = target;
                lastSeenPosition = target.transform.position;
                currentState = State.Chase;
                agent.SetDestination(target.transform.position);
            }
            else
            {
                switch (currentState)
                {
                    case State.Chase:
                        currentState = State.Search;
                        agent.SetDestination(lastSeenPosition);
                        break;

                    case State.Search:
                        if (!hasStartedSearch && !agent.pathPending && agent.remainingDistance < patrolPointReachedThreshold)
                        {
                            hasStartedSearch = true;
                            isRotating = true;
                            searchTimer = searchDuration;
                        }
                        break;

                    case State.Returning:
                        if (!hasSetReturnDestination)
                        {
                            agent.SetDestination(GetClosestPointInBounds(transform.position));
                            hasSetReturnDestination = true;
                        }

                        if (IsInsidePatrolBounds(transform.position))
                        {
                            currentState = State.Patrol;
                            hasSetReturnDestination = false;
                            SetRandomPatrolPoint();
                        }
                        break;

                    case State.Patrol:
                        if (!agent.pathPending && agent.remainingDistance < patrolPointReachedThreshold)
                        {
                            SetRandomPatrolPoint();
                        }
                        break;
                }
            }
        }

        if (currentState == State.Search && isRotating)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0)
            {
                isRotating = false;
                hasStartedSearch = false;
                hasSetReturnDestination = false;
                currentState = State.Patrol;
            }
        }

        if (currentState == State.Chase && currentChaseTarget != null)
        {
            agent.SetDestination(currentChaseTarget.transform.position);
        }
    }

    void HandleForcedState()
    {
        currentState = forcedState;

        switch (forcedState)
        {
            case State.Patrol:
                if (!agent.pathPending && agent.remainingDistance < patrolPointReachedThreshold)
                {
                    SetRandomPatrolPoint();
                }
                break;

            case State.Chase:
                if (sensor != null)
                {
                    GameObject target = GetNearestTarget();
                    if (target != null)
                    {
                        agent.SetDestination(target.transform.position);
                    }
                }
                break;

            case State.Search:
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                break;

            case State.Returning:
                agent.SetDestination(GetClosestPointInBounds(transform.position));
                break;
        }
    }

    GameObject GetNearestTarget()
    {
        if (sensor == null || sensor.Objects.Count == 0)
            return null;

        GameObject nearest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject obj in sensor.Objects)
        {
            float dist = Vector3.Distance(currentPos, obj.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = obj;
            }
        }
        return nearest;
    }

    void SetRandomPatrolPoint()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(-patrolSize.x / 2, patrolSize.x / 2),
            0,
            Random.Range(-patrolSize.z / 2, patrolSize.z / 2)
        );
        currentPatrolTarget = patrolCenter + randomPoint;

        if (NavMesh.SamplePosition(currentPatrolTarget, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    Vector3 GetClosestPointInBounds(Vector3 position)
    {
        Vector3 min = patrolCenter - patrolSize / 2f;
        Vector3 max = patrolCenter + patrolSize / 2f;

        float x = Mathf.Clamp(position.x, min.x, max.x);
        float z = Mathf.Clamp(position.z, min.z, max.z);
        return new Vector3(x, position.y, z);
    }

    bool IsInsidePatrolBounds(Vector3 position)
    {
        Vector3 min = patrolCenter - patrolSize / 2f;
        Vector3 max = patrolCenter + patrolSize / 2f;

        return position.x >= min.x && position.x <= max.x &&
               position.z >= min.z && position.z <= max.z;
    }

    public void PauseMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            isPaused = true;
        }
    }

    public void ResumeMovement()
    {
        if (agent != null)
        {
            agent.isStopped = false;
            isPaused = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(patrolCenter, patrolSize);
    }

    public void DisableAgent()
    {
        if (agent != null)
        {
            agent.enabled = false;
        }
    }

    public void EnableAgent()
    {
        if (agent != null)
        {
            agent.enabled = true;
        }
    }
}
