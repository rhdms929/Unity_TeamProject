using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolAble, IDamageable
{
	public float speed = 3f;
	public Transform target;

	[Header("Enemy AI")]
	public float detectRange = 5f;
	public float attackRange = 1f;
	public int damage = 1;
	public float attackCooldown = 1f;
	public int maxHP = 3;
	public float deathDestroyDelay = 1.5f;

	[Header("A* Pathfinding")]
	public float nextWaypointDistance = 0.3f; 
	private Pathfinding pathfinding;
	private List<Node> path;
	private int targetIndex;

	public static readonly List<Enemy> ActiveEnemies = new List<Enemy>();

    [Header("Drop")]
    [SerializeField] private string dropItemKey = "Coin";

    [Header("Potion Drop")]
    public ItemData hpPotionData;
    public ItemData mpPotionData;
    [Range(0, 100)] public int potionDropChance = 20;

    private Rigidbody2D rb;
	private Animator anim;
	private SpriteRenderer sr;
	private Collider2D col;
	private PlayerStats playerStats;

	private float attackTimer;
	private int currentHP;
	private bool isDead;
    private bool hasDetectedPlayer;
    private Coroutine returnCoroutine;  

    [Header("Reward")] 
    public int expReward = 10;

    [Header("Info")]
    public string monsterName = "머쉬룸";

    [Header("Alert")]
    public GameObject alertIcon;  

    [Header("Damage Text")]
    public Vector3 damageTextOffset = new Vector3(0, 1.2f, 0);

    private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		col = GetComponent<Collider2D>();
		pathfinding = FindObjectOfType<Pathfinding>();
		playerStats = FindObjectOfType<PlayerStats>();
	}

	private void OnEnable() 
	{
		currentHP = maxHP;
		isDead = false;
		attackTimer = 0f;
		path = null; 
        hasDetectedPlayer = false;

        if (rb != null)
		{
			rb.velocity = Vector2.zero;
			rb.simulated = true;
		}

		if (col != null) col.enabled = true;

		if (returnCoroutine != null)
		{
			StopCoroutine(returnCoroutine);
			returnCoroutine = null;
		}

        if (target == null)
		{
			GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
			if (playerObj != null) target = playerObj.transform;
		}

	
		InvokeRepeating("UpdatePath", 0f, 0.5f);
		ActiveEnemies.Add(this);
	}

	private void OnDisable()
	{
		CancelInvoke("UpdatePath");
		ActiveEnemies.Remove(this);
        if (alertIcon != null)
            alertIcon.SetActive(false);
    }

	void UpdatePath()
	{
		if (isDead || target == null || pathfinding == null) return;

		float distance = Vector2.Distance(transform.position, target.position);

	
		if (distance <= detectRange && distance > attackRange)
		{
			path = pathfinding.FindPath(transform.position, target.position);
			targetIndex = 0;
		}
	}

	void Update()
	{
		if (isDead) return;

		if (attackTimer > 0) attackTimer -= Time.deltaTime;
	}

	void FixedUpdate()
	{
		if (isDead || target == null) return;

		float distance = Vector2.Distance(transform.position, target.position);

  
        if (distance <= detectRange && !hasDetectedPlayer)
        {
            hasDetectedPlayer = true;
            ShowAlert();
        }


        if (distance > detectRange)
		{
            hasDetectedPlayer = false;
            if (alertIcon != null)
			{
				alertIcon.SetActive(false);
            }
            StopMoving();
			return;
		}


		if (distance <= attackRange)
		{
			StopMoving();
			AttackPlayer();
			return;
		}

		FollowPath();
	}

	void FollowPath()
	{
		if (path == null || targetIndex >= path.Count)
		{

			if (Vector2.Distance(transform.position, target.position) > attackRange)
			{
				UpdatePath();
			}
			return;
		}

		Vector3 targetWayPoint = path[targetIndex].worldPos;

		Vector2 dirToPlayer = (target.position - transform.position).normalized;
		sr.flipX = dirToPlayer.x < 0;

		anim.SetFloat("Speed", 1f);


		transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, speed * Time.deltaTime);


		if (Vector3.Distance(transform.position, targetWayPoint) < nextWaypointDistance)
		{
			targetIndex++;
		}
    }

	void StopMoving()
	{
		rb.velocity = Vector2.zero;
		anim.SetFloat("Speed", 0f);
	}

	void AttackPlayer()
	{
		if (isDead || attackTimer > 0) return;

		anim.SetTrigger("Attack");

		IDamageable damageable = target.GetComponent<IDamageable>();
		if (damageable != null)
		{
			damageable.TakeDamage(damage);
		}
		attackTimer = attackCooldown;
	}
    void ShowAlert() 
    {
        if (alertIcon == null) return;

        alertIcon.SetActive(true);
    }

    public void TakeDamage(int damage)
	{
		if (isDead) return;
        ShowDamageText(damage); 
        currentHP -= damage;
		if (currentHP <= 0) Die();
	}

	public void Die()
	{
		if (isDead) return;
		isDead = true;
		CancelInvoke("UpdatePath");

		if (col != null) col.enabled = false;

		if (rb != null)
		{
			rb.velocity = Vector2.zero;
			rb.simulated = false;
		}

		if (anim != null)
		{
			anim.SetFloat("Speed", 0f);
			anim.SetTrigger("Death");
		}
		if (alertIcon != null) 
		{
			alertIcon.SetActive(false);
		}

        returnCoroutine = StartCoroutine(ReturnToPoolAfterDelay());

        if (playerStats != null)
        {
            playerStats.GainExp(expReward);
        }

		if (QuestManager.Instance != null)
		{
			QuestManager.Instance.OnMonsterKilled(gameObject.name);
		}
	}
	IEnumerator ReturnToPoolAfterDelay()
	{
		// deathDestroyDelay 
		yield return new WaitForSeconds(deathDestroyDelay);

		GameObject dropItem = ObjectPoolManager.instance.GetGo(dropItemKey);
		if (dropItem != null)
		{
            dropItem.transform.position = transform.position;
            dropItem.transform.rotation = Quaternion.identity;

            GoldItem goldItem = dropItem.GetComponent<GoldItem>();
            if (goldItem != null)
            {
                goldItem.goldAmount = 1;
                goldItem.sourceMonsterName = monsterName;
            }
        }
		CheckPotionDrop();
		ReleaseObject();
	}

    void CheckPotionDrop()
    {
        if (InventoryManager.Instance == null) return;

        if (Random.Range(0, 100) >= potionDropChance) return;

        ItemData selectedPotion = (Random.Range(0, 2) == 0) ? hpPotionData : mpPotionData;

        if (selectedPotion == null) return;

        InventoryManager.Instance.AddItem(selectedPotion, 1);
    }

    void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, detectRange);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);

		if (path != null)
		{
			Gizmos.color = Color.cyan;
			for (int i = targetIndex; i < path.Count; i++)
			{
				Gizmos.DrawCube(path[i].worldPos, Vector3.one * 0.2f);
				if (i == targetIndex) Gizmos.DrawLine(transform.position, path[i].worldPos);
				else Gizmos.DrawLine(path[i - 1].worldPos, path[i].worldPos);
			}
		}
	}
    void ShowDamageText(int damage)
    {
        if (ObjectPoolManager.instance == null) return;

        GameObject textObj = ObjectPoolManager.instance.GetGo("DamageText");
        if (textObj == null) return;

        textObj.transform.position = transform.position + damageTextOffset;
        textObj.transform.rotation = Quaternion.identity;

        DamageText damageText = textObj.GetComponent<DamageText>();
        if (damageText != null)
        {
            damageText.SetDamage(damage);
        }
    }
}