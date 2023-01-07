using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Plot : MonoBehaviour
{
    public enum plotStatus { raw, tilled, planted}
    [SerializeField] bool watered;
    Plant currentPlant;
    [SerializeField] Sprite drySoil;
    [SerializeField] Sprite wetSoil;

    //dependencies
    SpriteRenderer plotImg;
    [SerializeField] SpriteRenderer plantImg;
    AudioSource source;
    Gardener gardener;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        gardener = FindObjectOfType<Gardener>();
        plotImg = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (currentPlant && watered) GrowPlant();
        plotImg.sprite = watered ? wetSoil : drySoil;
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

    public void PlantHere(Plant _plant)
    {
        var plant = Instantiate(_plant);
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
        if (toPlant != null)
            PlantHere(toPlant);
    }

    void RemoveCurrentPlant()
    {
        if (currentPlant.currentStage == Plant.GrowthStage.wilted) {
            currentPlant = null;
            return;
        }
        if (currentPlant.currentStage == Plant.GrowthStage.mature) {
            watered = false;
            AudioManager.instance.PlayGlobal(1);
            GameManager.instance.inventory.AddItem(currentPlant.plantItem);
            currentPlant = null;
            return;
        }
    }

    private void OnMouseDown()
    {
        if (Vector2.Distance(transform.position, GameManager.instance.player.transform.position) > GameManager.instance.playerReach) return;
        if (gardener.HoldingWateringCan()) {
            watered = true;
        }
        Interact(gardener.GetSelectedSeedPlant());
    }
}
