using UnityEngine;

public class FixedBlockInitializer : MonoBehaviour
{
    private void Start()
    {
        GridBoard board = FindFirstObjectByType<GridBoard>();
        BlockShape shape = GetComponent<BlockShape>();

        Vector2Int basePos = board.WorldToGrid(transform.position);
        Quaternion rot = transform.rotation;

        var cells = shape.GetWorldCells(board, transform.position, rot);

        foreach (var c in cells)
        {
            if (board.IsInside(c.x, c.y))
                board.Occupy(c.x, c.y);
        }
    }
}
