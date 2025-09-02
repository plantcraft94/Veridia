using UnityEngine;

public class EnemyWeaponHitbox : MonoBehaviour
{
    public AiAttack ownerAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;
        if (other.CompareTag("Player"))
        {
            PlayerResource playerRes = other.GetComponent<PlayerResource>();
            if (playerRes != null)
            {
                playerRes.DamageHealth(ownerAttack.damage);
                Debug.Log("Goblin sword hit player!");
            }
        }
    }
}
