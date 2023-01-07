using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Plot : MonoBehaviour
{
    public enum plotStatus { raw, tilled, planted}
    [SerializeField] bool watered;
    [SerializeField] Plant currentPlant;
    SpriteRenderer plotImg;
    [SerializeField] SpriteRenderer plantImg;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (currentPlant) GrowPlant();
    }

    void GrowPlant()
    {
        currentPlant.Countdown(Time.deltaTime);
        
    }

    void UpdateVisuals()
    {
        if (currentPlant) plantImg.sprite = currentPlant.GetCurrentSprite();
        else plantImg.sprite = null;
    }

    public void PlantAdvance()
    {
        UpdateVisuals();
        AudioManager.instance.PlayHere(0, source);
    }

    public void PlantHere(Plant plant)
    {
        plant.PlantInPlot(this);
        currentPlant = plant;
        UpdateVisuals();
        AudioManager.instance.PlayHere(0, source);
    }

    public void Interact(Plant toPlant)
    {
        if (currentPlant) {
            RemoveCurrentPlant();
            UpdateVisuals();
            return;
        }
        PlantHere(toPlant);
    }

    void RemoveCurrentPlant()
    {
        if (currentPlant.currentStage == Plant.GrowthStage.wilted) {
            currentPlant = null;
            return;
        }
        if (currentPlant.currentStage == Plant.GrowthStage.mature) {
            AudioManager.instance.PlayGlobal(1);
            GameManager.instance.inventory.AddItem(currentPlant.plantItem);
            currentPlant = null;
            return;
        }
    }
}
