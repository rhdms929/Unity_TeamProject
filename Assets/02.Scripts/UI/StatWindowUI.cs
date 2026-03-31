using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatWindowUI : MonoBehaviour
{
    public PlayerStats playerStats;

    [Header("Texts")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI mpText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackDelayText;
    public TextMeshProUGUI hpRegenText;
    public TextMeshProUGUI mpRegenText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI statPointText;

    void Start()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (playerStats == null) return;

        hpText.text = playerStats.GetCurrentHP().ToString();
        mpText.text = playerStats.GetCurrentMP().ToString();
        damageText.text = playerStats.GetBonusDamage().ToString();
        attackDelayText.text = playerStats.autoAttack.attackDelay.ToString("F1");
        hpRegenText.text = playerStats.hpRegen.ToString("F1");
        mpRegenText.text = playerStats.mpRegen.ToString("F1");
        speedText.text = playerStats.GetSpeed().ToString("F1");
        levelText.text = playerStats.level.ToString();
        statPointText.text = playerStats.statPoint.ToString();
    }
}