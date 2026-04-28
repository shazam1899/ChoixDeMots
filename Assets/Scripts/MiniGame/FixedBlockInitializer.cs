using UnityEngine;
using System.Collections.Generic;

public class FixedBlockInitializer : MonoBehaviour
{
    [System.Serializable]
    public class FixedBlockEntry
    {
        public GameObject prefab;
        public Vector2Int gridPosition;
        public int rotation;
    }

    [System.Serializable]
    public class FixedBlockConfig
    {
        public string name;
        public List<FixedBlockEntry> entries = new List<FixedBlockEntry>();
    }

    [Header("Configurations de niveaux")]
    public List<FixedBlockConfig> configurations = new List<FixedBlockConfig>();

    [Header("Blocs disponibles pour le joueur")]
    public List<GameObject> allBlockPrefabs = new List<GameObject>();

    [Header("Zone de spawn (cube)")]
    public Transform spawnAreaCenter;
    public Vector3 spawnAreaSize = new Vector3(4, 2, 2);
    public float spawnGridSpacing = 1.2f;

    private GridBoard board;
    private Transform spawnedRoot;

    private void EnsureBoard()
    {
        if (board == null)
        {
            board = FindFirstObjectByType<GridBoard>();

            if (board != null)
                board.InitializeBoard();
            else
                Debug.LogError("❌ Aucun GridBoard trouvé !");
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

        if (configIndex < 0 || configIndex >= configurations.Count)
        {
            Debug.LogError("Index de configuration invalide");
            return;
        }

        var config = configurations[configIndex];

        foreach (var entry in config.entries)
        {
            bool ok = SpawnAndPlace(entry);

            if (!ok)
                Debug.LogWarning("⚠ Bloc ignoré : " + (entry.prefab != null ? entry.prefab.name : "NULL"));
        }

        SpawnRemainingBlocks(config);
    }

    private bool SpawnAndPlace(FixedBlockEntry entry)
    {
        EnsureBoard();
        if (board == null) return false;

        if (entry.prefab == null)
        {
            Debug.LogError("❌ Prefab manquant !");
            return false;
        }

        Vector3 worldPos = board.GridToWorld(entry.gridPosition.x, entry.gridPosition.y);
        Quaternion rot = Quaternion.Euler(-90f, entry.rotation, 0f);

        GameObject block = Instantiate(entry.prefab, worldPos, rot, spawnedRoot);

        BlockShape shape = block.GetComponent<BlockShape>();
        if (shape == null)
        {
            Debug.LogError("❌ Le prefab " + entry.prefab.name + " n'a PAS de BlockShape !");
            return false;
        }

        shape.EditorInitialize();

        if (shape.localCells == null || shape.localCells.Length == 0)
        {
            Debug.LogError("❌ localCells VIDE sur " + entry.prefab.name);
            return false;
        }

        var cells = shape.GetWorldCells(board, worldPos, rot);
        if (cells == null)
        {
            Debug.LogError("❌ GetWorldCells NULL pour " + entry.prefab.name);
            return false;
        }

        foreach (var c in cells)
        {
            if (!board.IsInside(c.x, c.y))
            {
                Debug.LogError("❌ Cellule hors grille : " + c + " pour " + entry.prefab.name);
                return false;
            }

            board.Occupy(c.x, c.y);
        }

        var rb = block.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        var col = block.GetComponent<Collider>();
        if (col) col.enabled = false;

        return true;
    }

    private void SpawnRemainingBlocks(FixedBlockConfig config)
    {
        if (spawnAreaCenter == null)
        {
            Debug.LogError("❌ Aucun spawnAreaCenter assigné !");
            return;
        }

        HashSet<GameObject> used = new HashSet<GameObject>();
        foreach (var entry in config.entries)
            used.Add(entry.prefab);

        int countX = Mathf.FloorToInt(spawnAreaSize.x / spawnGridSpacing);
        int countY = Mathf.FloorToInt(spawnAreaSize.y / spawnGridSpacing);
        int countZ = Mathf.FloorToInt(spawnAreaSize.z / spawnGridSpacing);

        if (countX < 1 || countY < 1 || countZ < 1)
        {
            Debug.LogWarning("⚠ Zone de spawn trop petite pour au moins un slot.");
            return;
        }

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
            if (prefab == null) continue;
            if (used.Contains(prefab))
                continue;

            int x = index % countX;
            int y = (index / countX) % countY;
            int z = index / (countX * countY);

            if (z >= countZ)
            {
                Debug.LogWarning("⚠ Pas assez de place dans la zone de spawn !");
                break;
            }

            Vector3 pos = origin + new Vector3(
                x * spawnGridSpacing,
                y * spawnGridSpacing,
                z * spawnGridSpacing
            );

            Quaternion baseRot = prefab.transform.rotation;
            Quaternion finalRot = Quaternion.Euler(
                -90f,
                baseRot.eulerAngles.y,
                baseRot.eulerAngles.z
            );

            Instantiate(prefab, pos, finalRot, spawnedRoot);

            index++;
        }
    }

    public void ClearBoard()
    {
        EnsureBoard();
        if (board == null) return;

        board.InitializeBoard();

        if (spawnedRoot != null)
        {
            List<GameObject> toDestroy = new List<GameObject>();

            foreach (Transform child in spawnedRoot)
                toDestroy.Add(child.gameObject);

            foreach (var obj in toDestroy)
                DestroyImmediate(obj);
        }

        Debug.Log("✔ Plateau vidé + blocs spawn supprimés");
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnAreaCenter == null) return;

        Gizmos.color = new Color(0f, 1f, 1f, 0.25f);
        Gizmos.DrawCube(spawnAreaCenter.position, spawnAreaSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(spawnAreaCenter.position, spawnAreaSize);
    }
}
