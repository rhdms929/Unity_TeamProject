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

            if (data.itemType == ItemData.ItemType.HP_Potion ||
                data.itemType == ItemData.ItemType.HP_Potion_Big)
            {
                text += "\n\n+ " + data.healAmount.ToString("F0") + " HP";
            }
            else if (data.itemType == ItemData.ItemType.MP_Potion ||
                     data.itemType == ItemData.ItemType.MP_Potion_Big)
            {
                text += "\n\n+ " + data.healAmount.ToString("F0") + " MP";
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