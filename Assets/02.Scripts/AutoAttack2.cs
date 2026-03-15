using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoAttack2 : MonoBehaviour
{
    [Header("Settings")]
    public bool isAutoMode = true;   // 자동 공격 모드 ON/OFF
    public float detectRange = 1.5f; // 적을 발견하는 범위
    public float attackDelay = 1f;   // 멈춰서 공격할 거리
    public int damage = 10;          // 공격 데미지

    private float timer = 0f;
    private Animator anim;
    private SpriteRenderer sr;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (!isAutoMode) return;

        Transform target = GetNearestEnemy();  // 가장 가까운 적 찾기

        if (target == null) return;   // 적이 없으면 종료
        LookTarget(target);

        timer += Time.deltaTime; 

        if (timer < attackDelay) return; //공격 쿨타임? 같은거

        Attack(target); 
        timer = 0f;
    }

    private void LookTarget(Transform target) // 적 바라보기 flip
    {
        float dirX = target.position.x - transform.position.x;

        if (dirX > 0.01f) sr.flipX = false;
        else if (dirX < -0.01f) sr.flipX = true;
    }

    private void Attack(Transform target) //공격
    {
        anim.SetTrigger("Attack");

        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }

    private Transform GetNearestEnemy() //가장 가까운 적 찾기
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearest = null;
        float minDistance = Mathf.Infinity;  // 최소 거리 저장

        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy) continue; // 비활성화된 오브젝트는 제외

            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance > detectRange) continue; // 감지 범위 밖이면 제외

            if (distance < minDistance) // 현재까지 가장 가까운 적이면 저장
            {
                minDistance = distance;
                nearest = enemy.transform;
            }
        }
        return nearest;   // 가장 가까운 적 반환
    }

    public void ToggleAutoAttack()  // 자동 공격 모드 ON/OFF
    {
        isAutoMode = !isAutoMode;
    }
}