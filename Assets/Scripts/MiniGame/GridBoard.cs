//Code réaliser par Dylan LAUNAY, avec l'aide de Copilot pour comprendre la logique et debugger
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GridBoard : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;

    private bool[,] occupied;

    private void Awake()
    {
        occupied = new bool[width, height];
    }


    private void SelectExitedEventArgs(SelectExitEventArgs args) //tente de placer le bloc sur la grille lorsque le joueur le lâche après l'avoir saisi, en vérifiant les cellules occupées et en ajustant la position et la rotation du bloc
    {
        var placement = args.interactableObject.transform.GetComponent<BlockPlacement>();
        if (placement != null)
        {
            placement.TryPlace();
        }
    }

    private void OnTriggerEnter(Collider other) //verifie si un bloc est lâché dans la zone du board pour tenter de le placer sur la grille
    {
        var grab = other.GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectExited.AddListener(SelectExitedEventArgs);
        }
    }

    private void OnTriggerExit(Collider other) //verifie si un bloc quitte la zone du board pour arrêter de tenter de le placer sur la grille
    {
        var grab = other.GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectExited.RemoveListener(SelectExitedEventArgs);
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPos) // Convertit une position dans le monde en coordonnées de grille en calculant la position locale par rapport au centre du board, en ajustant pour que (0,0) soit au coin inférieur gauche, et en divisant par la taille des cellules pour obtenir les indices de grille correspondants
    {
        Vector3 local = worldPos - transform.position;

        local.x += (width * cellSize) / 2f;
        local.z += (height * cellSize) / 2f;

        int x = Mathf.FloorToInt(local.x / cellSize);
        int y = Mathf.FloorToInt(local.z / cellSize);

        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(int x, int y) // Convertit des coordonnées de grille en une position dans le monde en calculant la position locale du centre de la cellule correspondante par rapport au coin inférieur gauche du board, et en ajoutant la position du board pour obtenir la position finale dans le monde
    {
        float worldX = (x * cellSize) - (width * cellSize) / 2f + cellSize / 2f;
        float worldZ = (y * cellSize) - (height * cellSize) / 2f + cellSize / 2f;

        return transform.position + new Vector3(worldX, 0, worldZ);
    }

    public bool IsInside(int x, int y) // Vérifie si les coordonnées de grille données sont à l'intérieur des limites du board en vérifiant que les indices sont compris entre 0 et la largeur/hauteur du board
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public bool IsFree(int x, int y)
    {
        return !occupied[x, y];
    }

    public void Occupy(int x, int y) // Marque les coordonnées de grille données comme occupées sur le board en définissant la valeur correspondante dans le tableau "occupied" à true
    {
        occupied[x, y] = true;
    }

    public void Free(int x, int y) // Marque les coordonnées de grille données comme libres sur le board en définissant la valeur correspondante dans le tableau "occupied" à false
    {
        occupied[x, y] = false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        InitializeBoard();
    }
#endif

    public void InitializeBoard() // Initialise le tableau "occupied" en fonction de la largeur, hauteur et taille des cellules du board, en créant un nouveau tableau de booléens avec les dimensions appropriées pour suivre l'état d'occupation de chaque cellule de la grille
    {
        if (width <= 0 || height <= 0 || cellSize <= 0)
            return;

        occupied = new bool[width, height]; // <<< CORRECTION CRITIQUE
    }
}
