using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid : MonoBehaviour
{
	public LayerMask obstacleLayer;
	public Vector2 gridWorldSize;
	public float nodeRadius;

	Node[,] grid;
	int gridSizeX, gridSizeY;
	float nodeDiameter;

	void Awake()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}

	void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 bottomLeft = transform.position
			- Vector3.right * gridWorldSize.x / 2
			- Vector3.up * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = bottomLeft
					+ Vector3.right * (x * nodeDiameter + nodeRadius)
					+ Vector3.up * (y * nodeDiameter + nodeRadius);

				bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, obstacleLayer));
				grid[x, y] = new Node(walkable, worldPoint, x, y);
			}
		}
	}

	public Node NodeFromWorldPoint(Vector3 worldPos)
	{
		float percentX = Mathf.Clamp01((worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
		float percentY = Mathf.Clamp01((worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid[x, y];
	}

	public List<Node> GetNeighbors(Node node)
	{
		List<Node> neighbors = new List<Node>(8);

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0) continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX < 0 || checkX >= gridSizeX || checkY < 0 || checkY >= gridSizeY)
					continue;

				// 대각선 이동 시 옆 두 노드 중 하나라도 막혀 있으면 코너 커팅 차단
				if (x != 0 && y != 0)
				{
					if (!grid[node.gridX + x, node.gridY].walkable ||
						!grid[node.gridX, node.gridY + y].walkable)
						continue;
				}

				neighbors.Add(grid[checkX, checkY]);
			}
		}
		return neighbors;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
		if (grid == null) return;

		foreach (Node n in grid)
		{
			if (n.walkable)
				Gizmos.color = Color.green;
			else
				Gizmos.color = Color.red;

			Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.05f));
		}
	}
}