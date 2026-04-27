using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BlockPlacement : MonoBehaviour
{
    private GridBoard board;
    private BlockShape shape;
    private Rigidbody rb;
    private XRGrabInteractable grab;

    private List<Vector2Int> lastCells = new List<Vector2Int>();

    private Quaternion fixedRotation;

    private void Awake()
    {
        board = FindFirstObjectByType<GridBoard>();
        shape = GetComponent<BlockShape>();
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();

        // Rotation du prefab (ex: -90° X)
        fixedRotation = transform.rotation;

        // 🔥 Verrouille la rotation dans le monde
        rb.freezeRotation = true;

        grab.selectEntered.AddListener((args) => FreeCells());

        var preview = GetComponent<BlockPreview>();
        grab.selectEntered.AddListener((args) => preview.CreatePreview());
        grab.selectExited.AddListener((args) => preview.DestroyPreview());
    }

    // Origine du snap = centre du collider
    public Vector3 GetPlacementOrigin()
    {
        return GetComponent<Collider>().bounds.center;
    }

    public void TryPlace()
    {
        Quaternion finalRot = fixedRotation;

        // Snap position depuis le centre
        Vector2Int gridPos = board.WorldToGrid(GetPlacementOrigin());
        Vector3 finalPos = board.GridToWorld(gridPos.x, gridPos.y);

        // Vérifier les cases
        List<Vector2Int> cells = shape.GetWorldCells(board, finalPos, finalRot);

        foreach (var c in cells)
        {
            if (!board.IsInside(c.x, c.y)) return;
            if (!board.IsFree(c.x, c.y)) return;
        }

        // Libérer anciennes cases
        FreeCells();

        // Occuper nouvelles cases
        lastCells = cells;
        foreach (var c in cells)
            board.Occupy(c.x, c.y);

        // Appliquer position + rotation
        transform.position = finalPos;
        transform.rotation = finalRot;

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void FreeCells()
    {
        if (lastCells == null || lastCells.Count == 0)
            return;

        foreach (var c in lastCells)
            board.Free(c.x, c.y);

        lastCells.Clear();

        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public bool CanPlace(Vector3 pos, Quaternion rot)
    {
        List<Vector2Int> cells = shape.GetWorldCells(board, pos, rot);

        foreach (var c in cells)
        {
            if (!board.IsInside(c.x, c.y)) return false;
            if (!board.IsFree(c.x, c.y)) return false;
        }

        return true;
    }
}
