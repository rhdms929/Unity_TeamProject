using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSpawner : MonoBehaviour //각 Zone 안에서 몬스터 스폰, zone이 해금 됐는지 안됐는지
{
    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs;
    public int zoneLevel; // 1, 2, 3, 4 구역 설정
    public float spawnInterval = 3f;

    [HideInInspector] public bool isUnlocked = false; //플레이어가 현재 그 Zone 안에 있는지 확인 하기 위해

    private Collider2D zoneCollider;
    private bool playerInZone = false; //zone에 있는지 없는지 

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

            //해금 됐는지 안됐는지
            if (!isUnlocked)
                continue;

            // 플레이어가 이 구역 안에 없으면 스폰 안 함
            if (!playerInZone)
                continue;

            // zoneLevel만큼만 사용
            int maxIndex = Mathf.Min(zoneLevel, enemyPrefabs.Length);

            if (maxIndex <= 0)
                continue;

            // 핵심 로직: 현재 구역 레벨 이하의 적들 중 랜덤 소환
            // zoneLevel이 1이면 0번만, 2면 0~1번 중 랜덤
            int enemyIndex = Random.Range(0, maxIndex);
            SpawnEnemy(enemyPrefabs[enemyIndex]);
        }
    }

    void SpawnEnemy(GameObject prefab)
    {
        // Collider 영역 내 랜덤 좌표 생성
        Vector2 spawnPos = GetRandomPointInCollider();
		GameObject enemy = ObjectPoolManager.instance.GetGo(prefab.name);

		if (enemy != null)
			enemy.transform.position = spawnPos;
	}

    Vector2 GetRandomPointInCollider()
    {
        Bounds bounds = zoneCollider.bounds;
        Vector2 point;

        int maxTries = 30;
        int count = 0;

		int waterLayer = LayerMask.GetMask("Obstacle");

		do
        {
            point = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );
            count++;
        } while (count < maxTries &&
			(!zoneCollider.OverlapPoint(point) || Physics2D.OverlapPoint(point, waterLayer) != null));
		return point;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isUnlocked) return;
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }
}