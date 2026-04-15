using System.Collections.Generic;
using UnityEngine;


public class BlockPlacement : MonoBehaviour
{
    private GridBoard board;
    private BlockShape shape;
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    private void Awake()
    {
        board = FindFirstObjectByType<GridBoard>();
        shape = GetComponent<BlockShape>();
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    public void TryPlace()
    {
        // 1. Arrondir rotation
        float yRot = transform.eulerAngles.y;
        float snappedRot = Mathf.Round(yRot / 90f) * 90f;
        Quaternion finalRot = Quaternion.Euler(0, snappedRot, 0);

        // 2. Arrondir position
        Vector2Int gridPos = board.WorldToGrid(transform.position);
        Vector3 finalPos = board.GridToWorld(gridPos.x, gridPos.y);

        // 3. Vérifier les cases
        List<Vector2Int> cells = shape.GetWorldCells(board, finalPos, finalRot);

        foreach (var c in cells)
        {
            if (!board.IsInside(c.x, c.y))
                return;

            if (!board.IsFree(c.x, c.y))
                return;
        }

        // 4. Valider le placement
        foreach (var c in cells)
            board.Occupy(c.x, c.y);

        transform.position = finalPos;
        transform.rotation = finalRot;

        rb.isKinematic = true;
        rb.useGravity = false;

        grab.enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
