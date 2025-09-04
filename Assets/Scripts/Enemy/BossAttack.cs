using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour
{
    [Header("Shockwave Attack Settings")]
    public int shockwaveDamage = 3;
    public float shockwaveCooldown = 5f;
    public float shockwaveCastRange = 10f;
    public float shockwaveDamageDelay = 0.5f;
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
    public LayerMask playerLayer;
    public ParticleSystem shockwaveParticle;

    [Header("Projectile Attack Settings")]
    public GameObject projectilePrefab;
    public float projectileCooldown = 3f;
    public float projectileSpeed = 6f;
    public float projectileCastRange = 12f;

    private float lastShockwaveTime = -Mathf.Infinity;
    private float lastProjectileTime = -Mathf.Infinity;

    Animator animator;
    Transform player;
    public AiSensor sensor;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        sensor = GetComponent<AiSensor>();
        if (shockwaveParticle != null)
            shockwaveParticle.Stop();
    }

    void Update()
    {
        if (player == null) return;
        //Debug.Log("sensor.Objects.Count");

        bool canSeePlayer = sensor.Objects.Contains(player.gameObject);
        if (!canSeePlayer) return;
        //Debug.Log("Can see player");

        float dist = Vector3.Distance(transform.position, player.position);

        // Shockwave Attack
        if (Time.time >= lastShockwaveTime + shockwaveCooldown && dist <= shockwaveCastRange)
        {
            animator.SetTrigger("Shockwave");
            lastShockwaveTime = Time.time;
        }

        // Projectile Attack
        if (Time.time >= lastProjectileTime + projectileCooldown && dist <= projectileCastRange)
        {
            ShootProjectile();
            lastProjectileTime = Time.time;
        }
    }

    public void DoShockwave()
    {
        if (shockwaveParticle != null) shockwaveParticle.Play();


        StartCoroutine(DelayedShockwaveDamage());
    }

    IEnumerator DelayedShockwaveDamage()
    {
        yield return new WaitForSeconds(shockwaveDamageDelay);

        Collider[] hits = Physics.OverlapSphere(transform.position, shockwaveCastRange, playerLayer);
        foreach (var hit in hits)
        {
            PlayerMovement pm = hit.GetComponent<PlayerMovement>();
            if (pm != null && !pm.isJumping)
            {
                PlayerResource pr = pm.GetComponent<PlayerResource>();
                if (pr != null)
                {
                    pr.DamageHealth(shockwaveDamage);
                    ApplyKnockback(hit.transform);
                }
            }
        }

        if (shockwaveParticle != null) shockwaveParticle.Stop();
    }

    void ApplyKnockback(Transform target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
            StartCoroutine(TemporaryDisableMovement(target, knockbackDuration));
        }
    }

    IEnumerator TemporaryDisableMovement(Transform target, float duration)
    {
        PlayerMovement pMove = target.GetComponent<PlayerMovement>();
        if (pMove != null)
        {
            pMove.enabled = false;
            yield return new WaitForSeconds(duration);
            pMove.enabled = true;
        }
    }

    public void ShootProjectile()
    {
        if (projectilePrefab == null || player == null) return;

        GameObject proj = Instantiate(projectilePrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
        BossProjectile bp = proj.GetComponent<BossProjectile>();
        if (bp != null)
        {
            bp.Init(player, projectileSpeed);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shockwaveCastRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, projectileCastRange);
    }
}
