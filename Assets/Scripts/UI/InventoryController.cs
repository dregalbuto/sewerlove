using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private List<InventoryItem> inventoryItems;
    private static bool created = false;

    private void Awake()
    {
        if (!created)
        {
            // this is the first instance - make it persist
            DontDestroyOnLoad(this.gameObject);
            created = true;
            inventoryItems = new List<InventoryItem>();
        }
        else
        {
            // this must be a duplicate from a scene reload - DESTROY!
            Destroy(this.gameObject);
        }
    }


    public void AddItem(GameObject item)
    {
        inventoryItems.Add(new InventoryItem(item.name, item.GetComponent<Image>()));
    }

    public List<InventoryItem> GetInventoryItems()
    {
        return inventoryItems;
    }
}
