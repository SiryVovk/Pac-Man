using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    public Action<int> OnScoreChange;

    [SerializeField] private PalletManager palletManager;

    [SerializeField] private int palletCost = 10;
    [SerializeField] private int powerPalletCost = 25;
    [SerializeField] private int ghostEatCost = 200;

    private int score;

    private void OnEnable()
    {
        palletManager.OnPalletEaten += OnCellEatenAddScore;
    }

    private void Start()
    {
        GhostManager.Instance.OnGhostEaten += GhostEatenAddScore;

        if(GameSesion.Instance != null)
        {
            SaveData saveData = GameSesion.Instance.GetSaveData();
            LoadStartingScore((int)saveData.score);
        }
    }

    private void OnDisable()
    {
        palletManager.OnPalletEaten -= OnCellEatenAddScore;
    }

    private void OnDestroy()
    {
        if (GhostManager.Instance != null)
        {
            GhostManager.Instance.OnGhostEaten -= GhostEatenAddScore;
        }
    }

    private void OnCellEatenAddScore(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Pallet:
                score += palletCost;
                break;
            case CellType.PowerPallet:
                score += powerPalletCost;
                break;
            default:
                Debug.Log("Cell type undefined");
                break;
        }

        OnScoreChange?.Invoke(score);
    }

    private void GhostEatenAddScore()
    {
        score += ghostEatCost;

        OnScoreChange?.Invoke(score);
    }

    private void LoadStartingScore(int startingScore)
    {
        score = startingScore;
        OnScoreChange?.Invoke(score);
    }

    public float GetScore()
    {
        return score;
    }
}
