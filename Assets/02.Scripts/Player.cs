using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed;
	Rigidbody2D rb;
	SpriteRenderer sr;
	private Vector2 movement;
	Animator anim;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");

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