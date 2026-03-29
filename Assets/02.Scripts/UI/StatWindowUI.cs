using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatWindowUI : MonoBehaviour
{
    public PlayerStats playerStats;

    [Header("Stat Values")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI mpText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI statPointText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI regenText;
    public TextMeshProUGUI speedText;

    void Start()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (playerStats == null) return;

        if (hpText != null)
            hpText.text = playerStats.GetCurrentHP().ToString();

        if (mpText != null)
            mpText.text = playerStats.GetCurrentMP().ToString();

        if (levelText != null)
            levelText.text = playerStats.level.ToString();

        if (statPointText != null)
            statPointText.text = playerStats.statPoint.ToString();

        if (damageText != null)
            damageText.text = playerStats.GetBonusDamage().ToString();

        if (regenText != null)
            regenText.text = playerStats.GetRegen().ToString("F1");

        if (speedText != null)
            speedText.text = playerStats.GetSpeed().ToString("F1");
    }
}