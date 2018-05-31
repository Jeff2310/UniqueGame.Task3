using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "New Basic Item", menuName = "Item/Comsuable", order = 1)]
public class ItemConsumable : ItemBase {

    private void Awake()
    {
        type = ItemType.Consumable;
    }

    public void Use()
    {
        Debug.Log("Using " + itemName);
    }
}

[CustomEditor(typeof(ItemConsumable))]
public class ItemConsuamleEditor : ItemBaseEditor
{
    private ItemConsumable itemConsumable
    {
        get
        {
            return (ItemConsumable)target;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //itemConsumable.icon = (Sprite)EditorGUILayout.ObjectField("Icon", itemBase.icon, typeof(Sprite), false, null);
    }
}
