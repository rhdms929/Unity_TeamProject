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
	public float nextWaypointDistance = 0.3f; // 다음 노드로 넘어가기 위한 거리
	private Pathfinding pathfinding;
	private List<Node> path;
	private int targetIndex;

	[Header("Drop")]
	[SerializeField] private string dropItemKey = "Coin";

	private Rigidbody2D rb;
	private Animator anim;
	private SpriteRenderer sr;
	private Collider2D col;

	private float attackTimer;
	private int currentHP;
	private bool isDead;
    private bool hasDetectedPlayer; //플레이어를 감지했는지
   // private Coroutine alertCoroutine; //alert코루틴 함수
    private Coroutine returnCoroutine;  //	풀링 쓰면서 코루틴 함수

    [Header("Reward")] //경험치 보상
    public int expReward = 10;

    [Header("Info")]
    public string monsterName = "고블린";

    [Header("Alert")]
    public GameObject alertIcon;   // 느낌표 오브젝트
                                   //public float alertShowTime = 0.6f; // 표시 시간

    [Header("Damage Text")] //적 위에 데미지 수치 보이게 하기
    public GameObject damageTextPrefab;
    public Vector3 damageTextOffset = new Vector3(0, 1.2f, 0);

    private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		col = GetComponent<Collider2D>();
		pathfinding = FindObjectOfType<Pathfinding>();
	}

	private void OnEnable() //OnEnable() -> 풀에서 꺼낼 때마다 실행해서 함수 바꿨으요잉~
	{
		currentHP = maxHP;
		isDead = false;
		attackTimer = 0f;
		path = null; // 경로 초기화
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

		// 0.5초마다 경로를 갱신 -> 성능 최적화
		InvokeRepeating("UpdatePath", 0f, 0.5f);
	}

	private void OnDisable()
	{
		CancelInvoke("UpdatePath");
        if (alertIcon != null)
            alertIcon.SetActive(false);
    }

	void UpdatePath()
	{
		if (isDead || target == null || pathfinding == null) return;

		float distance = Vector2.Distance(transform.position, target.position);

		// 감지 범위 내에 있을 때만 길을 찾기
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

        // 처음 감지한 순간 느낌표 표시
        if (distance <= detectRange && !hasDetectedPlayer)
        {
            hasDetectedPlayer = true;
            ShowAlert();
        }

        // 감지 범위 밖 -> 느낌표 끄기
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

		// 공격
		if (distance <= attackRange)
		{
			StopMoving();
			AttackPlayer();
			return;
		}

		// 추적 중 (A* 경로 따라가기)
		FollowPath();
	}

	void FollowPath()
	{
		if (path == null || targetIndex >= path.Count)
		{
			// 경로가 끝났는데도 플레이어가 멀리 있다면 다시 길찾기 시도
			if (Vector2.Distance(transform.position, target.position) > attackRange)
			{
				UpdatePath();
			}
			return;
		}

		Vector3 targetWayPoint = path[targetIndex].worldPos;
		// 적의 현재 위치에서 목표 노드를 향한 방향 계산

		Vector2 direction = ((Vector2)targetWayPoint - (Vector2)transform.position).normalized;

		// 플레이어 방향을 바라보도록 flipX 설정 (노드 방향이 아니라 실제 플레이어 방향 기준)
		Vector2 dirToPlayer = (target.position - transform.position).normalized;
		sr.flipX = dirToPlayer.x < 0;

		anim.SetFloat("Speed", 1f);

		// 이동 처리
		transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, speed * Time.deltaTime);

		// 현재 노드에 충분히 가까워지면 다음 노드로
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
    void ShowAlert() //!표시
    {
        if (alertIcon == null) return;

        alertIcon.SetActive(true);
    }

    public void TakeDamage(int damage)
	{
		if (isDead) return;
        ShowDamageText(damage); //데미지 수치 보이게 
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
		if (alertIcon != null) //죽으면 ! 꺼주기
		{
			alertIcon.SetActive(false);
		}
        // 골드 소환은 코루틴에게 맡깁니다.
        returnCoroutine = StartCoroutine(ReturnToPoolAfterDelay());

        PlayerStats PlayerStats = FindObjectOfType<PlayerStats>(); //적이 죽으면 경험치 보상
        if (PlayerStats != null)
        {
            PlayerStats.GainExp(expReward);
        }
		// 퀘스트 매니저에게 몬스터가 죽었다고 알리기
		if (QuestManager.Instance != null)
		{
			QuestManager.Instance.OnMonsterKilled(gameObject.name);
		}
	}
	IEnumerator ReturnToPoolAfterDelay()
	{
		// deathDestroyDelay 시간만큼 기다립니다 (애니메이션 재생 시간 등)
		yield return new WaitForSeconds(deathDestroyDelay);

		// 골드 생성
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
		// 20% 확률 포션 드랍
		CheckPotionDrop();
		// 적 오브젝트를 풀로 반환
		ReleaseObject();
	}

	// 확률을 계산하고 아이템 슬롯에 직접 넣기
	void CheckPotionDrop()
	{
		// 1. 일단 20% 확률로 아이템이 나올지 결정
		if (Random.Range(0, 100) < 20)
		{
			// 2. 아이템이 나오게 된다면 이번에 줄 아이템이 HP인지 MP인지 랜덤 결정 (0 또는 1)
			// 0이면 HP_Potion, 1이면 MP_Potion
			ItemSlot.ItemType selectedType = (Random.Range(0, 2) == 0)
				? ItemSlot.ItemType.HP_Potion
				: ItemSlot.ItemType.MP_Potion;

			// 3. 씬에 있는 모든 ItemSlot을 뒤져서 선택된 타입과 일치하는 슬롯에 아이템 추가
			ItemSlot[] allSlots = FindObjectsOfType<ItemSlot>();
			foreach (ItemSlot slot in allSlots)
			{
				if (slot.type == selectedType)
				{
					slot.AddItem(1);
					break; 
				}
			}
		}
	}

	// 적 범위 시각화입니당
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, detectRange);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);

		// 현재 경로 시각화 (디버그용)
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
    void ShowDamageText(int damage) //적 데미지 수치 보이게
    {
        if (damageTextPrefab == null) return;

        GameObject textObj = Instantiate(damageTextPrefab, transform.position + damageTextOffset, Quaternion.identity);
        DamageText damageText = textObj.GetComponent<DamageText>();

        if (damageText != null)
        {
            damageText.SetDamage(damage);
        }
    }
}