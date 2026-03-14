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


    // --- 전투 시스템 ---
    //public Transform attackPoint;      
	//public float attackRange = 0.3f;   // 공격 반경
	//public LayerMask enemyLayer;       // 공격이 닿을 수 있는 적 레이어
	//public int attackDamage = 10;

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

		// 방향 전환
		if(movement.x > 0)
		{
			sr.flipX = false;
		}
		else if (movement.x < 0)
		{
			sr.flipX = true;
		}

		// 스페이스바 -> 공격
		//if(Input.GetKeyDown(KeyCode.Space))
		//{
		//	Attack();
		//}
	}

//	void Attack()
//	{
//		anim.SetTrigger("Attack");
//		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
//
//		// 공격 범위 내의 모든 적에게 데미지 적용
//		foreach (Collider2D hit in hitEnemies)
//		{
//			IDamageable damageable = hit.GetComponent<IDamageable>();
//
//			if (damageable != null)
//			{
//				damageable.TakeDamage(attackDamage);
//			}
//		}
//	}
//
//	// 공격 범위 시각화
//	void OnDrawGizmosSelected()
//	{
//		if (attackPoint == null) return;
//		Gizmos.color = Color.yellow;
//		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
//	}

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