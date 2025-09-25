using System;
using UnityEngine;

public class PalletManager : MonoBehaviour
{
    public Action<CellType> OnPalletEaten;
    public Action OnAllPalletEaten;

    [SerializeField] private Field field;
    [SerializeField] private CellType[] palletCellTypes;

    private int totalPallets = 0;

    private void OnEnable()
    {
        field.OnPlayerCellEnter += EatPallet;
    }

    private void OnDisable()
    {
        field.OnPlayerCellEnter -= EatPallet;
    }

    private void Start()
    {
        foreach (CellType cellType in palletCellTypes)
        {
            totalPallets += field.CountCellsOfType(cellType);
        }
    }

    private void EatPallet(Cell playerCellEnter)
    {
        if (IsPalletCell(playerCellEnter))
        {
            OnPalletEaten?.Invoke(playerCellEnter.Type);
            field.ChangeTile(playerCellEnter.LocalPosition);
            totalPallets--;
        }

        if (totalPallets <= 0)
        {
            OnAllPalletEaten?.Invoke();
        }
    }

    private bool IsPalletCell(Cell cellToCheck)
    {
        foreach (CellType cellType in palletCellTypes)
        {
            if (cellType == cellToCheck.Type)
            {
                return true;
            }
        }

        return false;
    }
}
