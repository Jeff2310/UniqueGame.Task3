using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Item/Comsuable", order = 1)]
public class ItemConsumable : ItemBase {

    private void Awake()
    {
        type = ItemType.Consumable;
    }

    public void Use()
    {
        Debug.Log("Using " + name);
    }
}

/*
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
    
}*/
