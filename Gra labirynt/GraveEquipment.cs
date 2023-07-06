using System;
using UnityEngine;

public class GraveEquipment : MonoBehaviour
{
    public InventoryItem[] graveItems;

    public void addItem(InventoryItem item, int index)
    {
        graveItems[index] = item;
    }

    public InventoryItem[] getGraveItems()
    {
        Array.Resize(ref graveItems, 5);
        return graveItems;
    }

    public void clearEquipment()
    {
        for(int i = 0; i < graveItems.Length; i++)
        {
            graveItems[i] = null;
        }     
    }
}
