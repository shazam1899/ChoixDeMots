using System.Collections.Generic;
using UnityEngine;

public class BlockShape : MonoBehaviour
{
    public List<Vector2Int> localCells = new List<Vector2Int>();

    public List<Vector2Int> GetWorldCells(GridBoard board, Vector3 worldPos, Quaternion rotation)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        foreach (var cell in localCells)
        {
            Vector3 rotated = rotation * new Vector3(cell.x, 0, cell.y);
            Vector3 cellWorldPos = worldPos + rotated;
            Vector2Int gridPos = board.WorldToGrid(cellWorldPos);
            result.Add(gridPos);
        }

        return result;
    }
}
