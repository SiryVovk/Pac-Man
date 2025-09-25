using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public  event Action<int> OnHealthChanged;
    public  event Action OnPlayerDied;

    [SerializeField] private int maxHealth = 3;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage()
    {
        currentHealth--;
        OnHealthChanged?.Invoke(GetCurrentHealth());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnPlayerDied?.Invoke();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
