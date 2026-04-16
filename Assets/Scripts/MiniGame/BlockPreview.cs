using UnityEngine;

public class BlockPreview : MonoBehaviour
{
    private GameObject preview;
    private GridBoard board;
    private BlockShape shape;
    private BlockPlacement placement;

    private void Awake()
    {
        board = FindFirstObjectByType<GridBoard>();
        shape = GetComponent<BlockShape>();
        placement = GetComponent<BlockPlacement>();
    }

    public void CreatePreview()
    {
        if (preview != null) return;

        // Clone du mesh automatiquement
        preview = Instantiate(gameObject, transform.position, transform.rotation);
        DestroyImmediate(preview.GetComponent<BlockPlacement>());
        DestroyImmediate(preview.GetComponent<BlockShape>());
        DestroyImmediate(preview.GetComponent<BlockPreview>());
        DestroyImmediate(preview.GetComponent<Rigidbody>());
        DestroyImmediate(preview.GetComponent<Collider>());

        // Rendre transparent
        foreach (var r in preview.GetComponentsInChildren<Renderer>())
        {
            r.material = new Material(r.material);
            r.material.color = new Color(0, 1, 0, 0.3f);
        }
    }

    public void DestroyPreview()
    {
        if (preview != null)
            Destroy(preview);
    }

    private void Update()
    {
        if (preview == null) return;

        // Snap rotation
        float yRot = transform.eulerAngles.y;
        float snappedRot = Mathf.Round(yRot / 90f) * 90f;
        Quaternion finalRot = Quaternion.Euler(-90f, snappedRot, 0f);

        // Snap position
        Vector2Int gridPos = board.WorldToGrid(transform.position);
        Vector3 finalPos = board.GridToWorld(gridPos.x, gridPos.y);

        preview.transform.position = finalPos;
        preview.transform.rotation = finalRot;

        // Validité
        bool valid = placement.CanPlace(finalPos, finalRot);

        Color c = valid ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);

        foreach (var r in preview.GetComponentsInChildren<Renderer>())
            r.material.color = c;
    }
}
