using System.Collections.Generic;
using UnityEngine;

public class BlockShape : MonoBehaviour
{
    public List<Vector2Int> localCells = new List<Vector2Int>();

    // Trouve le pivot bas-gauche automatiquement
    public Vector2Int GetPivot()
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;

        foreach (var c in localCells)
        {
            if (c.x < minX) minX = c.x;
            if (c.y < minY) minY = c.y;
        }

        return new Vector2Int(minX, minY);
    }

    public List<Vector2Int> GetWorldCells(GridBoard board, Vector3 worldPos, Quaternion rotation)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        foreach (var cell in localCells)
        {
            Vector3 local = new Vector3(cell.x * board.cellSize, 0, cell.y * board.cellSize);
            Vector3 rotated = rotation * local;
            Vector3 world = worldPos + rotated;

            Vector2Int grid = board.WorldToGrid(world);
            result.Add(grid);
        }

        return result;
    }
}
