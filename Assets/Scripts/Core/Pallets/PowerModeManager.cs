using System;
using System.Collections;
using UnityEngine;

public class PowerModeManager : MonoBehaviour
{
    public Action OnPowerUpModStarted;
    public Action OnPowerUpModEnded;
    public Action<float,float> OnPowerModeTick;

    [SerializeField] private PalletManager palletManager;

    [SerializeField] private float timeOfPowerUp = 10f;

    private Coroutine powerUpRoutine;
    private float powerModeTimeLeft = 0f;

    private const CellType POWER_PALLET_TYPE = CellType.PowerPallet;

    private void OnEnable()
    {
        palletManager.OnPalletEaten += HandlePallets;
    }

    private void OnDisable()
    {
        palletManager.OnPalletEaten -= HandlePallets;
    }

    private void Start()
    {
        if(GameSesion.Instance != null)
        {
            SaveData saveData = GameSesion.Instance.GetSaveData();
            if (saveData.powerModeActive)
            {
                ActivatePowerMode(saveData.powerModeTimeLeft);
            }
        }
    }
    private void HandlePallets(CellType cellType)
    {
        if (cellType == POWER_PALLET_TYPE)
        {
            ActivatePowerMode();
        }
    }

    private void ActivatePowerMode()
    {
        if (powerUpRoutine != null)
        {
            StopCoroutine(powerUpRoutine);
        }

        powerUpRoutine = StartCoroutine(PowerUpRoutine());
    }

    private void ActivatePowerMode(float savedTime)
    {
        if (powerUpRoutine != null)
        {
            StopCoroutine(powerUpRoutine);
        }

        powerUpRoutine = StartCoroutine(PowerUpRoutine(savedTime));
    }

    private IEnumerator PowerUpRoutine()
    {
        OnPowerUpModStarted?.Invoke();

        powerModeTimeLeft = timeOfPowerUp;

        while (powerModeTimeLeft > 0)
        {
            powerModeTimeLeft -= Time.deltaTime;
            OnPowerModeTick?.Invoke(powerModeTimeLeft, timeOfPowerUp);
            yield return null;
        }

        OnPowerUpModEnded?.Invoke();
        powerUpRoutine = null;
    }

    private IEnumerator PowerUpRoutine(float savedTime)
    {
        OnPowerUpModStarted?.Invoke();

        powerModeTimeLeft = savedTime;

        while (powerModeTimeLeft > 0)
        {
            powerModeTimeLeft -= Time.deltaTime;
            OnPowerModeTick?.Invoke(powerModeTimeLeft, timeOfPowerUp);
            yield return null;
        }

        OnPowerUpModEnded?.Invoke();
        powerUpRoutine = null;
    }

    public bool IsPowerModeActive()
    {
        return powerUpRoutine != null;
    }
    
    public float GetPowerModeTimeLeft()
    {
        return powerModeTimeLeft;
    }
}
