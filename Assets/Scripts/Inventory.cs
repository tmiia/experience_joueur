using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Inventory : MonoBehaviour
{
    private static List<GameObject> listInventoryItems = new List<GameObject>();

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("inventory");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        GameObject inventory = GameObject.Find("InventoryContainer");
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            GameObject item = inventory.transform.GetChild(i).gameObject;
            listInventoryItems.Add(item);
        }
    }
    public static void DisplayInventory(string itemTag)
    {
        foreach (GameObject itemInventory in listInventoryItems)
        {
            if (itemInventory.gameObject.tag == itemTag)
            {
                itemInventory.gameObject.SetActive(true);
            }
        }
    }
    public static void MaskInventory()
    {
        foreach (GameObject itemInventory in listInventoryItems)
        {
            itemInventory.gameObject.SetActive(false);
        }
    }
}
