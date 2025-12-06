using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    public float moveSpeed = 3f;

    private AStarPathfinder pathfinder;
    private Coroutine moveRoutine;

    private void Awake()
    {
        pathfinder = GetComponent<AStarPathfinder>();
    }

    private void Update()
    {
        Debug.Log("Update triggered");
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                MoveTo(hit.point);
            }
        }
    }

    public void MoveTo(Vector3 target)
    {
        Debug.Log("MoveTo called! Target = " + target);
        Debug.Log("Current Transform: " + transform.position);
        Debug.Log("Target Position is " + target);
        List<GridNode> path = pathfinder.FindPath(transform.position, target);
        var pathVisualizer = GetComponent<PathVisualizer>();

        if (pathVisualizer != null)
            pathVisualizer.DrawPath(path);

        if (path == null) {
            Debug.LogError("A* reutrned NULL path!");
            return;
        }

        Debug.Log("A* returned " + path.Count + " nodes.");
        if (moveRoutine != null)
        StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(FollowPath(path));
    }

    private IEnumerator FollowPath(List<GridNode> path)
    {
        foreach (GridNode node in path)
        {
            while (Vector3.Distance(transform.position, node.WorldPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    node.WorldPos,
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }
        }
    }
}