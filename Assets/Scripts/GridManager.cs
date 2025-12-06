using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 20;
    public int height = 20;
    public float cellSize = 1f;

    public LayerMask obstacleLayer;

    private GridNode[,] grid;

    public static GridManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        CreateGrid();
    }

    private void CreateGrid()
    {
        Debug.Log("Generating grid: " + width + "x" + height);
        grid = new GridNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                Vector3 worldPos = GridToWorld(gridPos);

                bool walkable = !Physics.CheckSphere(worldPos, cellSize *0.4f, obstacleLayer);

                grid[x, y] = new GridNode(walkable, gridPos, worldPos);
            }
        }
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * cellSize, 0.5f, gridPos.y * cellSize);
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Debug.Log("WorldToGrid called " + worldPos);
        int x = Mathf.FloorToInt(worldPos.x / cellSize);
        int y = Mathf.FloorToInt(worldPos.z / cellSize);
        Debug.Log("After to grid: " + x + ", " + y );
        return new Vector2Int(x, y);
    }

    public GridNode GetNode(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height) return null;

        return grid[pos.x, pos.y];
    }

    public List<GridNode> GetNeighbours(GridNode node)
    {
        List<GridNode> neighbours = new List<GridNode>();

        Vector2Int[] offsets =
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
        };

        foreach (Vector2Int offset in offsets)
        {
            Vector2Int neighboursPos = node.GridPos + offset;
            GridNode neighbour = GetNode(neighboursPos);

            if (neighbour != null && neighbour.Walkable)
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    // private void OnDrawGizmos()
    // {
    //     if (grid == null) return;

    //     foreach (GridNode node in grid)
    //     {
    //         Gizmos.color = node.Walkable ? Color.white : Color.red;
    //         Gizmos.DrawCube(node.WorldPos, Vector3.one * (cellSize * 0.9f));
    //     }
    // }
}