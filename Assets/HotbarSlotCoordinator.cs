using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotCoordinator : MonoBehaviour
{
    [SerializeField] GameObject selected;
    [SerializeField] int index;
    InventoryScript inv;
    ItemObject item;
    [SerializeField] Image itemImg;

    private void Start()
    {
        inv = GameManager.instance.inventory;
        index = transform.GetSiblingIndex();
        InventoryScript.OnHotbarUpdate += UpdateDisplay;
    }

    void UpdateDisplay()
    {
        if (inv.hotbarIndex == index) selected.SetActive(true);
        else selected.SetActive(false);
        
        DisplayItem();
    }

    void DisplayItem()
    {
        item = inv.GetItemInHotbarSlot(index);
        if (selected.activeInHierarchy) inv.selectedItem = item;
        if (item == null) {
            itemImg.sprite = null;
            itemImg.enabled = false;
            return;
        }
        itemImg.sprite = item.itemSprite;
        itemImg.enabled = true;
    }

}
