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
		[TextArea]
		public string zoneDescription; 
		public Sprite[] monsterImages;
		public string[] monsterNames;
	}

	[System.Serializable]
	public class MonsterSlot
	{
		public Image image;
		public TextMeshProUGUI nameText;
	}

	[Header("Zone Info")]
    public ZoneData[] zones;

	[Header("Zone Info Panel")]
	public GameObject zonePanel;
	public MonsterSlot[] monsterSlots;
	public TextMeshProUGUI infoDescText;

	[Header("Player")]
    public Transform player;

    private int currentExp = 0;

	void Start()
    {
		if (zonePanel != null) zonePanel.SetActive(false);
		RefreshZones();
    }

    // 외부에서 현재 경험치를 넘겨주면 그 값 기준으로 해금 갱신
    public void SetExp(int exp)
    {
        currentExp = exp;
        Debug.Log("ZoneManager 받은 경험치: " + currentExp);
        RefreshZones();
    }
    void RefreshZones()
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
                    zones[i].buttonText.text = zones[i].zoneName + "\n(Locked)";
            }
        }
    }
    public void MoveToZone(int zoneIndex)
    {
        if (zoneIndex < 0 || zoneIndex >= zones.Length)
            return;

        if (currentExp < zones[zoneIndex].unlockExp)
            return;

        if (player != null && zones[zoneIndex].movePoint != null)
        {
            player.position = zones[zoneIndex].movePoint.position;
        }

    }

	public void OnHoverZone(int zoneIndex)
	{
		if (zoneIndex < 0 || zoneIndex >= zones.Length) return;

		zonePanel.SetActive(true);
		ZoneData zone = zones[zoneIndex];

		for (int i = 0; i < monsterSlots.Length; i++)
		{
			if (i < zone.monsterImages.Length && zone.monsterImages[i] != null)
			{
				monsterSlots[i].image.gameObject.SetActive(true);
				monsterSlots[i].image.sprite = zone.monsterImages[i];
				monsterSlots[i].nameText.gameObject.SetActive(true);
				monsterSlots[i].nameText.text = zone.monsterNames[i];
			}
			else
			{
				monsterSlots[i].image.gameObject.SetActive(false);
				monsterSlots[i].nameText.gameObject.SetActive(false);
			}
		}

		if (infoDescText != null) infoDescText.text = zone.zoneDescription;
	}
}