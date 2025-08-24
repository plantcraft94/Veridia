using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AiHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 20f;
    public float currentHealth;

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent onDamage;

    [Header("Drop Settings")]
    public GameObject[] dropPrefabs;
    public int minDrop = 1;
    public int maxDrop = 2;

    [Header("Damage Sources Allowed")]
    public List<string> allowedDamageTags = new List<string>() { "PlayerSword", "PlayerArrow", "Bomb" };
    public bool TakeDamageFromThrowable = true;
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, GameObject damageSource)
    {
        if (damageSource != null)
        {
            string sourceTag = damageSource.tag;
            if (!allowedDamageTags.Contains(sourceTag))
            {
                if(TakeDamageFromThrowable == false || damageSource.GetComponent<ThrowableObject>() == null)
                {
                    Debug.Log("nana");
                    return;
                }
            }
        }

        currentHealth -= amount;
        if (onDamage != null) onDamage.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void Die()
    {
        DropItems();

        if (onDeath != null) onDeath.Invoke();
        Destroy(gameObject);
    }

    private void DropItems()
    {
        if (dropPrefabs == null || dropPrefabs.Length == 0) return;
        int dropCount = Random.Range(minDrop, maxDrop + 1);

        for (int i = 0; i < dropCount; i++)
        {
            var prefab = dropPrefabs[Random.Range(0, dropPrefabs.Length)];
            Vector3 dropPos = transform.position + Random.insideUnitSphere * 0.5f;
            dropPos.y = transform.position.y;
            Instantiate(prefab, dropPos, Quaternion.identity);
        }
    }
}