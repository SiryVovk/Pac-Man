using Unity.VisualScripting;
using UnityEngine;

public class GhostsSpawner : MonoBehaviour
{
    [SerializeField] private GhostSO[] ghostsSO;
    [SerializeField] private Field field;
    [SerializeField] private Vector2Int exitPoint;
    [SerializeField] private Vector2Int[] onGridPositions;

    private void Start()
    {
        if (ghostsSO.Length > onGridPositions.Length)
        {
            Debug.Log("Not enought places to spawn");
        }

        int ghostNumber = 0;
        foreach (GhostSO ghostSO in ghostsSO)
        {
            SpawnGhost(ghostSO,ghostNumber);
            ghostNumber++;
        }
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
        ghost.Init(cell.Position, field, ghostSO.GhostStratagy, exitPoint);
    }
}
