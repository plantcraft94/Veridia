using UnityEngine;

public class AiAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage = 10;
    public Animator anim;
    public Transform targetTransform;

    private float lastAttackTime = -Mathf.Infinity;

    void Update()
    {
        if (targetTransform == null) return;

        float dist = Vector3.Distance(transform.position, targetTransform.position);
        if (dist <= attackRange)
        {
            transform.LookAt(new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z));

            if (Time.time - lastAttackTime > attackCooldown)
            {
                lastAttackTime = Time.time;
                DoAttack(targetTransform);

                if (anim != null) anim.SetTrigger("Attack");
            }
        }
    }

    void DoAttack(Transform target)
    {
        var playerResource = target.GetComponent<PlayerResource>();
        if (playerResource != null)
        {
            playerResource.DamageHealth(damage);
        }
    }
}