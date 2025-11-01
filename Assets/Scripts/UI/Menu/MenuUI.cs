using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject gameSessionPrefab;

    [SerializeField] private Button continueButton;

    private GameObject currentMenu;

    private void Start()
    {
        continueButton.interactable = SaveManager.HasSave();
    }

    public void ContinueButton()
    {
        SaveData data = SaveManager.LoadGame();
        
        if (data != null)
        {
            GameObject sessionObj = Instantiate(gameSessionPrefab);
            GameSesion session = sessionObj.GetComponent<GameSesion>();
            session.SetSaveData(data);
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(data.sceneIndex);
        }
    }
    
    public void LevelsButton()
    {
        currentMenu = levelMenu;

        levelMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void SettingsButton()
    {
        currentMenu = settingMenu;

        settingMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BackButton()
    {
        currentMenu.SetActive(false);
        mainMenu.SetActive(true);

        currentMenu = null;
    }
}
