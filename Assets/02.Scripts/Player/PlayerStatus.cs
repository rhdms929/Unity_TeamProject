using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatus : MonoBehaviour, IDamageable
{

    [Header("Auto Attack")]
    public AutoAttack autoAttack;

    [Header(" Status Values ")]
    public int maxHp = 100;
    private int currentHp;
    public float maxMp = 50f;
    public float currentMp;

    [Header("Power Stat")]
    public Image powerFillImage;
    public TextMeshProUGUI powerText;
    public Button powerUpButton;
    public Button powerDownButton;

    public int powerLevel = 0;
    public int maxPowerLevel = 10;
    public int damageIncrease = 2;

    [Header("Attack Delay Stat")]
    public Image attackDelayFillImage;
    public TextMeshProUGUI attackDelayText;
    public Button attackDelayUpButton;
    public Button attackDelayDownButton;

    public int attackDelayLevel = 0;
    public int maxAttackDelayLevel = 10;
    public float attackDelayDecrease = 0.2f; // 누를 때마다 줄어듦

    [Header("HP Regen Stat")]
    public Image hpRegenFillImage;
    public TextMeshProUGUI hpRegenText;
    public Button hpRegenUpButton;
    public Button hpRegenDownButton;

    public int hpRegenLevel = 0;
    public int maxHpRegenLevel = 10;
    public float hpRegen = 1f;

    [Header("MP Regen Stat")]
    public Image mpRegenFillImage;
    public TextMeshProUGUI mpRegenText;
    public Button mpRegenUpButton;
    public Button mpRegenDownButton;

    public int mpRegenLevel = 0;
    public int maxMpRegenLevel = 10;
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
        RefreshAllStatsUI();
    }

    void Update()
    {
        RegenerateMana(); //마나 회복
        RegenerateHP();
        UpdateHpMpUI();

        //   float targetHpRatio = (float)currentHp / maxHp;
        //	float targetMpRatio = currentMp / maxMp;
        //
        //	// HP 바 
        //	if (topHpFill != null) topHpFill.fillAmount = Mathf.Lerp(topHpFill.fillAmount, targetHpRatio, Time.deltaTime * 5f);
        //	if (bottomHpFill != null) bottomHpFill.fillAmount = Mathf.Lerp(bottomHpFill.fillAmount, targetHpRatio, Time.deltaTime * 5f);
        //
        //	// MP 바 
        //	if (topMpFill != null) topMpFill.fillAmount = Mathf.Lerp(topMpFill.fillAmount, targetMpRatio, Time.deltaTime * 5f);
        //	if (bottomMpFill != null) bottomMpFill.fillAmount = Mathf.Lerp(bottomMpFill.fillAmount, targetMpRatio, Time.deltaTime * 5f);
        //
        //	// 텍스트는 즉시 업데이트
        //	if (hpText != null) hpText.text = $"{currentHp} / {maxHp}";
        //	if (mpText != null) mpText.text = $"{(int)currentMp} / {(int)maxMp}";
    }

    //power
    void UpdatePowerUI()
    {
        powerText.text = autoAttack.damage.ToString();
        powerFillImage.fillAmount = (float)powerLevel / maxPowerLevel;

        powerUpButton.interactable = powerLevel < maxPowerLevel;
        powerDownButton.interactable = powerLevel > 0;
    }
    public void IncreasePower() //스탯 파워 업
    {
        if (powerLevel >= maxPowerLevel) return;

        powerLevel++;
        autoAttack.damage += damageIncrease;

        UpdatePowerUI();
    }
    public void DecreasePower()  //스탯 파워 다운
    {
        if (powerLevel <= 0) return;

        powerLevel--;
        autoAttack.damage -= damageIncrease;

        UpdatePowerUI();
    }
    //AttackDelay
    void UpdateAttackDelayUI()
    {
        attackDelayText.text = autoAttack.attackDelay.ToString("F1");
        attackDelayFillImage.fillAmount = (float)attackDelayLevel / maxAttackDelayLevel;

        attackDelayUpButton.interactable = attackDelayLevel < maxAttackDelayLevel;
        attackDelayDownButton.interactable = attackDelayLevel > 0;
    }
    public void IncreaseAttackDelay() //스탯 공격 딜레이 업
    {
        if (attackDelayLevel >= maxAttackDelayLevel) return;

        attackDelayLevel++;
        autoAttack.attackDelay -= attackDelayDecrease;

        if (autoAttack.attackDelay < 0.1f)
            autoAttack.attackDelay = 0.1f;

        UpdateAttackDelayUI();
    }
    public void DecreaseAttackDelay() //스탯 공격 딜레이 다운
    {
        if (attackDelayLevel <= 0) return;

        attackDelayLevel--;
        autoAttack.attackDelay += attackDelayDecrease;

        UpdateAttackDelayUI();
    }
    //hp
    void UpdateHPRegenUI()
    {
        hpRegenText.text = hpRegen.ToString("F1");
        hpRegenFillImage.fillAmount = (float)hpRegenLevel / maxHpRegenLevel;

        hpRegenUpButton.interactable = hpRegenLevel < maxHpRegenLevel;
        hpRegenDownButton.interactable = hpRegenLevel > 0;
    }
    public void IncreaseHPRegen() //스탯 체력 업
    {
        if (hpRegenLevel >= maxHpRegenLevel) return;

        hpRegenLevel++;
        hpRegen += 0.5f;

        UpdateHPRegenUI();
    }

    public void DecreaseHPRegen() //스탯 체력 다운
    {
        if (hpRegenLevel <= 0) return;

        hpRegenLevel--;
        hpRegen -= 0.5f;

        if (hpRegen < 0) hpRegen = 0;

        UpdateHPRegenUI();
    }
    void RegenerateHP() //체력 회복
    {
        if (currentHp >= maxHp) return;

        currentHp += (int)(hpRegen * Time.deltaTime);
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
    }
    //MP
    void UpdateMPRegenUI()
    {
        mpRegenText.text = mpRegen.ToString("F1");
        mpRegenFillImage.fillAmount = (float)mpRegenLevel / maxMpRegenLevel;

        mpRegenUpButton.interactable = mpRegenLevel < maxMpRegenLevel;
        mpRegenDownButton.interactable = mpRegenLevel > 0;
    }
    public void IncreaseMPRegen() //스탯 mp 업
    {
        if (mpRegenLevel >= maxMpRegenLevel) return;

        mpRegenLevel++;
        mpRegen += 0.5f;

        UpdateMPRegenUI();
    }
    public void DecreaseMPRegen() //스탯 mp 다운
    {
        if (mpRegenLevel <= 0) return;

        mpRegenLevel--;
        mpRegen -= 0.5f;

        if (mpRegen < 0) mpRegen = 0;

        UpdateMPRegenUI();
    }
    void RegenerateMana() //마나 회복
    {
        if (currentMp >= maxMp) return;  //꽉 차있으면 멈추기

        if (mpRegenTimer > 0f)
        {
            mpRegenTimer -= Time.deltaTime;
            return;
        }

        currentMp += mpRegen * Time.deltaTime;
        currentMp = Mathf.Clamp(currentMp, 0, maxMp);
    }
    //hp,mp 
    // Enemy가 호출하는 함수
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        if (currentHp <= 0)
            Die();
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

    //UI
    void UpdateHpMpUI()
    {
        float hpRatio = (float)currentHp / maxHp;
        float mpRatio = currentMp / maxMp;

        topHpFill.fillAmount = Mathf.Lerp(topHpFill.fillAmount, hpRatio, Time.deltaTime * 5f);
        bottomHpFill.fillAmount = Mathf.Lerp(bottomHpFill.fillAmount, hpRatio, Time.deltaTime * 5f);

        topMpFill.fillAmount = Mathf.Lerp(topMpFill.fillAmount, mpRatio, Time.deltaTime * 5f);
        bottomMpFill.fillAmount = Mathf.Lerp(bottomMpFill.fillAmount, mpRatio, Time.deltaTime * 5f);

        hpText.text = $"{currentHp} / {maxHp}";
        mpText.text = $"{(int)currentMp} / {(int)maxMp}";
    }

    void UpdateAllStatusUI()
    {
        topHpFill.fillAmount = (float)currentHp / maxHp;
        bottomHpFill.fillAmount = (float)currentHp / maxHp;

        topMpFill.fillAmount = currentMp / maxMp;
        bottomMpFill.fillAmount = currentMp / maxMp;
    }

    void RefreshAllStatsUI()
    {
        UpdatePowerUI();
        UpdateAttackDelayUI();
        UpdateHPRegenUI();
        UpdateMPRegenUI();
    }

    //(SkillSlot 에서 호출)
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
}