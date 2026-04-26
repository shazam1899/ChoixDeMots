using UnityEngine;

public class GridCompletionChecker : MonoBehaviour
{
    private GridBoard board;

    private void Awake()
    {
        board = GetComponent<GridBoard>();
    }

    private void Update()
    {
        if (IsGridFull())
        {
            Debug.Log("Grille complete");
            // GameManager.Instance.OnGridCompleted();
        }
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
