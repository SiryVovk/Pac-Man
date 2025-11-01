using UnityEngine;

public class GameSesion : MonoBehaviour
{
    public static GameSesion Instance { get; private set; }

    private SaveData saveData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public SaveData GetSaveData()
    {
        return saveData;
    }

    public void SetSaveData(SaveData data)
    {
        saveData = data;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
