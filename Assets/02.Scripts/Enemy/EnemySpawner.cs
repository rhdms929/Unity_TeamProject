using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 3f; //	몬스터가 생성되는 간격

    private PathGrid pathGrid; // A* 경로 탐색을 위한 그리드 참조
	private float timer;
    void Start()
    {
        pathGrid = FindObjectOfType<PathGrid>();
	}

	void Update()
	{
        if (ObjectPoolManager.instance == null) return;
        if (ObjectPoolManager.instance.IsReady == false) return;
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }
    void SpawnEnemy()
	{
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[randomIndex];

        if (selectedPrefab == null) return;

		// A* 경로 탐색 그리드에서 유효한 스폰 위치 찾기
		Vector2 spawnPos = GetValidSpawnPosition();

		GameObject enemy = ObjectPoolManager.instance.GetGo(selectedPrefab.name);
        if (enemy == null) return;

        enemy.transform.position = spawnPos;
        enemy.transform.rotation = Quaternion.identity;
    }

	Vector2 GetValidSpawnPosition()
	{
		Vector2 randomPos = Vector2.zero;
		bool isValid = false;
		int attempts = 0;

		// 최대 20번 시도
		while (!isValid && attempts < 20)
		{
			randomPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));

			if (pathGrid != null)
			{
				Node node = pathGrid.NodeFromWorldPoint(randomPos);
				// 해당 위치의 노드가 이동 가능한(walkable) 곳인지 확인
				if (node != null && node.walkable)
				{
					isValid = true;
				}
			}
			else
			{
				// PathGrid를 못 찾았다면 루프 탈출
				isValid = true;
			}
			attempts++;
		}

		return randomPos;
	}
}