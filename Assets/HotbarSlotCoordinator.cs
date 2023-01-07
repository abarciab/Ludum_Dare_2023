using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarSlotCoordinator : MonoBehaviour
{
    [SerializeField] GameObject selected, itemCountParent;
    [SerializeField] int index;
    InventoryScript inv;
    ItemObject item;
    int count;
    [SerializeField] public Image itemImg;
    [SerializeField] TextMeshProUGUI itemCount;

    private void Start()
    {
        inv = GameManager.instance.inventory;
        index = transform.GetSiblingIndex();
        InventoryScript.OnHotbarUpdate += UpdateDisplay;
        DisplayItem();
    }

    void UpdateDisplay()
    {
        if (inv.hotbarIndex == index) selected.SetActive(true);
        else selected.SetActive(false);
        
        DisplayItem();
    }

    void DisplayItem()
    {
        var slot = inv.GetItemInHotbarSlot(index);
        if (slot != null) item = slot.item;
        if (selected.activeInHierarchy) inv.selectedItem = item;
        if (slot == null) {
            itemImg.sprite = null;
            itemImg.enabled = false;
            return;
        }
        itemImg.sprite = item.itemSprite;
        itemImg.enabled = true;

        count = slot.amount;
        itemCountParent.SetActive(false);
        if (count > 1) DisplayCount(count);
    }

    void DisplayCount(int count)
    {
        itemCount.text = count.ToString();
        itemCountParent.SetActive(true);
    }

}
