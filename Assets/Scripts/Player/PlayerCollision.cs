using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ghost>())
        {
            health.TakeDamage();
        }
    }
}
