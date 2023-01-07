using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Item", menuName="ScriptableObjects/ItemObject", order=1)]
public class ItemObject : ScriptableObject
{
    public string itemName;
    public string type;
    public void Init(string _itemName, string _type) {
        itemName = _itemName;
        type = _type;
    }
}
