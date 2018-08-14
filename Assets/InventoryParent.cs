using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryParent : MonoBehaviour {

    private static bool created = false;

    private void Awake()
    {
        if (!created)
        {
            // this is the first instance - make it persist
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            // this must be a duplicate from a scene reload - DESTROY!
            Destroy(this.gameObject);
        }
    }
}
