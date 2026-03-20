using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatus : MonoBehaviour, IDamageable
{
	[Header(" Status Values ")]
	public int maxHp = 100;
	private int currentHp;
	public float maxMp = 50f;
	public float currentMp;

    [Header("MP Regen")]
    public float mpRegen = 2f; // 초당 마나 회복량
    public float mpRegenDelay = 1.5f; // 스킬 사용 후 회복 멈추는 시간
    private float mpRegenTimer = 0f;

    [Header(" UI References ")]
	public Image topHpFill;
	public Image bottomHpFill;
	public Image topMpFill;
	public Image bottomMpFill;
	public TextMeshProUGUI hpText;
	public TextMeshProUGUI mpText;
    public UIManager uiManager;

    [Header("Animation")]
	public Animator animator; 
	private bool isDead = false;
    void Awake()
	{
		currentHp = maxHp;
		currentMp = maxMp;

	}

	void Start()
	{
		UpdateAllStatusUI(); // 시작할 때 UI 초기화
	}

	void Update()
	{
        RegenerateMana(); //마나 회복

        float targetHpRatio = (float)currentHp / maxHp;
		float targetMpRatio = currentMp / maxMp;

		// HP 바 
		if (topHpFill != null) topHpFill.fillAmount = Mathf.Lerp(topHpFill.fillAmount, targetHpRatio, Time.deltaTime * 5f);
		if (bottomHpFill != null) bottomHpFill.fillAmount = Mathf.Lerp(bottomHpFill.fillAmount, targetHpRatio, Time.deltaTime * 5f);

		// MP 바 
		if (topMpFill != null) topMpFill.fillAmount = Mathf.Lerp(topMpFill.fillAmount, targetMpRatio, Time.deltaTime * 5f);
		if (bottomMpFill != null) bottomMpFill.fillAmount = Mathf.Lerp(bottomMpFill.fillAmount, targetMpRatio, Time.deltaTime * 5f);

		// 텍스트는 즉시 업데이트
		if (hpText != null) hpText.text = $"{currentHp} / {maxHp}";
		if (mpText != null) mpText.text = $"{(int)currentMp} / {(int)maxMp}";
	}
	public bool UseMana(float amount)
	{
		if (currentMp >= amount)
		{
			currentMp -= amount; // 마나 차감
			return true; 
		}
		else
		{
			return false; 
		}
	}
    void RegenerateMana() //마나 회복
    {
        if (currentMp >= maxMp) return; //꽉 차있으면 멈추기

        if (mpRegenTimer > 0f)
        {
            mpRegenTimer -= Time.deltaTime;
            return;
        }
        currentMp += mpRegen * Time.deltaTime;
        currentMp = Mathf.Clamp(currentMp, 0, maxMp);
    }

	// 체력 회복 (ItemSlot에서 호출)
	public void HealHP(float amount)
	{
		if (isDead) return;

		currentHp += (int)amount; 
		currentHp = Mathf.Clamp(currentHp, 0, maxHp); 

		//Debug.Log($"체력 회복 현재 HP: {currentHp}");
	}

	// 마나 회복 (ItemSlot에서 호출)
	public void HealMP(float amount)
	{
		if (isDead) return;

		currentMp += amount;
		currentMp = Mathf.Clamp(currentMp, 0, maxMp);

		//Debug.Log($"마나 회복 현재 MP: {currentMp}");
	}

	// Enemy가 호출하는 함수
	public void TakeDamage(int damage)
	{
		currentHp -= damage;
		currentHp = Mathf.Clamp(currentHp, 0, maxHp);

		Debug.Log($"HP: {currentHp}");

		if (currentHp <= 0)
		{
			Die();
		}
	}
	void Die()
	{
		if (isDead) return;
		isDead = true;

		Debug.Log("플레이어 사망");

		if (animator != null)
		{
			animator.SetTrigger("Dead");
		}
		// 사망 시 조작 불능 처리 -> 일단 해둠
		GetComponent<PlayerMovement>().enabled = false;
        Invoke(nameof(ShowGameOver), 1.5f);
    }
    void ShowGameOver()
    {
        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
    }
    void UpdateAllStatusUI()
	{
		float hpRatio = (float)currentHp / maxHp;
		if (topHpFill != null) topHpFill.fillAmount = hpRatio;
		if (bottomHpFill != null) bottomHpFill.fillAmount = hpRatio;
	}
}