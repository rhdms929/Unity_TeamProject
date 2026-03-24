using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSpawner : MonoBehaviour
{
	[Header("Spawn Settings")]
	public GameObject[] enemyPrefabs; 
	public int zoneLevel; // 1, 2, 3, 4 구역 설정
	public float spawnInterval = 3f;

	private Collider2D zoneCollider;

	void Start()
	{
		zoneCollider = GetComponent<Collider2D>();
		StartCoroutine(SpawnRoutine());
	}

	IEnumerator SpawnRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnInterval);

			// 핵심 로직: 현재 구역 레벨 이하의 적들 중 랜덤 소환
			// zoneLevel이 1이면 0번만, 2면 0~1번 중 랜덤
			int enemyIndex = Random.Range(0, zoneLevel);
			SpawnEnemy(enemyPrefabs[enemyIndex]);
		}
	}

	void SpawnEnemy(GameObject prefab)
	{
		// Collider 영역 내 랜덤 좌표 생성
		Vector2 spawnPos = GetRandomPointInCollider();
		Instantiate(prefab, spawnPos, Quaternion.identity);
	}

	Vector2 GetRandomPointInCollider()
	{
		Bounds bounds = zoneCollider.bounds;
		Vector2 point;
		do
		{
			point = new Vector2(
				Random.Range(bounds.min.x, bounds.max.x),
				Random.Range(bounds.min.y, bounds.max.y)
			);
		} while (!zoneCollider.OverlapPoint(point)); // 폴리곤 콜라이더 영역 안일 때까지 반복

		return point;
	}
}
