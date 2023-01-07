using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryScript : MonoBehaviour {
    public List<InventorySlot> container = new List<InventorySlot>();
    
    //hotbar
    public static Action OnHotbarUpdate;
    public int hotbarIndex;

    void Start() {
        // temp code to test
        ItemObject item = ScriptableObject.CreateInstance("ItemObject") as ItemObject;
        item.Init("test", "type");
        AddItem(item);
    }

    public bool AddItem(ItemObject _item, int _amount=1) {
        for (int i = 0; i < container.Count; i++) {
            if (container[i].item == _item) {
                container[i].AddAmount(_amount);
                return true;
            }
        }
        container.Add(new InventorySlot(_item, _amount));
        return true;
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
        
    }

    void ScrollHotBar()
    {
        var scroll = Input.mouseScrollDelta.y;
        if (scroll > 0) hotbarIndex += 1;
        if (scroll < 0) hotbarIndex -= 1;
        if (hotbarIndex < 0) hotbarIndex = 8;
        if (hotbarIndex > 8) hotbarIndex = 0;
        if (scroll != 0) UpdateHotbar();
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
