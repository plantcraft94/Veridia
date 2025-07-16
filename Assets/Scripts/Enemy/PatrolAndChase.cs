using UnityEngine;

public class PatrolAndChase : MonoBehaviour
{
    [Header("General Settings")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 8f;
    public float attackRange = 2f;

    public float jumpForce = 10f;

    public LayerMask playerLayer;

    private Transform player;
    private Rigidbody rb;

    private Vector3 patrolDirection;
    private float patrolChangeInterval = 3f;
    private float patrolTimer;

    private bool isGrounded;
    private bool isAttacking = false;


    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ChooseNewPatrolDirection();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
        if (isAttacking) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                JumpAttack();
            }
        }
        else
        {
            Patrol();
        }
    }


    void Patrol()
    {
        patrolTimer += Time.deltaTime;

        Vector3 moveDir = new Vector3(patrolDirection.x, 0, patrolDirection.z);
        rb.linearVelocity = new Vector3(moveDir.x * patrolSpeed, rb.linearVelocity.y, moveDir.z * patrolSpeed);

        if (patrolTimer >= patrolChangeInterval)
        {
            ChooseNewPatrolDirection();
        }
    }

    void ChooseNewPatrolDirection()
    {
        patrolTimer = 0f;
        patrolDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector3(direction.x * chaseSpeed, rb.linearVelocity.y, direction.z * chaseSpeed);
    }

    void JumpAttack()
    {
        if (isGrounded && !isAttacking)
        {
            isAttacking = true;

            Vector3 direction = (player.position - transform.position).normalized;

            rb.linearVelocity = new Vector3(direction.x * chaseSpeed, rb.linearVelocity.y, direction.z * chaseSpeed);

            Vector3 jumpDir = new Vector3(direction.x, 1, direction.z).normalized;
            rb.AddForce(jumpDir * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetAttack), 1.5f);
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    void ResetAttack()
    {
        isAttacking = false;
    }

}