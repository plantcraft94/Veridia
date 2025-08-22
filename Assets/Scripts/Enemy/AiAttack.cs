using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class AiAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 2f;          // khoảng cách bắt đầu charge
    public float lockTime = 2f;             // thời gian ghim mục tiêu trước khi nhảy
    public float jumpForce = 12f;           // lực nhảy
    public int damage = 10;                 // damage gây ra

    [Header("References")]
    public LayerMask playerLayer;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public AiMovement aiMovement;           // tham chiếu tới script di chuyển

    private Transform player;
    private Rigidbody rb;

    private bool isGrounded;
    private bool isCharging = false;
    private bool isJumping = false;
    private Vector3 lockedTargetPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (aiMovement == null)
            aiMovement = GetComponent<AiMovement>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        // chỉ check tấn công khi không tấn công trước đó
        if (!isCharging && !isJumping)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                StartCoroutine(ChargeAndJump());
            }
        }
    }

    IEnumerator ChargeAndJump()
    {
        isCharging = true;
        rb.linearVelocity = Vector3.zero;

        // khoá AI movement khi charge
        if (aiMovement != null)
            aiMovement.enabled = false;

        // ghim vị trí player
        lockedTargetPos = player.position;

        // chờ lockTime giây (Player có thể di chuyển tránh)
        yield return new WaitForSeconds(lockTime);

        // bắt đầu nhảy
        if (isGrounded)
        {
            isJumping = true;
            Vector3 dir = (lockedTargetPos - transform.position).normalized;
            Vector3 jumpDir = new Vector3(dir.x, 1f, dir.z).normalized;
            rb.AddForce(jumpDir * jumpForce, ForceMode.Impulse);
        }

        // chờ slime hạ xuống đất
        while (!isGrounded)
        {
            yield return null;
        }

        // kết thúc tấn công → mở lại AI movement
        isJumping = false;
        isCharging = false;
        if (aiMovement != null)
            aiMovement.enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isJumping && collision.collider.CompareTag("Player"))
        {
            PlayerResource playerRes = collision.collider.GetComponent<PlayerResource>();
            if (playerRes != null)
            {
                playerRes.DamageHealth(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
