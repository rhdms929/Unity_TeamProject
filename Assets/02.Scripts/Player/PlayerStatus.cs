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

	[Header(" UI References ")]
	public Image topHpFill;
	public Image bottomHpFill;
	public Image topMpFill;
	public Image bottomMpFill;
	public TextMeshProUGUI hpText;
	public TextMeshProUGUI mpText;

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
	}

	void UpdateAllStatusUI()
	{
		float hpRatio = (float)currentHp / maxHp;
		if (topHpFill != null) topHpFill.fillAmount = hpRatio;
		if (bottomHpFill != null) bottomHpFill.fillAmount = hpRatio;
	}
}