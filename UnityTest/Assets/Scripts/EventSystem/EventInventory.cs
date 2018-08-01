using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInventory : EventBase
{
    public ItemBase item;
    public enum ItemChangeType { Gain, Lose }
    public ItemChangeType itemChangeType;
    public int amount = 1;
    public bool notice = true;
}