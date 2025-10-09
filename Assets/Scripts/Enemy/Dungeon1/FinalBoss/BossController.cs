using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AiSensor))]
[RequireComponent(typeof(AiMovement))]
public class BossController : MonoBehaviour
{
    public enum BossState { Idle, Attack, Special, Dead }
    public BossState currentState = BossState.Idle;

    private AiMovement aiMovement;
    private AiSensor sensor;
    private Transform player;

    [Header("Dash Settings")]
    public float dashOffset = 3f;
    public float dashSpeedMultiplier = 5f;
    public float dashCooldown = 5f;
    private float lastDashTime = -Mathf.Infinity;
    private bool isDashing = false;
    private float lostPlayerTimer = 0f;

    [Header("Stun Settings")]
    public float stunDuration = 2f;
    private bool isStunned = false;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float wallCheckDistance = 1f;

    void Start()
    {
        aiMovement = GetComponent<AiMovement>();
        sensor = GetComponent<AiSensor>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isStunned) return;

        switch (currentState)
        {
            case BossState.Idle:
                CheckPlayerDetection();
                break;

            case BossState.Attack:
                if (!sensor.Objects.Contains(player.gameObject))
                {
                    lostPlayerTimer += Time.deltaTime;
                    if (lostPlayerTimer >= 2f)
                    {
                        aiMovement.enabled = true;
                        lostPlayerTimer = 0f;
                    }
                }
                else
                {
                    lostPlayerTimer = 0f;
                    if (!isDashing && Time.time >= lastDashTime + dashCooldown)
                    {
                        StartCoroutine(DoDash());
                    }
                }
                break;

            case BossState.Special:
                break;

            case BossState.Dead:
                aiMovement.enabled = false;
                break;
        }
    }

    void CheckPlayerDetection()
    {
        if (sensor.Objects.Contains(player.gameObject))
        {
            currentState = BossState.Attack;
            aiMovement.enabled = true;
        }
    }

    IEnumerator DoDash()
    {
        if (player == null) yield break;

        isDashing = true;
        lastDashTime = Time.time;

        aiMovement.enabled = false;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Vector3 dir = (player.position - transform.position).normalized;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        
        Vector3 dashTarget = transform.position + dir * (distToPlayer + dashOffset);

        if (UnityEngine.AI.NavMesh.SamplePosition(dashTarget, out var hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
        {
            dashTarget = hit.position;
        }

        float originalSpeed = agent.speed;
        agent.isStopped = false;
        agent.speed = originalSpeed * dashSpeedMultiplier;
        agent.SetDestination(dashTarget);

        while (agent.pathPending || agent.remainingDistance > 0.5f)
        {
            if (IsHittingWall() && agent.remainingDistance < 2f)
            {
	            GameManager.Instance.ShakeCamera();
                Debug.Log("Boss hit the wall!");
                StartCoroutine(DoStun());
                break;
            }
            yield return null;
        }

        agent.speed = originalSpeed;
        isDashing = false;

        if (!isStunned)
            aiMovement.enabled = true;
    }


    IEnumerator DoStun()
    {
        isStunned = true;
        isDashing = false;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.isStopped = true;
        aiMovement.enabled = false;

        // TODO: thêm animation Stun
        yield return new WaitForSeconds(stunDuration);
        currentState = BossState.Idle;
        agent.isStopped = false;
        isStunned = false;
        aiMovement.enabled = true;
    }

    bool IsHittingWall()
    {
        Vector3 origin = transform.position + Vector3.up;
        Vector3 dir = transform.forward * wallCheckDistance;

        Debug.DrawRay(origin, dir, Color.yellow);
        return Physics.Raycast(origin, transform.forward, wallCheckDistance, groundLayer);
    }
}
