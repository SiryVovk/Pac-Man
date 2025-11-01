using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsMenuUI : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;

    private void Start()
    {
        SetLevelButtonsActive();
    }

    private void SetLevelButtonsActive()
    {
        for (int i = 0; i < levelButtons.Length - 1; i++)
        {
            levelButtons[i + 1].interactable = ProgressManager.IsLevelComplited(i + 1);
        }
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
