using UnityEngine;
using UnityEngine.AI;
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

    [Header("Escape Settings After Shockwave")]
    public Transform waypoint1;
    public Transform waypoint2;
    public float escapeSpeedMultiplier = 2f;

    private float lastShockwaveTime = -Mathf.Infinity;
    private float lastProjectileTime = -Mathf.Infinity;

    Animator animator;
    Transform player;
    AiSensor sensor;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        sensor = GetComponent<AiSensor>();
        if (shockwaveParticle != null)
            shockwaveParticle.Stop();

        if (waypoint1 == null || waypoint2 == null)
        {
            Transform room = transform.parent;
            if (room != null)
            {
                Transform wp1 = room.Find("Waypoint1");
                Transform wp2 = room.Find("Waypoint2");
                waypoint1 = wp1;
                waypoint2 = wp2;
            }
        }
    }


    void Update()
    {
        if (player == null) return;

        bool canSeePlayer = sensor.Objects.Contains(player.gameObject);
        if (!canSeePlayer) return;

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

        StartCoroutine(EscapeAfterShockwave());
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

    IEnumerator EscapeAfterShockwave()
    {
        AiMovement aiMove = GetComponent<AiMovement>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent == null || waypoint1 == null || waypoint2 == null) yield break;

        float dist1 = Vector3.Distance(transform.position, waypoint1.position);
        float dist2 = Vector3.Distance(transform.position, waypoint2.position);
        Transform targetWaypoint = (dist1 > dist2) ? waypoint1 : waypoint2;

        float originalSpeed = agent.speed;

        if (aiMove != null) aiMove.isOverridden = true;

        agent.isStopped = false;
        agent.ResetPath();
        agent.speed = originalSpeed * escapeSpeedMultiplier;
        agent.SetDestination(targetWaypoint.position);

        while (agent.pathPending || agent.remainingDistance > 0.5f)
        {
            yield return null;
        }

        agent.speed = originalSpeed;

        if (aiMove != null)
        {
            aiMove.isOverridden = false;
            aiMove.ResumeMovement();
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

        if (waypoint1 != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(waypoint1.position, 0.5f);
        }
        if (waypoint2 != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(waypoint2.position, 0.5f);
        }
    }
}
