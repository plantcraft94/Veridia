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
    // qly animation, func
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
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
        if (onDeath != null) onDeath.Invoke();
        Destroy(gameObject);
    }
}