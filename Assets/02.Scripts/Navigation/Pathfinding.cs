
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

		NodeHeap openSet = new NodeHeap();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node currentNode = openSet.RemoveMin();
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
					else
						openSet.UpdateItem(neighbor);
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

	class NodeHeap
	{
		List<Node> items = new List<Node>();
		Dictionary<Node, int> indexMap = new Dictionary<Node, int>();

		public int Count => items.Count;
		public bool Contains(Node node) => indexMap.ContainsKey(node);

		public void Add(Node node)
		{
			items.Add(node);
			int i = items.Count - 1;
			indexMap[node] = i;
			BubbleUp(i);
		}

		public Node RemoveMin()
		{
			Node min = items[0];
			int last = items.Count - 1;
			Swap(0, last);
			items.RemoveAt(last);
			indexMap.Remove(min);
			if (items.Count > 0)
				SiftDown(0);
			return min;
		}

		// 비용이 낮아졌을 때만 호출되므로 BubbleUp만으로 충분
		public void UpdateItem(Node node) => BubbleUp(indexMap[node]);

		void BubbleUp(int i)
		{
			while (i > 0)
			{
				int parent = (i - 1) / 2;
				if (Compare(items[i], items[parent]) < 0)
				{
					Swap(i, parent);
					i = parent;
				}
				else break;
			}
		}

		void SiftDown(int i)
		{
			int count = items.Count;
			while (true)
			{
				int left = 2 * i + 1, right = 2 * i + 2, smallest = i;

				if (left < count && Compare(items[left], items[smallest]) < 0)
					smallest = left;
				if (right < count && Compare(items[right], items[smallest]) < 0)
					smallest = right;

				if (smallest == i) break;
				Swap(i, smallest);
				i = smallest;
			}
		}

		void Swap(int i, int j)
		{
			Node tmp = items[i];
			items[i] = items[j];
			items[j] = tmp;
			indexMap[items[i]] = i;
			indexMap[items[j]] = j;
		}

		int Compare(Node a, Node b)
		{
			int cmp = a.fCost.CompareTo(b.fCost);
			return cmp != 0 ? cmp : a.hCost.CompareTo(b.hCost);
		}
	}
}
