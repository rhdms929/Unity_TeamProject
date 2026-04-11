using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InventoryManager : MonoBehaviour //¾ÆÀÌÅÛ È¹µæÀº ÀüºÎ ÀÌ ÄÚµå·Î µé¾î¿È
{
    public static InventoryManager Instance;

    [Header("Inventory Data")]
    public List<InventoryEntry> items = new List<InventoryEntry>();

	private InventoryUI inventoryUI;
	private ActionBarSlot[] actionSlots;

	private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
			inventoryUI = FindObjectOfType<InventoryUI>();
			actionSlots = FindObjectsOfType<ActionBarSlot>(true);
		}
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemData data, int amount = 1, bool showLog = true)
    {
        if (data == null) return;

        InventoryEntry entry = items.Find(x => x.itemData == data);

        if (entry != null)
        {
            entry.count += amount;
        }
        else
        {
            items.Add(new InventoryEntry(data, amount));
        }

        if (showLog && LogManager.Instance != null)
        {
            LogManager.Instance.AddLootLog(
				$"{data.itemName} {amount}°³ È¹µæ"
			);
        }

		if (QuestManager.Instance != null)
		{

			for (int i = 0; i < amount; i++)
			{
				QuestManager.Instance.OnItemGained(data.itemName);
			}
		}
		RefreshUI();
    }

    public bool RemoveItem(ItemData data, int amount = 1)
    {
        if (data == null) return false;

        InventoryEntry entry = items.Find(x => x.itemData == data);

        if (entry == null || entry.count < amount)
            return false;

        entry.count -= amount;

        if (entry.count <= 0)
            items.Remove(entry);

        RefreshUI();
        return true;
    }

    public int GetItemCount(ItemData data)
    {
        InventoryEntry entry = items.Find(x => x.itemData == data);
        return entry != null ? entry.count : 0;
    }

	public void RefreshUI()
	{
		if (inventoryUI != null)
			inventoryUI.RefreshInventoryUI();

		if (actionSlots != null)
		{
			foreach (ActionBarSlot slot in actionSlots)
			{
				if (slot != null)
					slot.RefreshUI();
			}
		}
	}
}