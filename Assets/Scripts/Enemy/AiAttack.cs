using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class AiAttack : MonoBehaviour
{
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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (aiMovement == null)
            aiMovement = GetComponent<AiMovement>();
    }

    void Update()
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
        Debug.Log("ChargeAndJump started");
        isCharging = true;
        rb.linearVelocity = Vector3.zero;
        aiMovement.PauseMovement();

        lockedTargetPos = player.position;
        yield return new WaitForSeconds(lockTime);

        Debug.Log("Attempt jump");
        isJumping = true;
        hasDealtDamage = false; 
        aiMovement.DisableAgent();

        Vector3 toTarget = lockedTargetPos - transform.position;
        Vector3 horizontalDir = toTarget;
        horizontalDir.y = 0f;
        horizontalDir.Normalize();

        float jumpHorizontalForce = jumpForce;
        float jumpVerticalForce = jumpForce * 1.8f;

        Vector3 jumpVector = horizontalDir * jumpHorizontalForce + Vector3.up * jumpVerticalForce;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(jumpVector, ForceMode.Impulse);

        yield return new WaitUntil(() => Physics.CheckSphere(groundCheck.position, 0.05f, groundLayer));

        isCharging = false;
        lastAttackTime = Time.time;
        aiMovement.EnableAgent();
        aiMovement.ResumeMovement();

        yield return null;
        isJumping = false; 
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.collider.name);
        if (!hasDealtDamage && collision.collider.CompareTag("Player"))
        {
            PlayerResource playerRes = collision.collider.GetComponent<PlayerResource>();
            
                Debug.Log("Dealing damage to player");
                playerRes.DamageHealth(damage);
                hasDealtDamage = true;
            
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
