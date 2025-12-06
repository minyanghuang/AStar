using UnityEngine;

[System.Serializable]
public class GridNode
{
    public bool Walkable;
    public Vector2Int GridPos;
    public Vector3 WorldPos;
    
    public int GCost;
    public int HCost;
    public int FCost => GCost + HCost;

    public GridNode Parent;

    public GridNode(bool walkable, Vector2Int gridPos, Vector3 worldPos)
    {
        Walkable = walkable;
        GridPos = gridPos;
        WorldPos = worldPos;
    }
}
