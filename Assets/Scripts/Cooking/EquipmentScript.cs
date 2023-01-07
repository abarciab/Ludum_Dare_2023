using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentScript : MonoBehaviour
{
    // the number of slots which the player can enter stuff. negative is infinite
    private int numItems = 0;
    public int maxSlots = 3;

    public bool canInteract = false;

    public List<ItemObject> currentItems = new List<ItemObject>();

    [SerializeField] InventoryScript playerInventory;
    // [SerializeField] Image itemImg;
    [SerializeField] Transform UIBox;
    [SerializeField] List<GameObject> UIContainer = new List<GameObject>();
    [SerializeField] GameObject UISquare;


    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.Find("player").GetComponent<InventoryScript>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        if (canInteract) {
            // if player hits f while in collision, can interaact with the object
            if (Input.GetKeyDown(KeyCode.F)) {
                print("try to cook something");
                TempTakeItems();
            }
        }
    }

    void UpdateUI() {
        if (currentItems.Count == 0 || numItems == currentItems.Count) return;
        numItems = currentItems.Count;
        foreach(GameObject square in UIContainer) {
            GameObject.Destroy(square);
        }
        UIContainer.Clear();
        UIBox.transform.position = transform.position;
        Vector3 leftCoord = UIBox.GetChild(0).transform.position;
        Vector3 rightCoord = UIBox.GetChild(1).transform.position;
        float x_interval = (rightCoord.x - leftCoord.x)/(currentItems.Count+1);
        // dynamically create UI elements based on your mom
        for(int i = 1; i <= currentItems.Count; i++) {
            GameObject square = Instantiate(
                UISquare, new Vector3(leftCoord.x+(x_interval*i), UIBox.transform.position.y, 0), Quaternion.identity
            );
            print(square);
            UIContainer.Add(square);
            HotbarSlotCoordinator hotbar = square.GetComponent<HotbarSlotCoordinator>();
            hotbar.itemImg.sprite = currentItems[i-1].itemSprite;
            hotbar.itemImg.enabled = true;
            //UIContainer[square].sprite = currentItems[i-1].itemSprite;
            // UIContainer[square];
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        canInteract = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        canInteract = false;
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
        if (playerInventory.container.Count == 0) return;
        ItemObject item = playerInventory.RemoveItem(playerInventory.selectedItem);
        if (item)
            currentItems.Add(item); 
    }
}
