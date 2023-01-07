using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


// [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryScript : MonoBehaviour {
    public List<InventorySlot> container = new List<InventorySlot>();
    
    //hotbar
    public static Action OnHotbarUpdate;
    public int hotbarIndex;
    public ItemObject selectedItem;

    void Start() {
        // temp code to test
        ItemObject item = ScriptableObject.CreateInstance("ItemObject") as ItemObject;
        item.Init("test", "type");
        AddItem(item);
        UpdateHotbar();
    }

    public bool AddItem(ItemObject _item, int _amount=1) {
        for (int i = 0; i < container.Count; i++) {
            if (container[i].item == _item) {
                container[i].AddAmount(_amount);
                return true;
            }
        }
        _item.hotbarIndex = GetFreeHotbarSlot();
        container.Add(new InventorySlot(_item, _amount));
        if (_item.hotbarIndex != -1) UpdateHotbar();
        
        return true;
    }

    public ItemObject GetItemInHotbarSlot(int hotbarSlot)
    {
        for (int i = 0; i < container.Count; i++) {
            if (container[i].item.hotbarIndex == hotbarSlot)
                return container[i].item;
        }
        return null;
    }

    int GetFreeHotbarSlot()
    {
        int freenum = -1;
        for (int i = 0; i < 7; i++) {
            freenum = i;
            for (int j = 0; j < container.Count; j++) {
                if (container[j].item.hotbarIndex == i) {
                    freenum = -1;
                    break;
                }
            }
            if (freenum == i) break;
        }
        return freenum;
    }

    public bool RemoveItem(ItemObject _item, int _amount=1) {
        for (int i = 0; i < container.Count; i++) {
            if (container[i].item == _item) {
                container[i].AddAmount(_amount*-1);
                if (container[i].amount <= 0) {
                    container.RemoveAt(i);
                }
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        selectHotbarItem();
    }

    void selectHotbarItem()
    {
        HotbarHotkeys();

        var scroll = Input.mouseScrollDelta.y;
        if (scroll < 0) hotbarIndex += 1;
        if (scroll > 0) hotbarIndex -= 1;
        if (hotbarIndex < 0) hotbarIndex = 7;
        if (hotbarIndex > 7) hotbarIndex = 0;
        if (scroll != 0) UpdateHotbar();
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
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public InventorySlot(ItemObject _item, int _amount=1) {
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value) {
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
