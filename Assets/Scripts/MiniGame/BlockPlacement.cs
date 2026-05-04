//Code réaliser par Dylan LAUNAY, avec l'aide de Copilot pour comprendre la logique et debugger
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BlockPlacement : MonoBehaviour
{
    private GridBoard board;
    private BlockShape shape;
    private Rigidbody rb;
    private XRGrabInteractable grab;

    private List<Vector2Int> lastCells = new List<Vector2Int>(); // Liste des cellules occupées par le bloc lors de sa dernière position valide

    private Quaternion fixedRotation;

    private void Awake() // Initialisation des références aux composants et configuration des événements de grab
    {
        board = FindFirstObjectByType<GridBoard>();
        shape = GetComponent<BlockShape>();
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();

        fixedRotation = transform.rotation;

        rb.freezeRotation = true;

        grab.selectEntered.AddListener((args) => FreeCells());

        var preview = GetComponent<BlockPreview>();
        grab.selectEntered.AddListener((args) => preview.CreatePreview());
        grab.selectExited.AddListener((args) => preview.DestroyPreview());
    }

    public Vector3 GetPlacementOrigin() // Méthode pour obtenir la position centrale du bloc, utilisée comme origine pour le placement sur la grille
    {
        return GetComponent<Collider>().bounds.center;
    }

    public void TryPlace() // Méthode pour tenter de placer le bloc sur la grille en vérifiant les cellules occupées et en ajustant la position et la rotation du bloc
    {
        Quaternion finalRot = fixedRotation;

        // Snap position depuis le centre
        Vector2Int gridPos = board.WorldToGrid(GetPlacementOrigin());
        Vector3 finalPos = board.GridToWorld(gridPos.x, gridPos.y);

        // Vérifier les cases
        List<Vector2Int> cells = shape.GetWorldCells(board, finalPos, finalRot);

        foreach (var c in cells) // Vérifie que chaque cellule occupée par le bloc est à l'intérieur de la grille et libre avant de permettre le placement
        {
            if (!board.IsInside(c.x, c.y)) return;
            if (!board.IsFree(c.x, c.y)) return;
        }
        

        FreeCells();

        // Occupy new cells
        lastCells = cells;
        foreach (var c in cells)
            board.Occupy(c.x, c.y);

        transform.position = finalPos;
        transform.rotation = finalRot;

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void FreeCells() // Méthode pour libérer les cellules précédemment occupées par le bloc, appelée lorsque le bloc est saisi pour permettre de le déplacer à nouveau
    {
        if (lastCells == null || lastCells.Count == 0)
            return;
        
        foreach (var c in lastCells)
            board.Free(c.x, c.y);

        lastCells.Clear();

        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public bool CanPlace(Vector3 pos, Quaternion rot) // Méthode pour vérifier si le bloc peut être placé à une position et une rotation données en vérifiant les cellules occupées
    {
        List<Vector2Int> cells = shape.GetWorldCells(board, pos, rot);

        foreach (var c in cells) // Vérifie que chaque cellule occupée par le bloc à la position et rotation données est à l'intérieur de la grille et libre pour permettre le placement
        {
            if (!board.IsInside(c.x, c.y)) return false;
            if (!board.IsFree(c.x, c.y)) return false;
        }

        return true;
    }
}
