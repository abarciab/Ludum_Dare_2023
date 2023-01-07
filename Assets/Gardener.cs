using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gardener : MonoBehaviour
{
    [System.Serializable]
    public class SeedPlantPair
    {
        public ItemObject seed;
        public Plant plant;
    }

    [SerializeField] Plot nearbyCrop;
    [SerializeField] Plant toPlant;
    [SerializeField] ItemObject wateringCan;
    [SerializeField] List<SeedPlantPair> seedPlantList = new List<SeedPlantPair>();
    
    public Plant GetSelectedSeedPlant()
    {
        var heldItem = GameManager.instance.inventory.selectedItem;
        for (int i = 0; i < seedPlantList.Count; i++) {
            if (seedPlantList[i].seed == heldItem) return seedPlantList[i].plant;
        }
        return null;
    }

    public bool HoldingWateringCan()
    {
        if (GameManager.instance.inventory.selectedItem == wateringCan) return true;
        return false;
    }
}
