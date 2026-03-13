using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Skill")]
    public string skillName = "Explosion";
    public int damage = 20;
    public float radius = 1.5f;
    public float cooldown = 1f;
    public float autoCastInterval = 1f;
    public float autoTargetRange = 10f;

    [Header("References")]
    public GameObject effectPrefab;
    public Transform player;
    public Camera mainCamera;
    public LayerMask enemyLayer;

    [Header("UI")]
    public Image autoCastMark;
    public Image cooldownOverlay;
    public Image selectedMark;

    private bool isSelected;
    private bool isAutoCastOn;
    private float cooldownTimer;
    private float autoCastTimer;

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
        UpdateCooldown();
        HandleManualCast();
        HandleAutoCast();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SelectSkill();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ToggleAutoCast();
        }
    }

    private void UpdateCooldown()
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

    private void HandleManualCast()
    {
        if (!isSelected) return;
        if (cooldownTimer > 0f) return;

        if (Input.GetMouseButtonDown(1))
        {
            isSelected = false;
            UpdateSelectedUI();
            return;
        }

        if (!Input.GetMouseButtonDown(0)) return;

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

    private void HandleAutoCast()
    {
        if (!isAutoCastOn) return;
        if (cooldownTimer > 0f) return;

        autoCastTimer += Time.deltaTime;
        if (autoCastTimer < autoCastInterval) return;

        autoCastTimer = 0f;

        Transform target = FindNearestEnemy();
        if (target == null) return;

        CastSkill(target.position);
    }

    private void SelectSkill()
    {
        isSelected = true;
        UpdateSelectedUI();
        Debug.Log(skillName + " 선택됨");
    }

    private void ToggleAutoCast()
    {
        isAutoCastOn = !isAutoCastOn;
        autoCastTimer = 0f;
        UpdateAutoCastUI();

        Debug.Log(skillName + " 자동시전 : " + isAutoCastOn);
    }

    private void CastSkill(Vector3 centerPos)
    {
        cooldownTimer = cooldown;

        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, centerPos, Quaternion.identity);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(centerPos, radius, enemyLayer);

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

    private Transform FindNearestEnemy()
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

    private void RefreshUI()
    {
        UpdateAutoCastUI();
        UpdateCooldownUI();
        UpdateSelectedUI();
    }

    private void UpdateAutoCastUI()
    {
        if (autoCastMark != null)
        {
            autoCastMark.enabled = isAutoCastOn;
        }
    }

    private void UpdateCooldownUI()
    {
        if (cooldownOverlay == null) return;

        cooldownOverlay.fillAmount = cooldown <= 0f ? 0f : cooldownTimer / cooldown;
    }

    private void UpdateSelectedUI()
    {
        if (selectedMark != null)
        {
            selectedMark.enabled = isSelected;
        }
    }
}