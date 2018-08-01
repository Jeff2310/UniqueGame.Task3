using UnityEngine;
using UnityEditor;

public class ConditionInventory : ConditionBase
{
    public ItemBase Item;
    public int MinimumCount;
    public int MaximumCount;

    public override bool Check()
    {
        var item = GameInventory.Instance.FindItem(Item);
        if(item == null)
        {
            return MaximumCount < 0;
        }
        return item.amount >= MinimumCount && (MaximumCount <0 ? true : item.amount <= MaximumCount);
    }
}