using UnityEngine.UI;

public class InventoryItem {

    public string name;
    public Image image;

    public InventoryItem(string name, Image image)
    {
        this.name = name;
        this.image = image;
    }
}
