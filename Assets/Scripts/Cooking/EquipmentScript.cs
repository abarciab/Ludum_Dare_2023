using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentScript : MonoBehaviour
{
    // the number of slots which the player can enter stuff. negative is infinite
    public int maxSlots;

    public InventoryScript playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.Find("player").GetComponent<InventoryScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        // if player hits f while in collision, can interaact with the object
        if (Input.GetKey(KeyCode.F)) {
            print("try to cook something");
        }
    }

    // squares will be constantly added to the side of squares to drag to until mixSlots is reached
    // for now, simply get the cooking mechanic working
    void TempTakeItems() {
        // playerInventory.RemoveItem();
    }
}
