using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void SaveGame(int levelIndex, Field field, PlayerMovement player, Score score, Health health, GhostManager ghostManager, PowerModeManager powerModeManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(levelIndex,field, player, score, health, ghostManager, powerModeManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void DeleteSave()
    {
        string path = Application.persistentDataPath + "/save.dat";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    
    public static SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/save.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static bool HasSave()
    {
        string path = Application.persistentDataPath + "/save.dat";
        return File.Exists(path);
    }
}
