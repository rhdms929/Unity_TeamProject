using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatWindowUI : MonoBehaviour
{
    public PlayerStats playerStats;

    [Header("Texts")]
    public TextMeshProUGUI hpValue;
    public TextMeshProUGUI mpValue;
    public TextMeshProUGUI damageValue;
    public TextMeshProUGUI atkSpeedValue;
    public TextMeshProUGUI hpRegenValue;
    public TextMeshProUGUI mpRegenValue;
    public TextMeshProUGUI speedValue;
    public TextMeshProUGUI levelValue;
    public TextMeshProUGUI statPointValue;

    void Start()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (playerStats == null) return;

        hpValue.text = playerStats.GetCurrentHP().ToString();
        mpValue.text = playerStats.GetCurrentMP().ToString();
        damageValue.text = playerStats.GetBonusDamage().ToString();
        atkSpeedValue.text = playerStats.autoAttack.attackDelay.ToString("F1");
        hpRegenValue.text = playerStats.hpRegen.ToString("F1");
        mpRegenValue.text = playerStats.mpRegen.ToString("F1");
        speedValue.text = playerStats.GetSpeed().ToString("F1");
        levelValue.text = playerStats.level.ToString();
        statPointValue.text = playerStats.statPoint.ToString();
    }
}