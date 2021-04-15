using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    [SerializeField]
    private CrystallType crystallType;

    public CrystallType CrystallType
    {
        get => crystallType;
        set => crystallType = value;
    }

    [SerializeField]
    private int quantity;

    public int Quantity
    {
        get => quantity;
        set => quantity = value;
    }
}
