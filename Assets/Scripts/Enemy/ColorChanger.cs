using System.Collections;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer enemyRenderer;
    [SerializeField] private Color frightenedColor = Color.blue;

    [SerializeField] private float blinkTime = 0.25f;

    private PowerModeManager powerModeManager;
    private Color normalColor;
    private Coroutine blinkCoroutine;

    private const float FORTH_PART_OF_POWERUP_TIME = 0.25f;

    private void Awake()
    {
        normalColor = enemyRenderer.color;
    }
    
    public void Init(PowerModeManager powerModeManager)
    {
        this.powerModeManager = powerModeManager;

        powerModeManager.OnPowerUpModStarted += ChangeColorToPowerUp;
        powerModeManager.OnPowerUpModEnded += ChangeColorToNormal;
        powerModeManager.OnPowerModeTick += ChangeColorToPowerModeTick;
    }

    private void ChangeColorToPowerUp()
    {
        enemyRenderer.color = frightenedColor;

        if(blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    private void ChangeColorToPowerModeTick(float timeRemainin, float totalTime)
    {
        if (timeRemainin / totalTime > FORTH_PART_OF_POWERUP_TIME)
        {
            return;
        }

        if (blinkCoroutine != null)
        {
            return;
        }
        
        blinkCoroutine = StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            enemyRenderer.color = normalColor;
            yield return new WaitForSeconds(blinkTime);
            enemyRenderer.color = frightenedColor;
            yield return new WaitForSeconds(blinkTime);
        }
    }

    private void ChangeColorToNormal()
    {
        StopCoroutine(blinkCoroutine);
        blinkCoroutine = null;

        enemyRenderer.color = normalColor;
    }
}
