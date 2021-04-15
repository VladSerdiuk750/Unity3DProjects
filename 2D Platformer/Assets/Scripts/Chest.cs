using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrystallType
{
    Random,
    Red, 
    Green,
    Blue
}

public class Chest : MonoBehaviour
{
    public InventoryItem itemData = new InventoryItem();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Knight knight = other.gameObject.GetComponent<Knight>();

        if (itemData.CrystallType == CrystallType.Random)
        {
            itemData.CrystallType = (CrystallType) Random.Range(1, 4);
        }

        if (itemData.Quantity == 0)
        {
            itemData.Quantity = Random.Range(1, 6);
        }

        GameController.Instance.AddInventoryItem(itemData);
        
        if (knight != null)
        {
            Destroy(gameObject);
        }
    }
}
