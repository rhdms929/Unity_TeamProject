using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private GameObject enemyPrefab;		//	나중에 몬스터가 더 추가되면 리스트로 변경해야됨
    [SerializeField] private string enemyPoolKey ; //풀링
    [SerializeField] private float spawnInterval = 3.0f;    //	몬스터가 생성되는 간격
	private float timer;

	void Update()
	{
        if (ObjectPoolManager.instance == null) return;
        if (ObjectPoolManager.instance.IsReady == false) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
	{
        // 화면 밖에서 랜덤하게 생성되게 하거나 특정 범위 내에서 생성
        Vector2 randomPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));

        GameObject enemy = ObjectPoolManager.instance.GetGo(enemyPoolKey);
        if (enemy == null) return;

        enemy.transform.position = randomPos;
        enemy.transform.rotation = Quaternion.identity;
    }
}