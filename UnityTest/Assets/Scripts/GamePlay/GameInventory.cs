using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInventory : MonoBehaviour {
    public class Item
    {
        public ItemBase itemBase;
        public int amount;

        public Item(ItemBase itemBase,int amount)
        {
            this.itemBase = itemBase;
            this.amount = amount;
        }
    }

    public int capacity;
    public List<Item> items = new List<Item>();

    public Item GainItem(ItemBase itemBase,int amount = 1)
    {
        //Check for parameters
        if (itemBase==null||amount<=0)
        {
            return null;
        }

        //If the item has already existed
        foreach (var i in items)
        {
            if (i.itemBase == itemBase)
            {
                i.amount += amount;
                return i;
            }
        }

        //If it's a new item
        Item newItem = new Item(itemBase, amount);
        items.Add(newItem);
        return newItem;
    }

    public Item LoseItem(ItemBase itemBase,int amount = 1)
    {
        if (itemBase == null || amount <= 0)
        {
            return null;
        }

        foreach (var i in items)
        {
            if (i.itemBase == itemBase)
            {
                if (i.amount>amount)
                {
                    i.amount -= amount;
                }
                else
                {
                    items.Remove(i);
                    i.amount = 0;
                }
                return i;
            }
        }

        Debug.LogWarning("The Item to lose was not found!!");
        return null;
    }

}
