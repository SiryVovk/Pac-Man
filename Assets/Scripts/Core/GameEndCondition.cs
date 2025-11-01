using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndCondition : MonoBehaviour
{
    public event Action<float> GameWin;
    public event Action<float> GameLose;

    [SerializeField] private Score score;
    [SerializeField] private PalletManager palletManager;

    private Health playerHealth;

    private void OnEnable()
    {
        palletManager.OnAllPalletEaten += TogleGameWin;
    }

    private void OnDisable()
    {
        playerHealth.OnPlayerDied -= TogleGameLose;
        palletManager.OnAllPalletEaten -= TogleGameWin;
    }

    public void InitHealthComponent(Health health)
    {
        playerHealth = health;

        playerHealth.OnPlayerDied += TogleGameLose;
    }
    
    private void TogleGameLose()
    {
        SaveManager.DeleteSave();
        GameLose?.Invoke(score.GetScore());
    }
    
    private void TogleGameWin()
    {
        SaveManager.DeleteSave();
        ProgressManager.SetLevelComplition(SceneManager.GetActiveScene().buildIndex);
        GameWin?.Invoke(score.GetScore());
    }
}
