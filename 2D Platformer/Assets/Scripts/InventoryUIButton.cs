using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIButton : MonoBehaviour
{
    [SerializeField] private Text label;

    [SerializeField] private Text amount;

    [SerializeField] private Image image;

    [SerializeField] private List<Sprite> sprites;
        
    private InventoryUsedCallback callback;

    public InventoryUsedCallback Callback
    {
        get => callback;
        set => callback = value;
    }

    private InventoryItem itemData;

    public InventoryItem ItemData
    {
        get => itemData;
        set => itemData = value;
    }

    private void Start()
    {
        label.text = itemData.CrystallType.ToString();
        amount.text = itemData.Quantity.ToString();

        string spriteNameToSearch = itemData.CrystallType.ToString().ToLower();

        image.sprite = sprites.Find(x => x.name.Contains(spriteNameToSearch));
        
        gameObject.GetComponent<Button>().onClick.AddListener(() => callback(this));
    }
}
