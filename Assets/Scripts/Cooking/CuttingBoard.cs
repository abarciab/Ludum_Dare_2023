using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : EquipmentScript
{
    public Dictionary<string, int> numTypes = new Dictionary<string, int>();
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        validTypes = new List<string>() {
            "vege",
            "meat"
        };
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    public override void Cook() {
        foreach (ItemObject item in currentItems) {
            bool cooked = true;
            print(item);
            if (!numTypes.ContainsKey(item.type)) numTypes[item.type] = 0;
            numTypes[item.type]++;
            switch (item.itemName.ToLower()) {
                case "potato":
                    // cookingItems.Add();
                    break;
                case "tomato":
                    break;
                default:
                    cooked = false;
                    break;
            }
            if (cooked) continue;
            switch (item.type.ToLower()) {
                case "vege":
                    break;
                case "meat":
                    break;
            }
        }
        currentItems.Clear();
    }

}
