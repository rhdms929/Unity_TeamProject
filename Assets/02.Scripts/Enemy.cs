using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
	public float speed;
	public Transform target;

    [Header("Enemy AI")]
    public float detectRange = 5f;   // 플레이어 감지 범위
    public float attackRange = 1f;   // 공격 범위
    public int damage = 1;           // 플레이어에게 줄 데미지
    public float attackCooldown = 1f;
    public int maxHP = 3;
    public float deathDestroyDelay = 1.5f; //죽는 애니메이션 길게 하려고

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr; //Flip하기 위해 
    private Collider2D col; //죽었을때 충돌 수정 하려고

    private float attackTimer;
    private int currentHP;
    private bool isDead; 

    public GameObject goldPrefab; // 골드 프리팹

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");  // 태그로 찾기

        currentHP = maxHP;

        if (playerObj != null)
		{
			target = playerObj.transform;
		}
	}
	void Update()
	{
        if (isDead) return;

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }
	void FixedUpdate()
	{
        if (isDead) return;
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        // 감지 범위 밖
        if (distance > detectRange)
        {
            rb.velocity = Vector2.zero;
            anim.SetFloat("Speed", 0f);
            return;
        }
        // 공격
        if (distance <= attackRange)
        {
            rb.velocity = Vector2.zero;
            anim.SetFloat("Speed", 0f);
            AttackPlayer();
            return;
        }
        // 추적 중 
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        if (direction.x > 0.01f)
        {
            sr.flipX = false;
        }
        else if (direction.x < -0.01f)
        {
            sr.flipX = true;
        }
        anim.SetFloat("Speed", 1f);
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
    void AttackPlayer()
    {
        if (isDead) return;
        if (attackTimer > 0) return;

        anim.SetTrigger("Attack");

        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        attackTimer = attackCooldown;
    }
    public void TakeDamage(int damage)
	{
        if (isDead) return;

        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // 이동 멈춤
        rb.velocity = Vector2.zero;

        // 충돌 끄기
        if (col != null)
        {
            col.enabled = false;
        }

        // 물리 멈추기
        if (rb != null)
        {
            rb.simulated = false;
        }

        // 죽는 애니메이션
        if (anim != null)
        {
            anim.SetTrigger("Death");
        }
        // 죽은 자리에 골드 소환
        if (goldPrefab != null)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject, deathDestroyDelay); //죽는 애니메이션 보여주고 코인 드랍
    }

    //적 범위 시각화입니당 넹 ~ ㅋㅋ답장한다 답장!!
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}