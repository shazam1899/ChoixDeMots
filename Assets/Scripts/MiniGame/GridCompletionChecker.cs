using UnityEngine;

public class GridCompletionChecker : MonoBehaviour
{
    private GridBoard board;
    private bool wasFull = false;

    public FixedBlockInitializer initializer;

    private void Awake()
    {
        board = GetComponent<GridBoard>();
    }

    private void Update()
    {
        bool isFull = IsGridFull();

        if (!wasFull && isFull)
        {
            Debug.Log("🔥 Grille complète !");
            initializer?.CompleteMiniGame();
        }

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
