using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Item", menuName="ScriptableObjects/ItemObject", order=1)]
public class ItemObject : ScriptableObject
{
    public string itemName;
    public int quantity;
    public string type;
}
