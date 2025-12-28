using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{
    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount = 10;

    private ItemDictionary itemDictionary;

    private Key[] hotbarsKeys;

    // Start is called before the first frame update
    void Start()
    {
        hotbarsKeys = new Key[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            hotbarsKeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotbarsKeys[i]].wasPressedThisFrame)
            {
                UseItemInSlot(i);
            }
        }
    }

    void UseItemInSlot(int index)
    {
        Slot slot = hotbarPanel.transform.GetChild(index).GetComponent<Slot>();
        if (slot.currentItem != null) 
        {
            Item item = slot.currentItem.GetComponent<Item>();
            item.useItem();
        }
    }

    public List<InventorySavaData> GetHotbarItems()
    {

        List<InventorySavaData> hotbarData = new List<InventorySavaData>();
        foreach (Transform slotTransform in hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                hotbarData.Add(new InventorySavaData { itemId = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }
        return hotbarData;

    }

    public void SetHotbarItems(List<InventorySavaData> inventorySavaData)
    {
        foreach (Transform child in hotbarPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, hotbarPanel.transform);
        }

        foreach (InventorySavaData data in inventorySavaData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = hotbarPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemId);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }

        }
    }
}
