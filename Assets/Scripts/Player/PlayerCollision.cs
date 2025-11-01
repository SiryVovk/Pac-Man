using System;
using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Health health;
    private PowerModeManager powerModeManager;

    private bool canEatGhosts = false;


    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void Init(PowerModeManager powerModeManager)
    {
        this.powerModeManager = powerModeManager;

        powerModeManager.OnPowerUpModStarted += PowerUpStarted;
        powerModeManager.OnPowerUpModEnded += PowerUpEnded;
    }

    private void OnDestroy()
    {
        powerModeManager.OnPowerUpModStarted -= PowerUpStarted;
        powerModeManager.OnPowerUpModEnded -= PowerUpEnded;
    }

    private void PowerUpStarted()
    {
        canEatGhosts = true;
    }

    private void PowerUpEnded()
    {
        canEatGhosts = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ghost ghost = collision.GetComponent<Ghost>();
        if (!ghost)
        {
            return;
        }

        if (!canEatGhosts)
        {
            health.TakeDamage();
        }
        else
        {
            ghost.EatenByPlayer();
        }
    }
}
