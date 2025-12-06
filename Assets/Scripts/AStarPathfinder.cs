using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{
    private GridManager grid;

    private void Awake()
    {
        grid = GridManager.Instance;
        Debug.Log("GridManager instance = " + grid);
    }

    public List<GridNode> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Debug.Log("Start pos = " + startPos);
        Debug.Log("Grid start = " + grid.WorldToGrid(startPos));
        GridNode startNode = grid.GetNode(grid.WorldToGrid(startPos));
        GridNode targetNode = grid.GetNode(grid.WorldToGrid(targetPos));

        if (startNode == null || targetNode == null) return null;
        if (!targetNode.Walkable) return null;

        List<GridNode> openSet = new List<GridNode>();
        HashSet<GridNode> closedSet = new HashSet<GridNode>();

        startNode.GCost = 0;
        startNode.HCost = Distance(startNode, targetNode);

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GridNode currentNode = GetLowestFCost(openSet);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (GridNode neighbour in grid.GetNeighbours(currentNode))
            {
                if (closedSet.Contains(neighbour)) continue;

                int tentativeG = currentNode.GCost + Distance(currentNode, neighbour);

                if (!openSet.Contains(neighbour) || tentativeG < neighbour.GCost)
                {
                    neighbour.GCost = tentativeG;
                    neighbour.HCost = Distance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }

    private GridNode GetLowestFCost(List<GridNode> nodes)
    {
        GridNode bestNode = nodes[0];

        foreach (GridNode node in nodes)
        {
            if (node.FCost < bestNode.FCost || (node.FCost == bestNode.FCost && node.HCost < bestNode.HCost))
            {
                bestNode = node;
            }
        }
        
        return bestNode;
    }

    private List<GridNode> RetracePath(GridNode start, GridNode end)
    {
        List<GridNode> path = new List<GridNode>();
        GridNode current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }

    private int Distance(GridNode a, GridNode b)
    {
        return Mathf.Abs(a.GridPos.x - b.GridPos.x) + Mathf.Abs(a.GridPos.y - b.GridPos.y);
    }
}