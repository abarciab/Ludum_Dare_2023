using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatalogEntryCoordinator : MonoBehaviour
{
    [SerializeField] ItemObject item;
    [SerializeField] TextMeshProUGUI title, price;
    [SerializeField] Image icon;

    private void Start()
    {
        icon.sprite = item.itemSprite;
        title.text = item.name.ToUpper();
        price.text = "$ " + item.inrgedientCost;
    }

    public void OrderItem()
    {
        FindObjectOfType<CatalogCoordinator>().BuyItem(item);
    }
}
