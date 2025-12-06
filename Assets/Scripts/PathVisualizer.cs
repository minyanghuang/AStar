using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public void DrawPath(List<GridNode> path)
    {
        if (path == null || path.Count == 0)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, path[i].WorldPos + Vector3.up * 0.1f);
        }
    }
}
