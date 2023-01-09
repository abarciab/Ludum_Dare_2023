using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PageObject : MonoBehaviour
{
    ItemObject dish;
    [SerializeField] Image dishImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI equipmentText;
    [SerializeField] TextMeshProUGUI descText;
    
    
    public void Display(ItemObject _dish)
    {
        dish = _dish;
        dishImage.sprite = dish.itemSprite;
        nameText.text = dish.name;
        equipmentText.text = dish.equipment;
        descText.text = dish.description;
    }
}
