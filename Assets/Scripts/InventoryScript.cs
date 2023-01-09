using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryScript : MonoBehaviour {
    public List<InventorySlot> container = new List<InventorySlot>();
    //hotbar
    public static Action OnHotbarUpdate;
    public int hotbarIndex;
    public ItemObject selectedItem;
    //UI
    [SerializeField] TextMeshProUGUI moneyDisplay;
    public int money;

    private void Awake()
    {
        OnHotbarUpdate = null;
    }

    void Start() {
        AddExistingItemsToHotbar();
        UpdateHotbar();
    }

    void AddExistingItemsToHotbar()
    {
        for (int i = 0; i < container.Count; i++) {
            if (container[i].item.itemName.ToLower() == "potato seeds") {
                container[i].item.hotbarIndex = -1;
                container[i].invSlotIndex = 8;
            }
            else {
                container[i].item.hotbarIndex = -1;
                container[i].invSlotIndex = -1;
            }
        }

        for (int i = 0; i < 7; i++) {
            for (int j = 0; j < container.Count; j++) {
                //if (container[j].item.itemName.ToLower() == "potato seeds") continue;
                if (container[j].item.hotbarIndex == -1 && container[j].invSlotIndex == -1) {
                    container[j].item.hotbarIndex = i;
                    break;
                }
            }
        }
    }

    public bool HasItem(string name)
    {
        for (int i = 0; i < container.Count; i++) {
            if (container[i].item.itemName.ToUpper() == name.ToUpper()) return true;
        }
        return false;
    }

    public bool AddItem(ItemObject _item, int _amount=1) {
        if (_item == null) return false;

        for (int i = 0; i < container.Count; i++) {
            if (container[i].item == _item) {
                container[i].AddAmount(_amount);
                UpdateHotbar();
                return true;
            }
        }

        _item.hotbarIndex = GetFreeHotbarSlot();
        int invSlotIndex = _item.hotbarIndex == -1 ? GetFreeInvSlot() : -1;
        container.Add(new InventorySlot(_item, invSlotIndex, _amount));
        if (_item.hotbarIndex != -1) UpdateHotbar();
        
        return true;
    }

    

    public void SetInvSlot(InventorySlot item, int slotIndex)
    {
        if (item == null) return;

        for (int i = 0; i < container.Count; i++) {
            if (item.item.name == container[i].item.name) {
                container[i].invSlotIndex = slotIndex;
                return;
            }
        }
    }

    public void SetHotbarSlot(InventorySlot item, int hotbarIndex)
    {
        if (item == null) return;

        for (int i = 0; i < container.Count; i++) {
            if (item.item.name == container[i].item.name) {
                container[i].item.hotbarIndex = hotbarIndex;
                return;
            }
        }
    }

    public InventorySlot GetItemInInventory(int _invSlotIndex)
    {
        for (int i = 0; i < container.Count; i++) {
            if (container[i].invSlotIndex == _invSlotIndex)
                return container[i];
        }
        return null;
    }

    public InventorySlot GetItemInHotbarSlot(int hotbarSlot)
    {
        for (int i = 0; i < container.Count; i++) {
            if (container[i].item.hotbarIndex == hotbarSlot)
                return container[i];
        }
        return null;
    }

    public int GetFreeInvSlot()
    {
        int freeSlot = 8;
        for (int i = 8; i < 35; i++) {
            freeSlot = i;
            for (int j = 0; j < container.Count; j++) {
                if (container[j].invSlotIndex == i) {
                    freeSlot = -1;
                    break;
                }
            }
            if (freeSlot == i) break;
        }
        return freeSlot;
    }

    public int GetFreeHotbarSlot()
    {
        int freeSlot = -1;
        for (int i = 0; i < 7; i++) {
            freeSlot = i;
            for (int j = 0; j < container.Count; j++) {
                if (container[j].item.hotbarIndex == i) {
                    freeSlot = -1;
                    break;
                }
            }
            if (freeSlot == i) break;
        }
        return freeSlot;
    }

    public ItemObject RemoveItem(ItemObject _item, int _amount=1) {
        ItemObject item = null;
        for (int i = 0; i < container.Count; i++) {
            if (container[i].item == _item) {
                container[i].AddAmount(_amount*-1);
                if (container[i].amount <= 0) {
                    item = container[i].item;
                    container.RemoveAt(i);
                }
                break;
            }
        }
        UpdateHotbar();
        return item;
    }

    private void Update()
    {
        selectHotbarItem();
        UpdateHotbar();
        moneyDisplay.text = "$ " + money;
    }

    void selectHotbarItem()
    {
        HotbarHotkeys();

        var scroll = Input.mouseScrollDelta.y;
        if (scroll < 0) hotbarIndex += 1;
        if (scroll > 0) hotbarIndex -= 1;
        if (hotbarIndex < 0) hotbarIndex = 7;
        if (hotbarIndex > 7) hotbarIndex = 0;
    }

    void HotbarHotkeys()
    {
        var index = hotbarIndex;
        if (Input.GetKeyDown(KeyCode.Alpha1)) hotbarIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) hotbarIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) hotbarIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) hotbarIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) hotbarIndex = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6)) hotbarIndex = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7)) hotbarIndex = 6;
        if (Input.GetKeyDown(KeyCode.Alpha8)) hotbarIndex = 7;
        if (index != hotbarIndex) UpdateHotbar();
    }

    void UpdateHotbar()
    {
        if (OnHotbarUpdate != null) OnHotbarUpdate.Invoke();
    }
}

[System.Serializable]
public class InventorySlot {
    //[HideInInspector] public int invSlotIndex;
    public int invSlotIndex;
    public ItemObject item;
    public int amount;
    public InventorySlot(ItemObject _item, int invSlotIndex, int _amount = 1)
    {
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}
/*

    void InitInven() {
        inventory = GameObject.Find("Inventory").GetComponent<InventoryObject>();
        ItemObject item = ScriptableObject.CreateInstance("ItemObject") as ItemObject;
        item.Init("test", "type");
        inventory.AddItem(item);
    }
*/
