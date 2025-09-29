using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    public int damage = 2;
    public float lifetime = 6f;

    public void Init(Transform player, float moveSpeed)
    {
        target = player;
        speed = moveSpeed;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (target == null) return;

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerResource pr = other.GetComponent<PlayerResource>();
            if (pr != null)
            {
                pr.DamageHealth(damage);
            }
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
