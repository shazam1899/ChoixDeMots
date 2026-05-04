//Code réaliser par Dylan LAUNAY, avec l'aide de Copilot pour comprendre la logique et debugger
using UnityEngine;

public class BlockPreview : MonoBehaviour
{
    public Material previewMaterial;

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

        preview = Instantiate(gameObject); // Crée une copie du bloc actuel pour servir de prévisualisation
        preview.transform.localScale = transform.localScale; // Assure que la prévisualisation a la même échelle que le bloc original

        // Supprime les composants qui ne sont pas nécessaires pour la prévisualisation et applique le matériau de prévisualisation pour différencier visuellement la prévisualisation du bloc original
        DestroyImmediate(preview.GetComponent<BlockPlacement>());
        DestroyImmediate(preview.GetComponent<BlockShape>());
        DestroyImmediate(preview.GetComponent<BlockPreview>());
        DestroyImmediate(preview.GetComponent<Rigidbody>());
        DestroyImmediate(preview.GetComponent<Collider>());

        // Applique le matériau de prévisualisation à tous les renderers de la prévisualisation pour lui donner une apparence distincte
        foreach (var r in preview.GetComponentsInChildren<Renderer>())
        {
            r.material = new Material(previewMaterial);
        }
    }

    public void DestroyPreview() //detruit la prévisualisation lorsque le bloc est lâché pour éviter d'avoir des prévisualisations qui traînent dans la scène
    {
        if (preview != null)
            Destroy(preview);
    }

    private void Update()
    {
        if (preview == null) return;

        Quaternion finalRot = fixedRotation; //rotation fixe pour la prévisualisation

        Vector2Int gridPos = board.WorldToGrid(placement.GetPlacementOrigin()); 
        Vector3 finalPos = board.GridToWorld(gridPos.x, gridPos.y);

        preview.transform.position = finalPos; 
        preview.transform.rotation = finalRot;

        // Vérifie que le placement à la position et rotation actuelles est valide pour ajuster la couleur de la prévisualisation en conséquence (vert pour valide, rouge pour invalide)
        bool valid = placement.CanPlace(finalPos, finalRot);

        Color c = valid ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);

        foreach (var r in preview.GetComponentsInChildren<Renderer>())
            r.material.color = c;
    }
}
