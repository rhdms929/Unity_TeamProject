using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ZoneManager : MonoBehaviour //경험치 기준 해금 / 버튼 활성화 -->PlayerStatus 에서 경험치 전달
{
    [System.Serializable]
    public class ZoneData
    {
        public string zoneName;
        public int unlockExp;                  // 이 경험치 이상이면 해금
        public ZoneSpawner zoneSpawner;        // 해당 존의 스포너
        public Button zoneButton;              // UI 버튼
        public Transform movePoint;            // 이동 위치
        public TextMeshProUGUI buttonText;     // 버튼 텍스트(선택)
    }

    [Header("Zone Info")]
    public ZoneData[] zones;

    [Header("Player")]
    public Transform player;

    [Header("EXP")]
    public int currentExp = 0;

    void Start()
    {
        RefreshZones();
    }
    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log("현재 경험치: " + currentExp);
        RefreshZones();
    }
    public void RefreshZones()
    {
        for (int i = 0; i < zones.Length; i++)
        {
            bool unlocked = currentExp >= zones[i].unlockExp;

            if (zones[i].zoneSpawner != null)
                zones[i].zoneSpawner.isUnlocked = unlocked;

            if (zones[i].zoneButton != null)
                zones[i].zoneButton.interactable = unlocked;

            if (zones[i].buttonText != null)
            {
                if (unlocked)
                    zones[i].buttonText.text = zones[i].zoneName;
                else
                    zones[i].buttonText.text = zones[i].zoneName + " (잠김)";
            }
        }
    }
    public void MoveToZone(int zoneIndex)
    {
        if (zoneIndex < 0 || zoneIndex >= zones.Length)
            return;

        if (currentExp < zones[zoneIndex].unlockExp)
        {
            Debug.Log(zones[zoneIndex].zoneName + " 은 아직 잠겨있음");
            return;
        }

        if (player != null && zones[zoneIndex].movePoint != null)
        {
            player.position = zones[zoneIndex].movePoint.position;
            Debug.Log(zones[zoneIndex].zoneName + " 으로 이동");
        }
    }
}