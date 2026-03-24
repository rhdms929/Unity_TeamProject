using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoAttack : MonoBehaviour
{
	[Header("Settings")]
	public bool isAutoMode = true;
	public float detectRange = 10f;     // 적을 발견하는 범위
	public float attackRange = 1.2f;    // 멈춰서 공격할 거리
	public float moveSpeed = 4f;        // 추격 속도
	public float attackDelay = 1f;
	public int damage = 10;

	[Header("Mana Settings")]
	public float attackManaCost = 2f; // 공격 한 번당 소모할 마나량

	private float timer = 0f;
	private Animator anim;
	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private PlayerStatus playerStatus;

	Pathfinding pathfinding;
	List<Node> currentPath;
	int pathIndex = 0;
	float pathUpdateTimer = 0f;
	public float pathUpdateDelay = 0.5f; // 경로 재계산 주기

	void Start()
	{
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		playerStatus = GetComponent<PlayerStatus>();

		pathfinding = FindObjectOfType<Pathfinding>(); 
		isAutoMode = true;
	}

	void Update()
	{
		if (!isAutoMode) return;

		Transform target = GetNearestEnemy();

		// 현재 타겟이 없거나, 타겟이 죽었거나(비활성화), 너무 멀어졌을 때만 새로 찾기
		if (target == null || !target.gameObject.activeInHierarchy || Vector2.Distance(transform.position, target.position) > detectRange)
		{
			target = GetNearestEnemy();
		}

		if (target != null)
		{
			float distance = Vector2.Distance(transform.position, target.position);

			if (distance > attackRange)
			{
				// 추격
				ChaseEnemy(target);
			}
			else
			{
				// 공격
				StopAndAttack(target);
			}
		}
		else
		{
			anim.SetFloat("Speed", 0f);
		}
	}

	void ChaseEnemy(Transform target)
	{
		pathUpdateTimer += Time.deltaTime;

		// 일정 시간마다 경로 다시 계산
		if (pathUpdateTimer >= pathUpdateDelay)
		{
			currentPath = pathfinding.FindPath(transform.position, target.position);
			pathIndex = 0;
			pathUpdateTimer = 0f;
		}

		if (currentPath == null || pathIndex >= currentPath.Count) return;

		Vector3 targetPos = currentPath[pathIndex].worldPos;
		Vector2 direction = (targetPos - transform.position).normalized;

		transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

		// 노드에 도착하면 다음 노드로 이동
		if (Vector2.Distance(transform.position, targetPos) < 0.2f)
		{
			pathIndex++;
			if (pathIndex >= currentPath.Count)
			{
				pathIndex = currentPath.Count - 1;
			}
		}

		anim.SetFloat("Speed", 1f);

		if (direction.x > 0) sr.flipX = false;
		else if (direction.x < 0) sr.flipX = true;
	}



	void StopAndAttack(Transform target) // 멈춰서 공격
	{
		anim.SetFloat("Speed", 0f);

		timer += Time.deltaTime;
		if (timer >= attackDelay)
		{
			if (playerStatus != null && playerStatus.UseMana(attackManaCost))
			{
				PerformAttack(target); // 실제 공격 실행
				timer = 0f;            
			}
		}
	}

	void PerformAttack(Transform target)    // 공격
	{
		if (target == null) return;
		anim.SetTrigger("Attack");

		Enemy enemy = target.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.TakeDamage(damage);
		}
	}

	Transform GetNearestEnemy() //	가장 가까운 적 찾기
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		Transform nearestEnemy = null;
		float minDistance = Mathf.Infinity;

		foreach (GameObject enemy in enemies)
		{
			float distance = Vector2.Distance(transform.position, enemy.transform.position);
			if (distance < minDistance && distance <= detectRange)
			{
				minDistance = distance;
				nearestEnemy = enemy.transform;
			}
		}
		return nearestEnemy;
	}

	public void ToggleAutoAttack()
	{
		isAutoMode = !isAutoMode;
		// 자동 모드가 꺼지면 속도 0으로 초기화
		if (!isAutoMode) anim.SetFloat("Speed", 0f);
	}
}
