using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Item", menuName="ScriptableObjects/ItemObject", order=1)]
public class ItemObject : ScriptableObject
{
    [HideInInspector]public int hotbarIndex = -1;
    public Sprite itemSprite;
    public string itemName;
    /*
    types:
        vege
        meat
        ing
        seed
        item
        dish
    */
    public string type;
    [SerializeField] bool edible;
    [Tooltip("The amount of money gained from this item when fed to a hungry resident")]
    public int mealValue = 10;
    public int inrgedientCost;
    public void Init(string _itemName, string _type) {
        itemName = _itemName;
        type = _type;
    }

    public bool IsEdible()
    {
        return edible;
    }
}
