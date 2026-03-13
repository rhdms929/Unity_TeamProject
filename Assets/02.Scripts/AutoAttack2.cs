using UnityEngine;

public class AutoAttack2 : MonoBehaviour
{
    [Header("Settings")]
    public bool isAutoMode = true;
    public float detectRange = 1.5f;
    public float attackDelay = 1f;
    public int damage = 10;

    private float timer;
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

        Transform target = GetNearestEnemy();
        if (target == null) return;

        LookTarget(target);

        timer += Time.deltaTime;
        if (timer < attackDelay) return;

        Attack(target);
        timer = 0f;
    }

    private void LookTarget(Transform target)
    {
        float dirX = target.position.x - transform.position.x;

        if (dirX > 0.01f) sr.flipX = false;
        else if (dirX < -0.01f) sr.flipX = true;
    }

    private void Attack(Transform target)
    {
        anim.SetTrigger("Attack");

        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }

    private Transform GetNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance > detectRange) continue;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

    public void ToggleAutoAttack()
    {
        isAutoMode = !isAutoMode;
    }
}