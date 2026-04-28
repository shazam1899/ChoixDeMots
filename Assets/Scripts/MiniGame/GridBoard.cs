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

    // -----------------------------
    //  TRIGGER POUR LE PLACEMENT
    // -----------------------------
    private void SelectExitedEventArgs(SelectExitEventArgs args)
    {
        var placement = args.interactableObject.transform.GetComponent<BlockPlacement>();
        if (placement != null)
        {
            placement.TryPlace();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var grab = other.GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectExited.AddListener(SelectExitedEventArgs);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var grab = other.GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectExited.RemoveListener(SelectExitedEventArgs);
        }
    }

    // -----------------------------
    //  COORDONNÉES GRILLE <-> MONDE
    // -----------------------------

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 local = worldPos - transform.position;

        local.x += (width * cellSize) / 2f;
        local.z += (height * cellSize) / 2f;

        int x = Mathf.FloorToInt(local.x / cellSize);
        int y = Mathf.FloorToInt(local.z / cellSize);

        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(int x, int y)
    {
        float worldX = (x * cellSize) - (width * cellSize) / 2f + cellSize / 2f;
        float worldZ = (y * cellSize) - (height * cellSize) / 2f + cellSize / 2f;

        return transform.position + new Vector3(worldX, 0, worldZ);
    }

    // -----------------------------
    //  OCCUPATION DES CASES
    // -----------------------------
    public bool IsInside(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public bool IsFree(int x, int y)
    {
        return !occupied[x, y];
    }

    public void Occupy(int x, int y)
    {
        occupied[x, y] = true;
    }

    public void Free(int x, int y)
    {
        occupied[x, y] = false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        InitializeBoard();
    }
#endif

    public void InitializeBoard()
    {
        if (width <= 0 || height <= 0 || cellSize <= 0)
            return;

        occupied = new bool[width, height]; // <<< CORRECTION CRITIQUE
    }
}
