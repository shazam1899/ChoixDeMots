//Code réaliser par Dylan LAUNAY avec Copilot
using UnityEngine;

public class GridBoardGizmo : MonoBehaviour
{
    public GridBoard board;

    private void OnDrawGizmos() // Dessine les gizmos pour visualiser la grille du board dans l'éditeur en parcourant les dimensions du board et en dessinant un cube filaire à la position de chaque cellule en utilisant la taille des cellules pour déterminer l'espacement
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
