using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip instance;

    [Header("UI")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    private void Awake()
    {
        instance = this;
        Clear();
    }

    public void Show(ItemData data)
    {
        if (data == null)
        {
            Clear();
            return;
        }

        if (titleText != null)
            titleText.text = data.itemName;

        if (descriptionText != null)
        {
            string text = data.itemDescription;

            if (data.itemCategory == ItemCategory.Potion)
            {
                if (data.potionType == PotionType.HealHP)
                {
                    text += "\n\n+ " + data.effectValue.ToString("F0") + " HP";
                }
                else if (data.potionType == PotionType.HealMP)
                {
                    text += "\n\n+ " + data.effectValue.ToString("F0") + " MP";
                }
            }
            else if (data.itemCategory == ItemCategory.Equipment)
            {
                //text += "\n[장비 정보]";
                text += "\n\n공격력 +" + data.bonusAttack;
                text += "\n방어력 +" + data.bonusDefense;
                text += "\n최대 HP +" + data.bonusHp;
                text += "\n최대 MP +" + data.bonusMp;
            }

            descriptionText.text = text;
        }
    }

    public void Clear()
    {
        if (titleText != null)
            titleText.text = "아이템 이름";

        if (descriptionText != null)
            descriptionText.text = "아이템에 마우스를 올려 확인하세요";
    }
}
