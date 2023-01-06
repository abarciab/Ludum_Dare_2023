using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plot : MonoBehaviour
{
    public enum plotStatus { raw, tilled, planted}
    [SerializeField] bool watered;
    [SerializeField] Plant currentPlant;
    Image plotImg;
    [SerializeField] Image plantImg;

    void Update()
    {
        if (currentPlant) GrowPlant();

        //TEST

    }

    void GrowPlant()
    {
        currentPlant.Countdown(Time.deltaTime);
    }

    void UpdateVisuals()
    {
        if (currentPlant) plantImg.sprite = currentPlant.GetCurrentSprite();
    }

    public void PlantAdvance()
    {
        UpdateVisuals();
    }

    public void Plant(Plant plant)
    {
        UpdateVisuals();
        plant.PlantInPlot(this);
        currentPlant = plant;
    }
}
