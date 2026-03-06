using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private GameObject enemyPrefab;		//	나중에 몬스터가 더 추가되면 리스트로 변경해야됨
	[SerializeField] private float spawnInterval = 3.0f;    //	몬스터가 생성되는 간격
	private float timer;

	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= spawnInterval)
		{
			SpawnEnemy();
			timer = 0; // 초기화
		}
	}

	void SpawnEnemy()
	{
		// 화면 밖에서 랜덤하게 생성되게 하거나 특정 범위 내에서 생성
		Vector2 randomPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
		Instantiate(enemyPrefab, randomPos, Quaternion.identity);
	}
}