using System;
using UnityEngine;

public class ChestEquipment : MonoBehaviour
{
    public enum ChestType
    {
        None,
        Key,
        Code
    };

    public InventoryItem[] chestItems;

    [SerializeField]
    private ChestType chestType;

    public GameObject key;

    public int[] code;

    public string hint;

    public bool isUnlocked = false;

    public bool isChestOpened = false;

    public AudioClip openingSound;
    public AudioClip closingSound;
    public AudioClip invalidKeySound;
    public AudioClip correctKeySound;

    private void Start()
    {
        if (chestItems.Length != 5)
            chestItems = new InventoryItem[5];
    }

    public void addItem(InventoryItem item, int index)
    {
        chestItems[index] = item;
    }

    public InventoryItem[] getChestItems()
    {
        Array.Resize(ref chestItems, 5);
        return chestItems;
    }
    public ChestType getChestType()
    {
        return chestType;
    }
}
