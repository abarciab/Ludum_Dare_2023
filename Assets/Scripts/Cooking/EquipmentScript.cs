using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentScript : MonoBehaviour
{
    // the number of slots which the player can enter stuff. negative is infinite
    public int maxSlots = 3;
    public int currSlots = 0;

    public InventoryScript playerInventory;
    public GameObject UIBox;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.Find("player").GetComponent<InventoryScript>();
        UIBox = GameObject.Find("UIBox");
    }

    // Update is called once per frame
    void Update()
    {
    }

    /*void UpdateUI() {
        Transform topLeft = UIBox.Find("TopLeft").transform;
        Transform bottomRight = UIBox.Find("BotRight").transform;
        float x_interval = BotRight.x - topLeft.x;

        // dynamically create UI elements based on your mom
        for(int i = 0; i < currSlots; i++) {

        }
    }*/

    void OnTriggerEnter2D(Collider2D other) {
        // if player hits f while in collision, can interaact with the object
        if (Input.GetKey(KeyCode.F)) {
            print("try to cook something");
        }
    }

    /* 
    DRAG: make it so that for the inventory, the player can drag a UI item to somewhere.
    when the player lets go, if its on an interactive element that accepts it as input AND the player is in range, 
    the dragged element will be stored in the equipment, removing it from the inventory.
    if not a valid drop, it will be added back to the inventory.

    the inventory itself is a valid drop into one of the grids.

    to get the item back, the player can simply click on the item in the square bracket to have it added to their inventory.
    when cooked, the item will dissapear :D

    will work on once the inventory works
    */

    // squares will be constantly added to the side of squares to drag to until mixSlots is reached
    // for now, if the player walks near it, it will automatically take two items from the player
    void TempTakeItems() {
        playerInventory.RemoveItem(playerInventory.container[0].item);
        currSlots+=1;
        //Instantiate()
    }
}
