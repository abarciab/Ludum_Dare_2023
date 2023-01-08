using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogCoordinator : MonoBehaviour
{
    public void BuyItem(ItemObject item)
    {
        if (GameManager.instance.inventory.money < item.inrgedientCost) return;
        GameManager.instance.LoseMoney(item.inrgedientCost);
        GameManager.instance.DeliverItem(item);
        gameObject.SetActive(false);
    }
}
