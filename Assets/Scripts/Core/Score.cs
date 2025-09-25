using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    public Action<int> OnScoreChange;

    [SerializeField] private PalletManager palletManager;

    [SerializeField] private int palletCost = 10;
    [SerializeField] private int powerPalletCost = 25;

    private int score;

    private void OnEnable()
    {
        palletManager.OnPalletEaten += OnCellEatenAddScore;
    }

    private void OnDisable()
    {
        palletManager.OnPalletEaten -= OnCellEatenAddScore;
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

    public float GetScore()
    {
        return score;
    }
}
