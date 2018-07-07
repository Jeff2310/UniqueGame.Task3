using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName ="New Basic Item",menuName ="Item/Basic",order =0)]
public class ItemBase : SerializedScriptableObject {

    public enum ItemType { Basic, Consumable }
    [ReadOnly]
    public ItemType type;

    public string itemName = "New Item";

    [Multiline(4)]
    public string description;

    [Range(1,999)]
    public int maxStack=1;

    
    
    [PreviewField]
    public Sprite icon;

    public bool interactable = true;


    private void Awake()
    {
        type = ItemType.Basic;
    }
}

/*
[CustomEditor(typeof(ItemBase))]
public class ItemBaseEditor:Editor
{
    
    private ItemBase itemBase
    {
        get
        {
            return (ItemBase)target;
        }
    }

    public override void OnInspectorGUI()
    {
        GUIStyle typeStyle = GUIStyle.none;
        typeStyle.fontStyle = FontStyle.Bold;
        typeStyle.fontSize = 16;
        
        EditorGUILayout.LabelField("Item Type",itemBase.type.ToString(),typeStyle);
        base.OnInspectorGUI();
        itemBase.icon = (Sprite)EditorGUILayout.ObjectField("Icon", itemBase.icon, typeof(Sprite), false, null);
    }
    
}*/
