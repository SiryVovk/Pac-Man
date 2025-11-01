using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private GameObject healthLayautGroup;

    private GameObject[] healthIcons;
    private Health boundHealth;

    private void Awake()
    {
        healthIcons = new GameObject[healthLayautGroup.transform.childCount];
        for (int i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i] = healthLayautGroup.transform.GetChild(i).gameObject;
        }
    }

    public void BindTo(Health health)
    {
        if (boundHealth != null)
        {
            boundHealth.OnHealthChanged -= ChangeHealth;
        }

        boundHealth = health;

        if (boundHealth != null)
        {
            boundHealth.OnHealthChanged += ChangeHealth;
            ChangeHealth(boundHealth.GetCurrentHealth());
        }
    }
    
    private void ChangeHealth(int currentHealth)
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i].SetActive(i < currentHealth);
        }
    }
}
