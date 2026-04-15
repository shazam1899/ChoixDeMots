using UnityEngine;

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

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 local = worldPos - transform.position;
        int x = Mathf.RoundToInt(local.x / cellSize);
        int y = Mathf.RoundToInt(local.z / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(int x, int y)
    {
        return transform.position + new Vector3(x * cellSize, 0, y * cellSize);
    }

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
}
