using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostsSpawner : MonoBehaviour
{
    [SerializeField] private GhostSO[] ghostsSO;
    [SerializeField] private Field field;
    [SerializeField] private Vector2Int exitPoint;
    [SerializeField] private Vector2Int[] onGridPositions;
    [SerializeField] private List<SkaterTarget> scaterTargets;
    [SerializeField] private PowerModeManager powerModeManager;
    [SerializeField] private GhostManager ghostManager;

    private void Start()
    {
        if (ghostsSO.Length > onGridPositions.Length)
        {
            Debug.Log("Not enought places to spawn");
        }

        int ghostNumber = 0;
        foreach (GhostSO ghostSO in ghostsSO)
        {
            SpawnGhost(ghostSO, ghostNumber);
            ghostNumber++;
        }

        if (GameSesion.Instance == null)
        {
            return;
        }

        StartCoroutine(DeleyLoadingSaveData());
    }
    
    private IEnumerator DeleyLoadingSaveData()
    {
        yield return new WaitForEndOfFrame();
        ghostManager.LoadGhostsState(GameSesion.Instance.GetSaveData());
    }

    private void SpawnGhost(GhostSO ghostSO, int ghostNumber)
    {
        Cell cell = field.GetCellAtPosition(onGridPositions[ghostNumber]);

        if (cell.Type != CellType.Empty)
        {
            Debug.LogError("Wrong place for spawn. Not empty space");
        }

        Vector3 positonForSpawn = cell.InWorldPosition;
        GameObject ghostObject = Instantiate(ghostSO.GhostObject, positonForSpawn, Quaternion.identity);

        Ghost ghost = ghostObject.GetComponent<Ghost>();

        Queue<Vector2Int> scaterTarget = scaterTargets[ghostNumber].GetPointsQueue();
        ghost.Init(cell.Position, field, ghostSO.GhostStratagy, exitPoint, scaterTarget, powerModeManager, cell.Position);

        ColorChanger colorChanger = ghostObject.GetComponent<ColorChanger>();
        colorChanger.Init(powerModeManager);
    }
}
