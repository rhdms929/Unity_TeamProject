using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public double cost; // 초기 업그레이드 가격
    public double goldIncrease = 1; // 업그레이드 시 골드 증가량
    public TextMeshProUGUI costText; // 가격을 표시할 텍스트

    void Start()
    {
        UpdateUI();
	}
    
    public void BuyUpgrade()
    {
        if(GameManager.instance.currentGold >= cost)
        {
            GameManager.instance.currentGold -= cost;   //  돈 지불
            GameManager.instance.goldPerSecond += goldIncrease;     //  1초당 골드 획등량 증가
            cost *= 1.5;    //  가격상승 (1.5배로 해놨음)
            UpdateUI();
		}
        else
        {
            Debug.Log("돈 부족");
        }
    }

    void UpdateUI()
    {
        if(costText != null)
        {
			costText.text = "Upgrade\n(Cost : " + cost.ToString("F0") + ")";
		}
    }
}
