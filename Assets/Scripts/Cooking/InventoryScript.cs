using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryScript : MonoBehaviour {
    public List<InventorySlot> container = new List<InventorySlot>();

    void Start() {
        // temp code to test
        ItemObject item = ScriptableObject.CreateInstance("ItemObject") as ItemObject;
        item.Init("test", "type");
        AddItem(item);
    }

    public void AddItem(ItemObject _item, int _amount=1) {
        for (int i = 0; i < container.Count; i++) {
            if (container[i].item == _item) {
                container[i].AddAmount(_amount);
                return;
            }
        }
        container.Add(new InventorySlot(_item, _amount));
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
