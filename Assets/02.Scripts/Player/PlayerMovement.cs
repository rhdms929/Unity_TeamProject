using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	// --- 컴포넌트 및 설정 ---
	public float speed = 3f;

	Rigidbody2D rb;
	SpriteRenderer sr;
	Animator anim;
    private Vector2 movement;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");

		// 방향 전환
		if(movement.x > 0)
		{
			sr.flipX = false;
		}
		else if (movement.x < 0)
		{
			sr.flipX = true;
		}
	}
	void FixedUpdate()
	{
		Vector2 moveAmount = movement.normalized * speed * Time.fixedDeltaTime;
		rb.MovePosition(rb.position + moveAmount);
	}

	void LateUpdate()
	{
		anim.SetFloat("Speed", movement.sqrMagnitude);
	}
}