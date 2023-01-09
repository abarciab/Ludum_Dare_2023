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
    public int maxDishes = 3;

    public bool bought = false;
    public int price = 10;

    public bool canInteract = false;
    public bool used = false;
    public bool burnt = false;
    public bool usesFire = true;

    // dish result
    private string result = "";

    [SerializeField] public ParticleSystem smokeEmitter;
    protected Sprite defaultSprite;
    [SerializeField] protected Sprite usedSprite;
    protected SpriteRenderer equipRenderer;
    public List<ItemObject> currentItems = new List<ItemObject>();
    public List<ItemObject> cookingItems = new List<ItemObject>();
    public Dictionary<string, int> numTypes = new Dictionary<string, int>();
    [SerializeField] public List<ItemObject> possibleResults = new List<ItemObject>();
    [SerializeField] public List<string> validTypes = new List<string>();

    private InventoryScript playerInventory;
    public List<GameObject> UIContainer = new List<GameObject>();
    // [SerializeField] Image itemImg;
    private Transform UIBox;
    private GameObject UISquare;

    [SerializeField] public TextMeshProUGUI priceText;

    public float cookingTimer = 0f;
    public float baseCookingTime = 5f;
    public float burntTimer = 10f;

    private ItemObject burntFood;
    private ItemObject strangeFood;

    private AudioSource source;
    private AudioSource smokeSource;

    public int GreatMealValue = 50;

    private float blipFrequencyRate = 0.2f;


    // Start is called before the first frame update
    protected void Start()
    {
        playerInventory = GameObject.Find("player").GetComponent<InventoryScript>();
        UIBox = transform.GetChild(0).transform;
        UISquare = UIBox.GetChild(2).gameObject;
        equipRenderer = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
        defaultSprite = equipRenderer.sprite;
        
        // sort by mealValue
        possibleResults.Sort(delegate(ItemObject a, ItemObject b) { return b.mealValue.CompareTo(a.mealValue); });
        foreach(ItemObject dish in possibleResults) {
            if (dish.itemName.ToLower() == "burnt food") {
                burntFood = dish;
            }
            else if (dish.itemName.ToLower() == "strange food") {
                strangeFood = dish;
            }
        }
        if (smokeEmitter) {
            smokeSource = transform.Find("Smoke Emitter").gameObject.GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        UpdateUI();
        UpdateCooking();
        UpdateSmoke();
        UpdateSprite();
        priceText.enabled = false;
        if (canInteract) {
            if (!bought) {
                print("show price text");
                priceText.enabled = true;
                priceText.text = $"${price}";
                if (Input.GetKeyDown(KeyCode.F) && playerInventory.money >= price) {
                    bought = true;
                    playerInventory.money -= price;
                }
                return;
            }
            if (Input.GetKeyDown(KeyCode.F)) {
                AddItems();
            }
            if (!bought) return;
            if (Input.GetKeyDown(KeyCode.C)) {
                Cook();
            }
            if (Input.GetKeyDown(KeyCode.G)) {
                TakeItems();
            }
        }
    }

    void UpdateCooking() {
        if (!used) return;

        cookingTimer -= Time.deltaTime;
        if (cookingTimer <= 0 && currentItems.Count == 0) {
            AudioManager.instance.PlayHere(10, source); 
            foreach (ItemObject item in cookingItems) {
                currentItems.Add(item);
            }
            cookingItems.Clear();
            cookingTimer = burntTimer;
        }
        else if (!burnt && usesFire && cookingTimer <= -1*burntTimer && currentItems.Count != 0) {
            print("food got burnt!");
            AudioManager.instance.PlayHere(9, source);
            burnt = true;
            for (int i = 0; i < currentItems.Count; i++) {
                currentItems.Remove(currentItems[i]);
                currentItems.Add(burntFood);
            }
        }
    }

    void UpdateUI() {
        if (numItems == currentItems.Count) return;
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
            square.SetActive(true);
            square.GetComponent<UISquareScript>().storedItem = currentItems[i-1];
            square.GetComponent<Image>().sprite = currentItems[i-1].itemSprite;
            UIContainer.Add(square);
        }
    }

    protected void UpdateSprite() {
        if (!bought) {
            equipRenderer.color = Color.grey;
        }
        else {
            equipRenderer.color = Color.white;
        }
        if (used && usedSprite) {
            equipRenderer.sprite = usedSprite;
        }
        else if (!used) {
            equipRenderer.sprite = defaultSprite;
        }
    }

    void UpdateSmoke() {
        if (used && usesFire && !smokeEmitter.isPlaying) {
            smokeEmitter.Play();
        }
        else if (!used && smokeEmitter.isPlaying){
            smokeEmitter.Stop();
            if (smokeSource.isPlaying) {
                smokeSource.Stop();
            }
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
    void AddItems() {
        if (currentItems.Count >= maxSlots || used) return;
        ItemObject item = playerInventory.selectedItem;
        if (item && validTypes.Contains(item.type.ToLower())) {
            print($"add {item.itemName}");
            playerInventory.RemoveItem(playerInventory.selectedItem);
            float freq = 1 + currentItems.Count * blipFrequencyRate;
            print(freq);
            AudioManager.instance.PlayHere(16, source);  
            source.pitch = freq;
            currentItems.Add(item);
        }
        else if (item) {
            print($"{item.itemName} is invalid");
        }
    }

    void TakeItems() {
        if (used)
            CheckDishQuality();
        foreach(ItemObject item in currentItems) {
            print($"Took {item.itemName}");
            playerInventory.AddItem(item);
        }
        currentItems.Clear();
        used = false;
        burnt = false;
    }

    /*
    look through the recipe for the possible results, then take the right items
    edge cases:
        curr_items = [potato, potato, potato, potato]
        recipe = [1 vege, 2 potato]
        usedItems = []
        

        potato is vege but is also in recipe

        potato: is in & not in dict, so usedItems = [potato: 1]
        potato: is in, in dict but 1 < 2, so usedItems = [potato: 2]
        potato: is in, in dict but 2 !< 2
            check type: is vege, so usedItems = [potato: 2, vege: 1]
        potato: is in, in dict but 2 !< 2
            check type: is vege, but 1 !< 1, so is invalid.

        for the final check, compare sizes of recipe 
    */
    public void Cook() {
        if (currentItems.Count == 0 || used) return;
        source.pitch = 1;
        Dictionary<string, int> usedIng = new Dictionary<string, int>();
        List<ItemObject> usedItems = new List<ItemObject>();

        for (int i = 0; i < possibleResults.Count; i++) {
            if (cookingItems.Count >= maxDishes) break;

            ItemObject dish = possibleResults[i];
            bool lastDish = cookingItems.Count == maxDishes-1;
            int recipeSize = 0;
            bool overintake = false;
            foreach (Ingredient ingr in dish.recipe) {
                recipeSize += ingr.quantity;
                if (ingr.overintake) overintake = true;
            }
            // if their's only one slot left, only check recipies of the same size as the items OR overload recipes
            if (
                (recipeSize == 0 || recipeSize > currentItems.Count)
                || (lastDish && (recipeSize != currentItems.Count && !overintake))
            ) {
                continue;
            }
            // if one of the ingr is not fulfilled, then cook is false
            foreach (ItemObject item in currentItems) {
                bool validItem = false;
                string itemName = item.itemName.ToLower();
                // check if the ingredient is in the queue
                // this checks for all the name coinciding first
                foreach (Ingredient ingr in dish.recipe) {
                    bool validName = (
                        itemName==ingr.ingredient.ToLower()
                        && (!usedIng.ContainsKey(itemName) || (usedIng.ContainsKey(itemName) && (usedIng[itemName] < ingr.quantity || ingr.overintake)))
                    );
                    if (validName) {
                        if (!usedIng.ContainsKey(itemName)) usedIng[itemName] = 0;
                        usedIng[itemName]++;
                        usedItems.Add(item);
                        validItem = true;
                        break;
                    }
                }
                // if not a valid item, check if it's a valid type
                if (!validItem) {
                    string itemType = item.type.ToLower();
                    foreach (Ingredient ingr in dish.recipe) {
                        bool validType = (
                            itemType==ingr.ingredient.ToLower()
                            && (!usedIng.ContainsKey(itemType) || (usedIng.ContainsKey(itemType) && (usedIng[itemType] < ingr.quantity || ingr.overintake)))
                        );
                        if (validType) {
                            if (!usedIng.ContainsKey(itemType)) usedIng[itemType] = 0;
                            usedIng[itemType]++;
                            usedItems.Add(item);
                            validItem = true;
                            break;
                        }
                    }
                }
            }
            print("used items:");
            foreach (ItemObject item in usedItems) print(item);
            // cook IF: used items >= recipeSize 
            bool cook = (usedItems.Count >= recipeSize);
            // however if: overintake & last dish: cook only if every item is used
            if (cook && lastDish && overintake) {
                cook = usedItems.Count >= currentItems.Count;
            }
            
            if (cook) {
                print($"make {dish.itemName}");
                foreach(ItemObject item in usedItems) {
                    currentItems.Remove(item);
                }
                i--;
                cookingItems.Add(dish);
            }
            else {
                print($"couldn't make {dish.itemName}");    
            }
            usedItems.Clear();
            usedIng.Clear();
        } // end of dish loop
        // if there are remaining items & still space, make strange food
        if (currentItems.Count != 0 && cookingItems.Count < maxDishes) {
            print("strange food made");
            cookingItems.Add(strangeFood);
        }
        if (usesFire) {
            AudioManager.instance.PlayHere(8, source);
            AudioManager.instance.PlayHere(11, smokeSource);
        }
        else {
            AudioManager.instance.PlayHere(12, source);
        }

        cookingTimer = cookingItems.Count * baseCookingTime;
        used = true;
        currentItems.Clear();
    }

    void CheckDishQuality() {
        int highestVal = 0;
        int playId = 13;
        foreach (ItemObject dish in currentItems) {
            if (dish.mealValue > highestVal)
                highestVal = dish.mealValue;
        }

        if (highestVal <= 0) playId = 13;
        else if (highestVal < GreatMealValue) playId = 14;
        else playId = 15;
        AudioManager.instance.PlayHere(playId, source);
    }
}