using UnityEngine.UI;
using UnityEngine;
using System;

public class PowerUpUI : MonoBehaviour
{
    [SerializeField] private PowerModeManager powerModeManager;
    [SerializeField] private Image timerImage;

    private void Awake()
    {
        timerImage.enabled = false;
    }
    
    private void OnEnable()
    {
        powerModeManager.OnPowerUpModStarted += HandlePowerUpStart;
        powerModeManager.OnPowerUpModEnded += HandlePowerUpEnd;
        powerModeManager.OnPowerModeTick += HandlePowerUpTick;
    }

    private void OnDisable()
    {
        powerModeManager.OnPowerUpModStarted -= HandlePowerUpStart;
        powerModeManager.OnPowerUpModEnded -= HandlePowerUpEnd;
        powerModeManager.OnPowerModeTick -= HandlePowerUpTick;
    }

    private void HandlePowerUpStart()
    {
        timerImage.fillAmount = 1f;
        timerImage.enabled = true;
    }

    private void HandlePowerUpEnd()
    {
        timerImage.fillAmount = 0;
        timerImage.enabled = false;
    }
    
    private void HandlePowerUpTick(float timeLeft, float totalDuration)
    {
        timerImage.fillAmount = Mathf.Clamp01(timeLeft/totalDuration);
    }
}
