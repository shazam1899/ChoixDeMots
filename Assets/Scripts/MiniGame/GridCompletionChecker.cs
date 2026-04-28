using UnityEngine;

public class GridCompletionChecker : MonoBehaviour
{
    private GridBoard board;
    private bool wasFull = false; // état précédent

    private void Awake()
    {
        board = GetComponent<GridBoard>();
    }

    private void Update()
    {
        bool isFull = IsGridFull();

        // Transition : NON COMPLET → COMPLET
        if (!wasFull && isFull)
        {
            Debug.Log("Grille complete");
            // GameManager.Instance.OnGridCompleted();
        }

        // Transition : COMPLET → NON COMPLET
        if (wasFull && !isFull)
        {
            Debug.Log("Grille plus complete (reset)");
        }

        // Mise à jour de l'état
        wasFull = isFull;
    }

    private bool IsGridFull()
    {
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                if (board.IsFree(x, y))
                    return false;
            }
        }
        return true;
    }
}
