//Code réaliser par Dylan LAUNAY, avec l'aide de Copilot pour comprendre la logique et debugger
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

    private void Update() // Vérifie à chaque frame si la grille est complète en appelant la méthode IsGridFull
    {
        bool isFull = IsGridFull();

        if (!wasFull && isFull) // Si la grille est complète et qu'elle ne l'était pas lors de la dernière vérification, déclenche l'événement de complétion du mini-jeu
        {
            Debug.Log("Grille complète !");
            initializer?.CompleteMiniGame();
        }

        wasFull = isFull; // Met à jour le statut de complétion pour la prochaine vérification afin d'éviter de déclencher l'événement plusieurs fois si la grille reste complète pendant plusieurs frames
    }

    private bool IsGridFull() // Vérifie si la grille est complète en parcourant toutes les cellules du board et en vérifiant si elles sont toutes occupées, retourne false dès qu'une cellule libre est trouvée, sinon retourne true si toutes les cellules sont occupées
    {
        for (int x = 0; x < board.width; x++) 
        {
            for (int y = 0; y < board.height; y++) // Parcourt chaque cellule de la grille en utilisant les dimensions du board
            {
                if (board.IsFree(x, y))
                    return false;
            }
        }
        return true;
    }
}
