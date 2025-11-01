using System;
using UnityEngine;

public static class ProgressManager
{
    private const string LEVEL_STRING = "Level_";
    private const string LEVEL_COMPLIT_STRING = "_Complited";

    private const int LEVEL_COMPLITED = 1;
    private const int LEVEL_NOT_COMPLITED = 0;

    public static void SetLevelComplition(int levelIndex)
    {
        PlayerPrefs.SetInt(LEVEL_STRING + levelIndex + LEVEL_COMPLIT_STRING, LEVEL_COMPLITED);
    }
    
    public static bool IsLevelComplited(int levelIndex)
    {
        return PlayerPrefs.GetInt(LEVEL_STRING + levelIndex + LEVEL_COMPLIT_STRING, LEVEL_NOT_COMPLITED) == LEVEL_COMPLITED;
    }
}
