using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InventoryManager : MonoBehaviour //¾ÆÀÌÅÛ È¹µæÀº ÀüºÎ ÀÌ ÄÚµå·Î µé¾î¿È
{
    public static InventoryManager Instance;

    [Header("Inventory Data")]
    public List<InventoryEntry> items = new List<InventoryEntry>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(gameObject);
    }

    public void AddItem(ItemData data, int amount = 1, bool showLog = true)
    {
        if (data == null)
        {
            return;
        }

        InventoryEntry entry = FindEntry(data);

        if (entry != null)
        {
            entry.count += amount;
        }
        else
        {
            items.Add(new InventoryEntry(data, amount));
        }
        AddLootLog(data, amount, showLog);
        NotifyQuestItemGained(data, amount);
        RefreshUI();
    }

    public bool RemoveItem(ItemData data, int amount = 1)
    {
        if (data == null)
        {
            return false;
        }

        InventoryEntry entry = FindEntry(data);

        if (entry == null)
        {
            return false;
        }

        if (entry.count < amount)
        {
            return false;
        }

        entry.count -= amount;

        if (entry.count <= 0)
        {
            items.Remove(entry);
        }

        RefreshUI();
        return true;
    }

    public int GetItemCount(ItemData data)
    {
        InventoryEntry entry = FindEntry(data);

        if (entry == null)
        {
            return 0;
        }

        return entry.count;
    }

    public void RefreshUI()
    {
        RefreshInventoryWindow();
        RefreshActionBarSlots();
    }

    private InventoryEntry FindEntry(ItemData data)
    {
        return items.Find(x => x.itemData == data);
    }

    private void AddLootLog(ItemData data, int amount, bool showLog)
    {
        if (!showLog)
        {
            return;
        }
        if (LogManager.Instance == null)
        {
            return;
        }
        LogManager.Instance.AddLootLog($"{data.itemName} {amount}°³ È¹µæ");
    }

    private void NotifyQuestItemGained(ItemData data, int amount)
    {
        if (QuestManager.Instance == null)
        {
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            QuestManager.Instance.OnItemGained(data.itemName);
        }
    }

    private void RefreshInventoryWindow()
    {
        InventoryUI ui = FindObjectOfType<InventoryUI>();

        if (ui != null)
        {
            ui.RefreshInventoryUI();
        }
    }

    private void RefreshActionBarSlots()
    {
        ActionBarSlot[] slots = FindObjectsOfType<ActionBarSlot>(true);

        foreach (ActionBarSlot slot in slots)
        {
            if (slot != null)
            {
                slot.RefreshUI();
            }
        }
    }
}