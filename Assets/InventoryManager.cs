using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    [SerializeField] Image heldItemImg;
    public bool holdingItem;
    public InventorySlot heldItem;
    InventoryScript inv;
    [SerializeField] TextMeshProUGUI toolTip;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inv = GameManager.instance.inventory;
        GameManager.instance.invParent = gameObject;
    }

    public void DisplayToolTip(string item)
    {
        if (toolTip.transform.parent.gameObject.activeInHierarchy && toolTip.text == item) return;
        toolTip.transform.parent.gameObject.SetActive(true);
        toolTip.text = item;
    }

    public void EndToolTip(string item)
    {
        if (toolTip.text != item) return;
        toolTip.transform.parent.gameObject.SetActive(false);
        toolTip.text = item;
    }

    private void Update()
    {
        if (toolTip.transform.parent.gameObject.activeInHierarchy) toolTip.transform.parent.position = Input.mousePosition;
        if (holdingItem) heldItemImg.transform.position = Input.mousePosition;
        else heldItemImg.gameObject.SetActive(false);
    }

    public void StopHoldingItem()
    {
        heldItem = null;
        heldItemImg.gameObject.SetActive(false);
        holdingItem = false;
    }

    public void PutDownHeldItemInInv()
    {
        if (heldItem == null || !holdingItem) return;
        MoveItemToInv(heldItem);
        StopHoldingItem();
    }

    public void PickupItem(InventorySlot itemData)
    {
        if (itemData == null) return;

        if (Input.GetKey(KeyCode.LeftShift)) {
            if (itemData.item.hotbarIndex != -1)
                MoveItemToInv(itemData);
            else MoveItemToHotbar(itemData);
            return;
        }

        heldItem = itemData;
        heldItemImg.sprite = itemData.item.itemSprite;
        heldItemImg.gameObject.SetActive(true);
        holdingItem = true;
    }
    void MoveItemToInv(InventorySlot itemData)
    {
        int freeSlot = inv.GetFreeInvSlot();
        inv.SetInvSlot(itemData, freeSlot);
        inv.SetHotbarSlot(itemData, -1);
        //print("moved " + itemData.item.name + " to inventorySlot: " + freeSlot);
        InventoryScript.OnHotbarUpdate.Invoke();
    }

    void MoveItemToHotbar(InventorySlot itemData)
    {
        int freeSlot = inv.GetFreeHotbarSlot();
        inv.SetInvSlot(itemData, -1);
        inv.SetHotbarSlot(itemData, freeSlot);
        InventoryScript.OnHotbarUpdate.Invoke();
    }

}
