using UnityEngine;

public class BlockPreview : MonoBehaviour
{
    public Material previewMaterial; // assigné dans l’inspector

    private GameObject preview;
    private GridBoard board;
    private BlockShape shape;
    private BlockPlacement placement;

    private Quaternion fixedRotation;

    private void Awake()
    {
        board = FindFirstObjectByType<GridBoard>();
        shape = GetComponent<BlockShape>();
        placement = GetComponent<BlockPlacement>();

        fixedRotation = transform.rotation;
    }

    public void CreatePreview()
    {
        if (preview != null) return;

        preview = Instantiate(gameObject);
        preview.transform.localScale = transform.localScale;

        DestroyImmediate(preview.GetComponent<BlockPlacement>());
        DestroyImmediate(preview.GetComponent<BlockShape>());
        DestroyImmediate(preview.GetComponent<BlockPreview>());
        DestroyImmediate(preview.GetComponent<Rigidbody>());
        DestroyImmediate(preview.GetComponent<Collider>());

        foreach (var r in preview.GetComponentsInChildren<Renderer>())
        {
            r.material = new Material(previewMaterial);
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

        Quaternion finalRot = fixedRotation;

        Vector2Int gridPos = board.WorldToGrid(placement.GetPlacementOrigin());
        Vector3 finalPos = board.GridToWorld(gridPos.x, gridPos.y);

        preview.transform.position = finalPos;
        preview.transform.rotation = finalRot;

        bool valid = placement.CanPlace(finalPos, finalRot);

        Color c = valid ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);

        foreach (var r in preview.GetComponentsInChildren<Renderer>())
            r.material.color = c;
    }
}
