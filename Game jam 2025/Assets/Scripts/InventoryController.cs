using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    // Start is called before the first frame update
    void Start()
    {

        itemDictionary = FindObjectOfType<ItemDictionary>();

        //for(int i = 0; i < slotCount; i++)
        //{
        //    Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
        //    if(i < itemPrefabs.Length)
        //    {
        //        GameObject item = Instantiate(itemPrefabs[i], slot.transform);
        //        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //        slot.currentItem = item;

        //    }
        //}
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }

        Debug.Log("Inventory is full");
        return false;
    }
    
    public List<InventorySavaData> GetInventoryItems()
    {

        List<InventorySavaData> invData = new List<InventorySavaData>();
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent< Slot>();
            if(slot.currentItem != null)
            {
                Item item =slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySavaData { itemId = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }
        return invData;

    }

    public void SetInventoryItems(List<InventorySavaData> inventorySavaData)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        foreach (InventorySavaData data in inventorySavaData)
        {
            if(data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent< Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemId);
                if(itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }

        }
    }
}
