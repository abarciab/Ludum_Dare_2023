using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISquareScript : MonoBehaviour
{
    public ItemObject storedItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToInventory() {
        print(storedItem);
    }
}
