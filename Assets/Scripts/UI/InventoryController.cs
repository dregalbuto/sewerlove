using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    [ReadOnly] public ICollection<InventoryItem> inventoryItems;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddItem(InventoryItem item) {
        inventoryItems.Add(item);
    }
}
