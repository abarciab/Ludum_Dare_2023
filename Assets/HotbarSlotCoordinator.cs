using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSlotCoordinator : MonoBehaviour
{
    [SerializeField] GameObject selected;
    [SerializeField] int index;
    InventoryScript inv;

    private void Start()
    {
        inv = GameManager.instance.inventory;
        index = transform.GetSiblingIndex();
        InventoryScript.OnHotbarUpdate += UpdateDisplay;
    }

    void UpdateDisplay()
    {
        if (inv.hotbarIndex == index) selected.SetActive(true);
        else selected.SetActive(false);
    }

}
