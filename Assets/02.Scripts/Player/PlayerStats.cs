using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("Auto Attack")] //attack delay 스탯 때문에 필요함
    public AutoAttack autoAttack;

    [Header("Movement")] // speed 스탯 때문에 필요함
    public PlayerMovement playerMovement;

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

    [Header("Speed Stat")]
    public Image speedFillImage;
    public TextMeshProUGUI speedText;
    public Button speedUpButton;
    public Button speedDownButton;

    public int speedLevel = 0;
    public int maxSpeedLevel = 10;
    public float speedIncrease = 0.5f;

    [Header("Level / EXP")] //경험치 
    public int level = 1;                     //현재 레벨
    public float currentExp = 0f;             //현재 경험치
    public float maxExp = 100f;               //다음 레벨업까지 필요한 경험치
    public int statPoint = 0;                 //스탯 찍을 수 있는 포인트
    public float expGrowthMultiplier = 1.2f;  //레벨업 할 때 필요 경험치 증가 비율

    [Header("EXP UI")] //경험치 UI 연결
    public Image expFillImage;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI statPointText;

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

    [Header("Manager")]
    private ZoneManager zoneManager;
    private StatWindowUI statWindowUI;

    void Awake()
    {
        currentHp = maxHp;
        currentMp = maxMp;
    }

    void Start()
    {
        zoneManager = FindObjectOfType<ZoneManager>();
        statWindowUI = FindObjectOfType<StatWindowUI>();
        UpdateAllStatusUI(); // 시작할 때 UI 초기화
        RefreshAllStatsUI();
        UpdateExpUI();

        if (zoneManager != null)
        {
            zoneManager.SetExp(level);
        }

        RefreshStatWindowUI();
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

        powerUpButton.interactable = powerLevel < maxPowerLevel && statPoint > 0;
        powerDownButton.interactable = powerLevel > 0;
    }
    public void IncreasePower() //스탯 파워 업
    {
        if (powerLevel >= maxPowerLevel) return;
        if (!CanUseStatPoint()) return;

        powerLevel++;
        statPoint--;
        autoAttack.damage += damageIncrease;

        UpdatePowerUI();
        UpdateExpUI();
        RefreshAllStatsUI();
    }
    public void DecreasePower()  //스탯 파워 다운
    {
        if (powerLevel <= 0) return;

        powerLevel--;
        statPoint++;
        autoAttack.damage -= damageIncrease;

        UpdatePowerUI();
        UpdateExpUI();
        RefreshAllStatsUI();
    }
    //AttackDelay
    void UpdateAttackDelayUI()
    {
        attackDelayText.text = autoAttack.attackDelay.ToString("F1");
        attackDelayFillImage.fillAmount = (float)attackDelayLevel / maxAttackDelayLevel;

        attackDelayUpButton.interactable = attackDelayLevel < maxAttackDelayLevel && statPoint > 0;
        attackDelayDownButton.interactable = attackDelayLevel > 0;
    }
    public void IncreaseAttackDelay() //스탯 공격 딜레이 업
    {
        if (attackDelayLevel >= maxAttackDelayLevel) return;
        if (!CanUseStatPoint()) return;

        attackDelayLevel++;
        statPoint--;
        autoAttack.attackDelay -= attackDelayDecrease;

        if (autoAttack.attackDelay < 0.1f)
            autoAttack.attackDelay = 0.1f;

        UpdateAttackDelayUI();
        UpdateExpUI();
        RefreshAllStatsUI();
    }
    public void DecreaseAttackDelay() //스탯 공격 딜레이 다운
    {
        if (attackDelayLevel <= 0) return;

        attackDelayLevel--;
        statPoint++;
        autoAttack.attackDelay += attackDelayDecrease;

        UpdateAttackDelayUI();
        UpdateExpUI();
        RefreshAllStatsUI();
    }
    //hp
    void UpdateHPRegenUI()
    {
        hpRegenText.text = hpRegen.ToString("F1");
        hpRegenFillImage.fillAmount = (float)hpRegenLevel / maxHpRegenLevel;

        hpRegenUpButton.interactable = hpRegenLevel < maxHpRegenLevel && statPoint > 0;
        hpRegenDownButton.interactable = hpRegenLevel > 0;
    }
    public void IncreaseHPRegen() //스탯 체력 업
    {
        if (hpRegenLevel >= maxHpRegenLevel) return;
        if (!CanUseStatPoint()) return;

        hpRegenLevel++;
        statPoint--;
        hpRegen += 0.5f;

        UpdateHPRegenUI();
        UpdateExpUI();
        RefreshAllStatsUI();
    }

    public void DecreaseHPRegen() //스탯 체력 다운
    {
        if (hpRegenLevel <= 0) return;

        hpRegenLevel--;
        statPoint++;
        hpRegen -= 0.5f;

        if (hpRegen < 0) hpRegen = 0;

        UpdateHPRegenUI();
        UpdateExpUI();
        RefreshAllStatsUI();
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

        mpRegenUpButton.interactable = mpRegenLevel < maxMpRegenLevel && statPoint > 0;
        mpRegenDownButton.interactable = mpRegenLevel > 0;
    }
    public void IncreaseMPRegen() //스탯 mp 업
    {
        if (mpRegenLevel >= maxMpRegenLevel) return;
        if (!CanUseStatPoint()) return;

        mpRegenLevel++;
        statPoint--;
        mpRegen += 0.5f;

        UpdateMPRegenUI();
        UpdateExpUI();
        RefreshAllStatsUI();
    }
    public void DecreaseMPRegen() //스탯 mp 다운
    {
        if (mpRegenLevel <= 0) return;

        mpRegenLevel--;
        statPoint++;
        mpRegen -= 0.5f;

        if (mpRegen < 0) mpRegen = 0;

        UpdateMPRegenUI();
        UpdateExpUI();
        RefreshAllStatsUI();
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
    //Speed
    void UpdateSpeedUI()
    {
        speedText.text = playerMovement.speed.ToString("F1");
        speedFillImage.fillAmount = (float)speedLevel / maxSpeedLevel;

        speedUpButton.interactable = speedLevel < maxSpeedLevel && statPoint > 0;
        speedDownButton.interactable = speedLevel > 0;
    }

    public void IncreaseSpeed()
    {
        if (speedLevel >= maxSpeedLevel) return;
        if (!CanUseStatPoint()) return;

        speedLevel++;
        statPoint--;
        playerMovement.speed += speedIncrease;

        UpdateSpeedUI();
        UpdateExpUI();
        RefreshAllStatsUI();
    }

    public void DecreaseSpeed()
    {
        if (speedLevel <= 0) return;

        speedLevel--;
        statPoint++;
        playerMovement.speed -= speedIncrease;

        if (playerMovement.speed < 0.5f)
            playerMovement.speed = 0.5f;

        UpdateSpeedUI();
        UpdateExpUI();
        RefreshAllStatsUI();
    }
    //hp,mp 
    // Enemy가 호출하는 함수
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        RefreshStatWindowUI();//stat 창
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
		FindObjectOfType<TimeManager>().StopTimer();
	}
    void ShowGameOver()
    {
        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
    }
    //경험치 획득 함수
    public void GainExp(float amount)
    {
        currentExp += amount;

		if (QuestManager.Instance != null)
			QuestManager.Instance.OnEXPGained((int)amount);

		while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            LevelUp();
        }

        if (zoneManager != null)
        {
            zoneManager.SetExp(level);
        }

        RefreshAllStatsUI();
    }
    //레벨업 함수 추가
    void LevelUp()
    {
        int prevLevel = level;
        level++;                        //레벨 1 증가
        statPoint++;                    //스탯포인트 1 증가
        maxExp *= expGrowthMultiplier;  //다음 레벨업에 필요한 경험치 증가

        Debug.Log("레벨업! 현재 레벨: " + level);

        uiManager.logManager.AddActivityLog($"플레이어 레벨 업! {prevLevel} -> {level}");
    }
    //경험치 UI 갱신 함수 추가
    void UpdateExpUI()
    {
        if (expFillImage != null)
        {
            expFillImage.fillAmount = currentExp / maxExp; //경험치 바 채움 비율 바꿈
        }
        if (expText != null)
        {
            expText.text = $"{(int)currentExp} / {(int)maxExp}"; //경험치 숫자 보여줌
        }
        if (levelText != null)
        {
            levelText.text = $"Lv. {level}"; //레벨 표시
        }
        if (statPointText != null)
        {
            statPointText.text = $"SP : {statPoint}"; //스탯 포인트 표시
        }
    }
    bool CanUseStatPoint() //일단 스탯 포인트 있어야만 스탯 올리게 했습ㄷ니다(상의 후 수정 예정) / 골드로 사게 할 수 있게??
    {
        return statPoint > 0;
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
        UpdateSpeedUI();
        UpdateExpUI();
        RefreshStatWindowUI();//stat 창
    }

    //(SkillSlot 에서 호출)
    public bool UseMana(float amount)
    {
        if (currentMp >= amount)
        {
            currentMp -= amount; // 마나 차감
            RefreshStatWindowUI();//stat 창
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
        RefreshStatWindowUI();//stat 창
    }

    // 마나 회복 (ItemSlot에서 호출)
    public void HealMP(float amount)
    {
        if (isDead) return;

        currentMp += amount;
        currentMp = Mathf.Clamp(currentMp, 0, maxMp);

        //Debug.Log($"마나 회복 현재 MP: {currentMp}");
        RefreshStatWindowUI();//stat 창
    }
    //현재 내 stat 정보 불러오기 위해  (StatWindowUI 에서 씀)
    void RefreshStatWindowUI()
    {
        if (statWindowUI != null)
        {
            statWindowUI.RefreshUI();
        }
    }
    public int GetCurrentHP()
    {
        return currentHp;
    }

    public int GetCurrentMP()
    {
        return (int)currentMp;
    }

    public int GetBonusDamage()
    {
        return autoAttack != null ? autoAttack.damage : 0;
    }

    public float GetRegen()
    {
        return hpRegen;
    }

    public float GetSpeed()
    {
        return playerMovement != null ? playerMovement.speed : 0f;
    }
    //(StatWindowUI 에서 씀)
}