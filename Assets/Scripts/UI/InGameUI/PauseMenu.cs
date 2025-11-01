using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Field field;

    private bool isOnPause = false;

    private const float PAUSE_TIME = 0f;
    private const float ACTIVE_TIME = 1f;

    private const int MAIN__MENU_SCENE_INDEX = 0;
    private void OnEnable()
    {
        UIInputs.PauseAction += Pause;
    }

    private void OnDisable()
    {
        UIInputs.PauseAction -= Pause;
    }

    public void Pause()
    {
        isOnPause = !isOnPause;

        pauseMenu.SetActive(isOnPause);
        Time.timeScale = isOnPause ? PAUSE_TIME : ACTIVE_TIME;
    }

    public void ReturnToMenu()
    {
        if (GameSesion.Instance != null)
        {
            GameSesion.Instance.Destroy();
        }

        Save();
        Time.timeScale = ACTIVE_TIME;
        SceneManager.LoadScene(MAIN__MENU_SCENE_INDEX);
    }

    private void Save()
    {
        SaveManager.DeleteSave();

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Field field = this.field;
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        Score score = FindFirstObjectByType<Score>();
        Health health = FindFirstObjectByType<Health>();
        GhostManager ghostManager = FindFirstObjectByType<GhostManager>();
        PowerModeManager powerModeManager = FindFirstObjectByType<PowerModeManager>();
        SaveManager.SaveGame(sceneIndex, field, player, score, health, ghostManager, powerModeManager);
    }
}
