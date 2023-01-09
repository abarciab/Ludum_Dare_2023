using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Van : MonoBehaviour
{
    [SerializeField] GameObject packagePrefab;
    [SerializeField] Vector2 packagePos;
    [SerializeField]List<ItemObject> scheduledItems = new List<ItemObject>();
    bool delivering;

    private void Start()
    {
        GameManager.instance.vanScript = this;
        gameObject.SetActive(false);
    }

    public void DropOffItem(ItemObject item)
    {
        if (!delivering) gameObject.SetActive(false);
        scheduledItems.Add(item);

        if (!delivering) {
            delivering = true;
            gameObject.SetActive(true);
        }
    }

    public void SpawnPackage()
    {
        var newPackage = Instantiate(packagePrefab, packagePos, Quaternion.identity);
        newPackage.GetComponent<Package>().contents = scheduledItems[0];
        newPackage.GetComponent<Package>().Awake();
        AudioManager.instance.PlayGlobal(4);
    }

    public void CompleteDeliver()
    {
        delivering = false;
        scheduledItems.RemoveAt(0);
        if (scheduledItems.Count > 0) {
            DropOffItem(scheduledItems[0]);
            scheduledItems.RemoveAt(0);
        }
        else {
            gameObject.SetActive(false);
        }
    }
}
