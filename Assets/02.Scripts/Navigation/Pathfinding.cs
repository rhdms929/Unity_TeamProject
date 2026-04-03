
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
	PathGrid grid;

	void Awake()
	{
		grid = GetComponent<PathGrid>();
	}

	public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node currentNode = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < currentNode.fCost ||
					openSet[i].fCost == currentNode.fCost &&
					openSet[i].hCost < currentNode.hCost)
					currentNode = openSet[i];
			}

			openSet.Remove(currentNode);
			closedSet.Add(currentNode);

			if (currentNode == targetNode)
				return RetracePath(startNode, targetNode);

			foreach (Node neighbor in grid.GetNeighbors(currentNode))
			{
				if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

				int newCost = currentNode.gCost + GetDistance(currentNode, neighbor);
				if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
				{
					neighbor.gCost = newCost;
					neighbor.hCost = GetDistance(neighbor, targetNode);
					neighbor.parent = currentNode;

					if (!openSet.Contains(neighbor))
						openSet.Add(neighbor);
				}
			}
		}
		return null;
	}
	List<Node> RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node current = endNode;

		while (current != startNode)
		{
			path.Add(current);
			current = current.parent;
		}

		path.Reverse();
		return path;
	}

	int GetDistance(Node a, Node b)
	{
		int dstX = Mathf.Abs(a.gridX - b.gridX);
		int dstY = Mathf.Abs(a.gridY - b.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		else
			return 14 * dstX + 10 * (dstY - dstX);
	}
}
