using UnityEngine;

public class InventoryItem
{
    public ItensData data;
    public int count;
    public bool isEquipped;

    public InventoryItem(ItensData itemData, int initialCount = 1)
    {
        data = itemData;
        count = initialCount;
        isEquipped = false;
    }
}
