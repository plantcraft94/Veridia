using System.Collections.Generic;
using UnityEngine;

public class PlayerCommonInventory : MonoBehaviour
{
    private Dictionary<Item, int> items = new Dictionary<Item, int>();

    public bool AddItem(Item itemType, int amount)
    {
        if (items.ContainsKey(itemType))
            items[itemType] += amount;
        else
            items[itemType] = amount;

        Debug.Log("Picked up: " + itemType + " x" + amount);
        return true;
    }

    public int GetItemAmount(Item itemType)
    {
        items.TryGetValue(itemType, out int value);
        return value;
    }
}
