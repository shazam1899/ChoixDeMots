using System.Collections.Generic;
using UnityEngine;

public class BlockShape : MonoBehaviour
{
    public Vector2Int[] localCells; // positions locales du bloc (ex: L, I, carré)

    public List<Vector2Int> GetWorldCells(GridBoard board, Vector3 worldPos, Quaternion worldRot)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        // Convertir la position du bloc en coordonnées grille
        Vector2Int origin = board.WorldToGrid(worldPos);

        foreach (var cell in localCells)
        {
            // Rotation Y en 90° (même si toi tu ne tournes plus)
            Vector2Int rotated = RotateCell(cell, worldRot);

            // Cellule finale
            Vector2Int final = origin + rotated;

            result.Add(final);
        }

        return result;
    }

    private Vector2Int RotateCell(Vector2Int cell, Quaternion rot)
    {
        // On récupère l'angle Y
        float y = rot.eulerAngles.y;
        int r = Mathf.RoundToInt(y / 90f) % 4;

        switch (r)
        {
            case 0: return new Vector2Int(cell.x, cell.y);
            case 1: return new Vector2Int(cell.y, -cell.x);
            case 2: return new Vector2Int(-cell.x, -cell.y);
            case 3: return new Vector2Int(-cell.y, cell.x);
        }

        return cell;
    }
}
