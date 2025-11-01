using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseGameMenu : MonoBehaviour
{
    [SerializeField] private GameEndCondition gameEndCondition;
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private TMP_Text scoreText;

    private const int MAIN_MENU_SCENE_INDEX = 0;
    private const float PAUSED_GAME = 0f;
    private const float UNPAUSED_GAME = 1f;

    private void OnEnable()
    {
        gameEndCondition.GameLose += GameEnd;
    }

    private void GameEnd(float score)
    {
        Time.timeScale = PAUSED_GAME;
        gameEndPanel.SetActive(true);
        scoreText.text = "Score: " + score.ToString();
    }

    public void GoToMainMenu()
    {
        GameSesion.Instance.Destroy();
        Time.timeScale = UNPAUSED_GAME;
        SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
    }

    public void RestartLevel()
    {
        Time.timeScale = UNPAUSED_GAME;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
