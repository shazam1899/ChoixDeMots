//Code réaliser par Dylan LAUNAY, avec l'aide de Copilot pour comprendre la logique et debugger
using System.Collections.Generic;
using UnityEngine;

public class BlockShape : MonoBehaviour
{
    public Vector2Int[] localCells;

    // Méthode pour obtenir les cellules de la grille occupées par le bloc à une position et rotation données, en appliquant la rotation aux cellules locales du bloc et en les convertissant en coordonnées de grille basées sur la position du bloc dans le monde
    public List<Vector2Int> GetWorldCells(GridBoard board, Vector3 worldPos, Quaternion worldRot)
    {
        if (localCells == null || localCells.Length == 0)
        {
            return null;
        }

        List<Vector2Int> result = new List<Vector2Int>();

        Vector2Int origin = board.WorldToGrid(worldPos);

        foreach (var cell in localCells)
        {
            Vector2Int rotated = RotateCell(cell, worldRot);
            Vector2Int final = origin + rotated;
            result.Add(final);
        }

        return result;
    }

    private Vector2Int RotateCell(Vector2Int cell, Quaternion rot)
    {
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

#if UNITY_EDITOR
    public void EditorInitialize() //initialiser les cellules locales du bloc pour l'éditeur
    {
        if (localCells == null || localCells.Length == 0)
        {
            Debug.LogWarning("BlockShape EditorInitialize : localCells vide sur " + gameObject.name);
        }
    }
#endif
}
