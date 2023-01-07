using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotCoordinator : MonoBehaviour
{
    [SerializeField] GameObject itemCountParent;
    [SerializeField] int slotIndex;
    InventoryScript inv;
    InventorySlot itemData;
    [SerializeField] public Image itemImg;
    [SerializeField] TextMeshProUGUI itemCount;

    private void Start()
    {
        inv = GameManager.instance.inventory;
        slotIndex = transform.GetSiblingIndex() + 8;
        InventoryScript.OnHotbarUpdate += UpdateDisplay;
        DisplayItem();
    }

    void UpdateDisplay()
    {
        DisplayItem();
    }

    public void PickupItem()
    {
        if (!InventoryManager.instance.gameObject.activeInHierarchy) return;

        InventorySlot itemToStore = null;
        if (InventoryManager.instance.holdingItem) {
            itemToStore = InventoryManager.instance.heldItem;
            InventoryManager.instance.StopHoldingItem();
        }

        InventoryManager.instance.PickupItem(itemData);
        inv.SetInvSlot(itemData, -1);
        RemoveItem();

        if (itemToStore != null) {
            inv.SetInvSlot(itemToStore, slotIndex);
            DisplayItem(itemToStore);
        }
    }

    void RemoveItem()
    {
        itemData = null;
        DisplayItem();
    }

    void DisplayItem(InventorySlot _itemData = null)
    {
        if (_itemData == null) {
            _itemData = inv.GetItemInInventory(slotIndex);
            if (_itemData == null) _itemData = itemData;
        }

        if (_itemData == null) {
            itemImg.sprite = null;
            itemImg.enabled = false;
            itemData = null;
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
    }
}
