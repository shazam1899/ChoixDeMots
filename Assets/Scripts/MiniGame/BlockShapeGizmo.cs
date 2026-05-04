
//Code réaliser par Dylan LAUNAY avec Copilot
using UnityEngine;

[ExecuteAlways]
public class BlockShapeGizmo : MonoBehaviour
{
    public Color lineColor = Color.green;

    private BlockShape shape;
    private GridBoard board;

    private void OnDrawGizmos() // Dessine les gizmos pour visualiser les cellules occupées par le bloc dans l'éditeur, en utilisant une matrice de transformation qui applique la position et la rotation du bloc mais ignore l'échelle pour assurer que les gizmos représentent correctement les cellules de la grille
    {
        if (shape == null)
            shape = GetComponent<BlockShape>();

        if (board == null)
            board = FindFirstObjectByType<GridBoard>();

        if (shape == null || shape.localCells == null || board == null)
            return;

        float cs = board.cellSize; // taille d'une cellule

        Gizmos.color = lineColor;

        //MATRICE : position + rotation Y/Z du bloc, mais X = 0
        Matrix4x4 forcedMatrix =
            Matrix4x4.TRS(
                transform.position,
                Quaternion.Euler(0f, transform.eulerAngles.y, transform.eulerAngles.z),
                Vector3.one //pas de scale du bloc
            );

        Matrix4x4 old = Gizmos.matrix;
        Gizmos.matrix = forcedMatrix;

        foreach (var cell in shape.localCells)
        {
            // Position locale
            Vector3 center = new Vector3(cell.x * cs, 0f, cell.y * cs);

            // Coins du carré
            Vector3 r = new Vector3(cs / 2f, 0f, 0f);
            Vector3 f = new Vector3(0f, 0f, cs / 2f);

            Vector3 p1 = center - r - f;
            Vector3 p2 = center + r - f;
            Vector3 p3 = center + r + f;
            Vector3 p4 = center - r + f;

            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);
        }

        Gizmos.matrix = old;
    }
}
