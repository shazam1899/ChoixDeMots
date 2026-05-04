//Code réaliser par Dylan LAUNAY, avec l'aide de Copilot pour comprendre la logique et debugger
using UnityEngine;
using System.Collections.Generic;

public class FixedBlockInitializer : MonoBehaviour
{
    public System.Action OnMiniGameCompleted;

    [System.Serializable]
    public class FixedBlockEntry //config pour chaque bloc à placer : prefab, position sur la grille et rotation en degrés (0,90,180,270)
    {
        public GameObject prefab;
        public Vector2Int gridPosition;
        public int rotation;
    }

    [System.Serializable]
    public class FixedBlockConfig //config pour chaque niveau : nom du niveau et liste des blocs à placer
    {
        public string name;
        public List<FixedBlockEntry> entries = new List<FixedBlockEntry>();
    }

    public List<FixedBlockConfig> configurations = new List<FixedBlockConfig>(); //configurations pour chaque niveau, à configurer dans l'inspecteur
    public List<GameObject> allBlockPrefabs = new List<GameObject>(); // Liste de tous les prefabs de blocs, à configurer dans l'inspecteur pour pouvoir les faire spawn dans la zone de spawn

    public Transform spawnAreaCenter; // Centre de la zone de spawn pour les blocs restants, à configurer dans l'inspecteur
    public Vector3 spawnAreaSize = new Vector3(4, 2, 2); // Taille de la zone de spawn pour les blocs restants, à configurer dans l'inspecteur (X = largeur, Y = hauteur, Z = profondeur)
    public float spawnGridSpacing = 1.2f;

    private GridBoard board;
    private Transform spawnedRoot;

    private void EnsureBoard() //references au board 
    {
        if (board == null)
        {
            board = FindFirstObjectByType<GridBoard>();
            if (board != null)
                board.InitializeBoard();
        }

        if (spawnedRoot == null)
        {
            GameObject root = GameObject.Find("SpawnedBlocksRoot");
            if (root == null)
                root = new GameObject("SpawnedBlocksRoot");

            spawnedRoot = root.transform;
        }
    }

    public void Initialize(int configIndex)
    {
        EnsureBoard();
        if (board == null) return;

        var config = configurations[configIndex];

        foreach (var entry in config.entries)
            SpawnAndPlace(entry);

        SpawnRemainingBlocks(config);
    }

    private bool SpawnAndPlace(FixedBlockEntry entry) // Instancie un bloc à partir de son prefab, le placer à la position et rotation spécifiées sur la grille, et occuper les cellules correspondantes sur le board
    {
        EnsureBoard();

        Vector3 worldPos = board.GridToWorld(entry.gridPosition.x, entry.gridPosition.y);
        Quaternion rot = Quaternion.Euler(-90f, entry.rotation, 0f);

        GameObject block = Instantiate(entry.prefab, worldPos, rot, spawnedRoot);

        BlockShape shape = block.GetComponent<BlockShape>();

//editeur : initialiser les cellules locales du bloc pour pouvoir les occuper sur le board
#if UNITY_EDITOR
        shape.EditorInitialize();
#endif

        var cells = shape.GetWorldCells(board, worldPos, rot);
        foreach (var c in cells)
            board.Occupy(c.x, c.y);

        foreach (var comp in block.GetComponentsInChildren<Component>(true))
        {
            if (comp.GetType().Name.Contains("XRGrabInteractable"))
                Destroy(comp);
        }

        var rb = block.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        foreach (var col in block.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
            col.isTrigger = false;
        }

        return true;
    }

    private void SpawnRemainingBlocks(FixedBlockConfig config) // Instancie les blocs qui ne sont pas dans la configuration du niveau et les place dans la zone de spawn
    {
        HashSet<GameObject> used = new HashSet<GameObject>();
        foreach (var e in config.entries)
            used.Add(e.prefab);

        int countX = Mathf.FloorToInt(spawnAreaSize.x / spawnGridSpacing);
        int countY = Mathf.FloorToInt(spawnAreaSize.y / spawnGridSpacing);
        int countZ = Mathf.FloorToInt(spawnAreaSize.z / spawnGridSpacing);

        Vector3 origin =
            spawnAreaCenter.position -
            new Vector3(
                (countX - 1) * spawnGridSpacing,
                (countY - 1) * spawnGridSpacing,
                (countZ - 1) * spawnGridSpacing
            ) * 0.5f;

        int index = 0;

        foreach (var prefab in allBlockPrefabs)
        {
            if (used.Contains(prefab)) continue;

            int x = index % countX;
            int y = (index / countX) % countY;
            int z = index / (countX * countY);

            Vector3 pos = origin + new Vector3(
                x * spawnGridSpacing,
                y * spawnGridSpacing,
                z * spawnGridSpacing
            );

            Quaternion finalRot = Quaternion.Euler(-90f, prefab.transform.eulerAngles.y, prefab.transform.eulerAngles.z);

            GameObject obj = Instantiate(prefab, pos, finalRot, spawnedRoot);

            var rb = obj.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            foreach (var col in obj.GetComponentsInChildren<Collider>())
            {
                col.enabled = true;
                col.isTrigger = false;
            }

            index++;
        }
    }

    public void ClearBoard() // Libère toutes les cellules du board et détruit tous les blocs instanciés dans la zone de spawn pour réinitialiser le niveau
    {
        EnsureBoard();
        board.InitializeBoard();

        List<GameObject> toDestroy = new List<GameObject>();
        foreach (Transform child in spawnedRoot)
            toDestroy.Add(child.gameObject);

        foreach (var obj in toDestroy)
            Destroy(obj);
    }

    public void CompleteMiniGame() // Appelé lorsque le mini-jeu est complété pour déclencher l'événement de complétion du mini-jeu
    {
        Debug.Log("Mini-jeu complété !");
        OnMiniGameCompleted?.Invoke();
    }
}
