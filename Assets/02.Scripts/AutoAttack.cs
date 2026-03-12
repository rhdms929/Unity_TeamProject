using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoAttack : MonoBehaviour
{
	[Header("Settings")]
	public bool isAutoMode = false;
	public float detectRange = 10f;     // 적을 발견하는 범위
	public float attackRange = 1.2f;    // 멈춰서 공격할 거리
	public float moveSpeed = 4f;        // 추격 속도
	public float attackDelay = 1f;
	public int damage = 10;

	private float timer = 0f;
	private Animator anim;
	private SpriteRenderer sr;
	private Rigidbody2D rb;

	void Start()
	{
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (!isAutoMode) return;

		Transform target = GetNearestEnemy();

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
		Vector2 direction = (target.position - transform.position).normalized;

		transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

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
			PerformAttack(target);
			timer = 0f;
		}
	}

	void PerformAttack(Transform target)
	{
		if (target == null) return;
		anim.SetTrigger("Attack");

		Enemy enemy = target.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.TakeDamage(damage);
		}
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
		// 자동 모드가 꺼지면 속도 0으로 초기화
		if (!isAutoMode) anim.SetFloat("Speed", 0f);
	}
}
