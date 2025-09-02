using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class AiAttack : MonoBehaviour
{
    public enum AttackType { SlimeJump, GoblinSword }

    [Header("Attack Type")]
    public AttackType attackType = AttackType.SlimeJump;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float lockTime = 2f;
    public float jumpForce = 12f;
    public int damage = 10;
    public float attackCooldown = 2f;

    [Header("References")]
    public LayerMask playerLayer;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public AiMovement aiMovement;
    public Animator animator;

    [Header("Goblin Sword Hitboxes")]
    public Collider[] swordHitboxes;

    private Transform player;
    private Rigidbody rb;

    private bool isGrounded;
    private bool isCharging = false;
    private bool isJumping = false;
    private bool hasDealtDamage = false;

    private Vector3 lockedTargetPos;
    private float lastAttackTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (aiMovement == null)
            aiMovement = GetComponent<AiMovement>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        if (attackType == AttackType.SlimeJump)
        {
            HandleSlimeAttack();
        }
        else if (attackType == AttackType.GoblinSword)
        {
            HandleGoblinAttack();
        }
    }

    // ------------------- SLIME ATTACK -------------------
    void HandleSlimeAttack()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.25f, groundLayer);

        if (!isCharging && !isJumping && Time.time >= lastAttackTime + attackCooldown)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange && aiMovement.isChasing)
            {
                StartCoroutine(ChargeAndJump());
            }
        }
    }

    IEnumerator ChargeAndJump()
    {
        isCharging = true;
        rb.linearVelocity = Vector3.zero;
        aiMovement.PauseMovement();

        lockedTargetPos = player.position;
        yield return new WaitForSeconds(lockTime);

        isJumping = true;
        hasDealtDamage = false;
        aiMovement.DisableAgent();

        Vector3 toTarget = lockedTargetPos - transform.position;
        Vector3 horizontalDir = toTarget;
        horizontalDir.y = 0f;
        horizontalDir.Normalize();

        float jumpHorizontalForce = jumpForce;
        float jumpVerticalForce = jumpForce * 0.4f;

        Vector3 jumpVector = horizontalDir * jumpHorizontalForce + Vector3.up * jumpVerticalForce;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(jumpVector, ForceMode.Impulse);

        yield return new WaitUntil(() => Physics.CheckSphere(groundCheck.position, 0.05f, groundLayer));
        yield return new WaitForSeconds(0.1f);
        rb.linearVelocity = Vector3.zero;

        aiMovement.EnableAgent();
        aiMovement.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(transform.position);
        aiMovement.ResumeMovement();

        isCharging = false;
        lastAttackTime = Time.time;

        yield return null;
        isJumping = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (attackType != AttackType.SlimeJump) return;

        if (!hasDealtDamage && collision.collider.CompareTag("Player"))
        {
            PlayerResource playerRes = collision.collider.GetComponent<PlayerResource>();
            if (playerRes != null)
            {
                playerRes.DamageHealth(damage);
                hasDealtDamage = true;
            }
        }
    }

    // ------------------- GOBLIN ATTACK -------------------
    void HandleGoblinAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && aiMovement.isChasing)
        {
            aiMovement.PauseMovement();

            Vector3 toPlayer = (player.position - transform.position).normalized;
            animator.SetFloat("AttackX", toPlayer.x);
            animator.SetFloat("AttackY", toPlayer.z);

            animator.SetTrigger("Attack");

            lastAttackTime = Time.time;
            StartCoroutine(ResumeAfterAttack(0.8f));
        }
    }

    IEnumerator ResumeAfterAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        aiMovement.ResumeMovement();
    }

    public void EnableSwordHitbox(int index)
    {
        if (attackType != AttackType.GoblinSword) return;
        if (index >= 0 && index < swordHitboxes.Length && swordHitboxes[index] != null)
            swordHitboxes[index].enabled = true;
    }

    public void DisableSwordHitbox(int index)
    {
        if (attackType != AttackType.GoblinSword) return;
        if (index >= 0 && index < swordHitboxes.Length && swordHitboxes[index] != null)
            swordHitboxes[index].enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
