using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AutoAttack : MonoBehaviour
{
	[Header("Settings")]
	public bool isAutoMode = true;
	public float detectRange = 10f;         
	public float attackRange = 1.2f;    
	public float moveSpeed = 4f;        
	public float attackDelay = 1f;
	public int damage = 10;

	[Header("Mana Settings")]
	public float attackManaCost = 2f;                     

	private float timer = 0f;
	private Animator anim;
	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private PlayerStats playerStats;
	Pathfinding pathfinding;
	List<Node> currentPath;
	int pathIndex = 0;
	float pathUpdateTimer = 0f;
	public float pathUpdateDelay = 0.5f; 

	private Transform currentTarget;
	private float targetUpdateTimer = 0f;
	public float targetUpdateDelay = 0.5f;
	void Awake()
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

		targetUpdateTimer += Time.deltaTime;
		if (targetUpdateTimer >= targetUpdateDelay)
		{
			currentTarget = GetNearestEnemy();
			targetUpdateTimer = 0f;
		}

		if (currentTarget != null && currentTarget.gameObject.activeInHierarchy)
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
			{
				pathIndex = currentPath.Count - 1;
			}
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
		{
			enemy.TakeDamage(damage);
		}
	}
	Transform GetNearestEnemy()
	{
		Transform nearestEnemy = null;
		float minDistance = Mathf.Infinity;
		foreach (Enemy enemy in Enemy.ActiveEnemies)
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