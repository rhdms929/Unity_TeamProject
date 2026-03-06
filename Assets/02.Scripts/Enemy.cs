using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
	public float speed;
	public Transform target;

	void Start()
	{
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");  // 태그로 찾기

		if (playerObj != null)
		{
			target = playerObj.transform;
		}
	}

	void FixedUpdate()
	{
		if (target == null) return; 

		Vector2 direction = (target.position - transform.position).normalized;
		GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + direction * speed * Time.fixedDeltaTime);
	}

	public void TakeDamage(int damage)
	{
		Destroy(gameObject);
	}
}