using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController; 

    // Start is called before the first frame update
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindObjectOfType<InventoryController>();
        hotbarController = FindObjectOfType<HotbarController>();

        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData()
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name,
            inventorySavaData = inventoryController.GetInventoryItems(),
            hotbarSavaData = hotbarController.GetHotbarItems()
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if(File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();

            inventoryController.SetInventoryItems(saveData.inventorySavaData);
            hotbarController.SetHotbarItems(saveData.hotbarSavaData);


        }
        else
        {
            SaveGame();
        }

    }
}
