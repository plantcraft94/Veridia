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
    private AiHealth aiHealth;

    [Header("Phase Settings")]
    [Range(0.01f, 0.5f)]
    public float phase2ThresholdRatio = 0.2f;
    private bool isPhase2 = false;

    public GameObject crystalParent;

    [Header("Dash Settings")]
    public float dashOffset = 3f;
    public float dashSpeedMultiplier = 5f;
    public float dashCooldown = 5f;
    private float lastDashTime = -Mathf.Infinity;
    private bool isDashing = false;
    private bool hasDetectedPlayer = false;

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
        aiHealth = GetComponent<AiHealth>();

        if (crystalParent != null)
            crystalParent.SetActive(false);

        if (aiHealth != null)
        {
            aiHealth.onDamage.AddListener(OnDamaged);
            aiHealth.onDeath.AddListener(OnDeath);
        }
    }

    void Update()
    {
        if (isStunned) return;

        // Kiểm tra phase 2
        if (!isPhase2 && aiHealth != null)
        {
            float ratio = aiHealth.currentHealth / Mathf.Max(1f, aiHealth.maxHealth);
            if (ratio <= phase2ThresholdRatio)
                EnterPhase2();
        }

        switch (currentState)
        {
            case BossState.Idle:
                CheckPlayerDetection();
                break;

            case BossState.Attack:
                // Boss đã từng phát hiện player -> liên tục tấn công
                if (hasDetectedPlayer && !isDashing && Time.time >= lastDashTime + dashCooldown)
                {
                    StartCoroutine(DoDash());
                }
                break;

            case BossState.Special:
                // Phase 2 behavior (crystals)
                break;

            case BossState.Dead:
                aiMovement.enabled = false;
                break;
        }
    }

    void CheckPlayerDetection()
    {
        if (player == null || sensor == null) return;

        if (sensor.Objects.Contains(player.gameObject))
        {
            hasDetectedPlayer = true; // chỉ cần thấy 1 lần
            currentState = BossState.Attack;

            if (!isPhase2)
                aiMovement.enabled = true;
        }
    }

    // -----------------------------------
    // Các hàm khác giữ nguyên
    // -----------------------------------

    void OnDamaged()
    {
        if (aiHealth == null) return;
        float ratio = aiHealth.currentHealth / Mathf.Max(1f, aiHealth.maxHealth);
        if (!isPhase2 && ratio <= phase2ThresholdRatio)
            EnterPhase2();
    }

    void OnDeath()
    {
        currentState = BossState.Dead;
        aiMovement.enabled = false;
    }

    void EnterPhase2()
    {
        isPhase2 = true;
        currentState = BossState.Special;

        if (aiMovement != null)
            aiMovement.enabled = false;

        if (crystalParent != null)
            crystalParent.SetActive(true);

        Debug.Log("[BossController] Entered Phase 2: crystals activated.");
    }

    IEnumerator DoDash()
    {
        if (player == null) yield break;

        isDashing = true;
        lastDashTime = Time.time;

        if (aiMovement != null)
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
                Debug.Log("Boss hit the wall!");
                StartCoroutine(DoStun());
                break;
            }
            yield return null;
        }

        agent.speed = originalSpeed;
        isDashing = false;

        if (!isStunned && !isPhase2)
            aiMovement.enabled = true;
    }

    IEnumerator DoStun()
    {
        isStunned = true;
        isDashing = false;

        GameManager.Instance.ShakeCamera();
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.isStopped = true;
        if (aiMovement != null) aiMovement.enabled = false;

        yield return new WaitForSeconds(stunDuration);

        currentState = BossState.Attack; // sau khi stun xong, quay lại tấn công
        if (agent != null) agent.isStopped = false;
        isStunned = false;

        if (!isPhase2 && aiMovement != null)
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
