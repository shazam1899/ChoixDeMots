using UnityEngine;

public class GridBoardGizmo : MonoBehaviour
{
    public GridBoard board;

    private void OnDrawGizmos()
    {
        if (board == null) board = GetComponent<GridBoard>();
        if (board == null) return;

        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                Gizmos.color = Color.green;

                Vector3 pos = board.GridToWorld(x, y);

                Gizmos.DrawWireCube(
                    pos,
                    new Vector3(board.cellSize, 0.01f, board.cellSize)
                );
            }
        }
    }
}
