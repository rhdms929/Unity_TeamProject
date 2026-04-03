using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	public bool walkable;    // 갈 수 있는 땅인지
	public Vector3 worldPos; // 실제 게임 화면상의 위치
	public int gridX, gridY; // 바둑판상의 좌표
	public int gCost; // 시작점에서 여기까지 거리
	public int hCost; // 여기서 목적지까지 거리
	public Node parent; // 경로를 기억하기 위한 부모
	public int fCost => gCost + hCost; // 총 점수

	public Node(bool _walkable, Vector3 _worldPos, int x, int y)
	{
		walkable = _walkable;
		worldPos = _worldPos;
		gridX = x;
		gridY = y;
	}
}
