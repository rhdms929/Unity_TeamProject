using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Skill")]
    public string skillName = "Explosion";
    public int damage = 20;                // 스킬 데미지
    public float radius = 1.5f;            // 스킬 범위
    public float cooldown = 1f;            // 스킬 쿨타임
    public float autoCastInterval = 1f;    // 자동 시전 간격
    public float autoTargetRange = 10f;    // 자동 시전이 찾는 적 범위

    [Header("References")]
    public GameObject effectPrefab;
    public Transform player;
    public Camera mainCamera;
    public LayerMask enemyLayer;

    [Header("UI")]
    public Image autoCastMark;
    public Image cooldownOverlay;
    public Image selectedMark;

	[Header("Skill Settings")]
	public float manaCost = 10f;    //스킬당 소모 마나 설정

	private bool isSelected;       // 현재 스킬이 선택된 상태인지
    private bool isAutoCastOn;     // 자동 시전 ON/OFF 상태
    private float cooldownTimer;   // 쿨타임 타이머
    private float autoCastTimer;   // 자동시전 타이머

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        RefreshUI();
    }

    private void Update()
    {
        UpdateCooldown();    // 쿨타임 감소
        HandleManualCast();  // 수동
        HandleAutoCast();    // 자동
    }
    public void OnPointerClick(PointerEventData eventData) // UI 버튼 클릭 이벤트
    {
        if (eventData.button == PointerEventData.InputButton.Left) // 좌클릭 → 스킬 선택
        {
            SelectSkill();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)  // 우클릭 → 자동시전 ON/OFF
        {
            ToggleAutoCast();
        }
    }
    private void UpdateCooldown()  // 쿨타임 관리
    {
        if (cooldownTimer <= 0f)
        {
            cooldownTimer = 0f;
            UpdateCooldownUI();
            return;
        }

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0f) cooldownTimer = 0f;

        UpdateCooldownUI();
    }
    private void HandleManualCast()  // 수동일때
    {
        if (!isSelected) return; // 스킬 선택 상태 아니면 종료
        if (cooldownTimer > 0f) return;

        if (Input.GetMouseButtonDown(1)) // 우클릭하면 선택 취소
        {
            isSelected = false;
            UpdateSelectedUI();
            return;
        }

        if (!Input.GetMouseButtonDown(0)) return;  // 좌클릭이 아니면 종료

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;

        CastSkill(worldPos);

        isSelected = false;
        UpdateSelectedUI();
    }
    private void HandleAutoCast() // 자동 일때
    {
        if (!isAutoCastOn) return;
        if (cooldownTimer > 0f) return;

        autoCastTimer += Time.deltaTime;
        if (autoCastTimer < autoCastInterval) return;

        autoCastTimer = 0f;

        Transform target = FindNearestEnemy();  // 가장 가까운 적 찾기
        if (target == null) return;

        CastSkill(target.position); 
    }
    private void SelectSkill() // 스킬 선택
    {
        isSelected = true;
        UpdateSelectedUI();
    }
    private void ToggleAutoCast()  // 자동 시전 ON/OFF
    {
        isAutoCastOn = !isAutoCastOn;
        autoCastTimer = 0f;
        UpdateAutoCastUI();
    }
    private void CastSkill(Vector3 centerPos) // 스킬
    {
		PlayerStatus status = player.GetComponent<PlayerStatus>();

		if (status != null)
		{
			// 마나 사용 시도
			if (!status.UseMana(manaCost))
			{
				Debug.Log($"{skillName} 발동 실패: 마나 부족");
				return; // 마나가 없으면 여기서 함수를 종료해서 스킬이 안 나가게 함
			}
		}

		cooldownTimer = cooldown;

        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, centerPos, Quaternion.identity);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(centerPos, radius, enemyLayer); // 범위 안 적 찾기

        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }

        Debug.Log(skillName + " 발동");
    }
    private Transform FindNearestEnemy() // 가장 가까운 적 찾기
    {
        if (player == null) return null;

        Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, autoTargetRange, enemyLayer);

        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (!hit.gameObject.activeInHierarchy) continue;

            float distance = Vector2.Distance(player.position, hit.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = hit.transform;
            }
        }
        return nearest;
    }

    private void RefreshUI() // UI 초기화
    {
        UpdateAutoCastUI();
        UpdateCooldownUI();
        UpdateSelectedUI();
    }

    private void UpdateAutoCastUI() // 자동 시전 표시 UI
    {
        if (autoCastMark != null)
        {
            autoCastMark.enabled = isAutoCastOn;
        }
    }

    private void UpdateCooldownUI() // 쿨타임 UI 업데이트
    {
        if (cooldownOverlay == null) return;

        cooldownOverlay.fillAmount = cooldown <= 0f ? 0f : cooldownTimer / cooldown;
    }

    private void UpdateSelectedUI() // 선택 상태 UI
    {
        if (selectedMark != null)
        {
            selectedMark.enabled = isSelected;
        }
    }
}