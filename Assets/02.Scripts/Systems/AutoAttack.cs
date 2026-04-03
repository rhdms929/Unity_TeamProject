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
	private PlayerStats playerStats;

	Pathfinding pathfinding;
	List<Node> currentPath;
	int pathIndex = 0;
	float pathUpdateTimer = 0f;
	public float pathUpdateDelay = 0.5f; // 경로 재계산 주기

	// 적 탐색 최적화
	private Transform currentTarget;
	private float targetUpdateTimer = 0f;
	private float targetUpdateDelay = 0.3f;

	void Start()
	{
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		playerStats = GetComponent<PlayerStats>();

		pathfinding = FindObjectOfType<Pathfinding>(); 
		isAutoMode = true;
	}

	void Update()
	{
		if (!isAutoMode) return;

		// 타겟이 없거나 죽었거나 범위 벗어나면 일정 시간마다 다시 탐색
		targetUpdateTimer += Time.deltaTime;
		if (targetUpdateTimer >= targetUpdateDelay)
		{
			targetUpdateTimer = 0f;

			if (currentTarget == null
				|| !currentTarget.gameObject.activeInHierarchy
				|| Vector2.Distance(transform.position, currentTarget.position) > detectRange)
			{
				currentTarget = GetNearestEnemy();
			}
		}

		if (currentTarget != null)
		{
			float distance = Vector2.Distance(transform.position, currentTarget.position);
			if (distance > attackRange)
				ChaseEnemy(currentTarget);
			else
				StopAndAttack(currentTarget);
		}
		else
		{
			anim.SetFloat("Speed", 0f);
		}
	}

	void ChaseEnemy(Transform target)
	{
		pathUpdateTimer += Time.deltaTime;
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

		if (Vector2.Distance(transform.position, targetPos) < 0.2f)
		{
			pathIndex++;
			if (pathIndex >= currentPath.Count)
				pathIndex = currentPath.Count - 1;
		}

		anim.SetFloat("Speed", 1f);
		if (direction.x > 0) sr.flipX = false;
		else if (direction.x < 0) sr.flipX = true;
	}

	void StopAndAttack(Transform target)
	{
		anim.SetFloat("Speed", 0f);
		timer += Time.deltaTime;

		if (timer >= attackDelay)
		{
			if (playerStats != null && playerStats.UseMana(attackManaCost))
			{
				PerformAttack(target);
				timer = 0f;
			}
		}
	}

	void PerformAttack(Transform target)
	{
		if (target == null) return;
		anim.SetTrigger("Attack");

		Enemy enemy = target.GetComponent<Enemy>();
		if (enemy != null)
			enemy.TakeDamage(damage);
	}

	Transform GetNearestEnemy()
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
		if (!isAutoMode) anim.SetFloat("Speed", 0f);
	}
}