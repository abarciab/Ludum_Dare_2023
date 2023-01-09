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
    InventorySlot itemData;
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

    public void PickupItem()
    {
        if (InventoryManager.instance == null || !InventoryManager.instance.gameObject.activeInHierarchy) return;

        InventorySlot itemToStore = null;
        if (InventoryManager.instance.holdingItem) {
            itemToStore = InventoryManager.instance.heldItem;
            InventoryManager.instance.StopHoldingItem();
        }

        InventoryManager.instance.PickupItem(itemData);
        RemoveItem();

        if (itemToStore != null) {
            inv.SetHotbarSlot(itemToStore, index);
            DisplayItem(itemToStore);
        }
    }

    void RemoveItem()
    {
        inv.SetHotbarSlot(itemData, -1);
        if (itemData != null) InventoryManager.instance.EndToolTip(itemData.item.name);
        itemData = null;
        DisplayItem();
        DisplayCount(0);

    }

    void DisplayItem(InventorySlot _itemData = null)
    {
        if (inv == null) return;
        if (_itemData == null) _itemData = inv.GetItemInHotbarSlot(index);
        
        if (selected.activeInHierarchy) {
            inv.selectedItem = _itemData == null ? null : _itemData.item;
        }
        if (_itemData == null) {
            itemImg.sprite = null;
            itemImg.enabled = false;
            return;
        }

        itemData = _itemData;
        itemImg.sprite = itemData.item.itemSprite;
        itemImg.enabled = true;
        itemCountParent.SetActive(false);
        if (itemData.amount > 1) DisplayCount(itemData.amount);
    }

    void DisplayCount(int count)
    {
        itemCount.text = count.ToString();
        itemCountParent.SetActive(true);
        if (count == 0) itemCountParent.SetActive(false);
    }

    public void Hover()
    {
        if (InventoryManager.instance == null || itemData == null || !InventoryManager.instance.gameObject.activeInHierarchy) return;
        InventoryManager.instance.DisplayToolTip(itemData.item.name);
    }

    public void ExitHover()
    {
        print("exiting");
        if (itemData == null || InventoryManager.instance == null || !InventoryManager.instance.gameObject.activeInHierarchy) return;
        InventoryManager.instance.EndToolTip(itemData.item.name);
    }

}
