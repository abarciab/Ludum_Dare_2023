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
    [SerializeField] float particleJiggleSpeed = 2;
    [SerializeField] float particleJiggleScale = 2;

    //dependencies
    SpriteRenderer plotImg;
    [SerializeField] SpriteRenderer plantImg;
    [SerializeField] GameObject ripeParticles;
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
        if (ripeParticles.activeInHierarchy) ripeParticles.transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * particleJiggleSpeed) * particleJiggleScale);
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
        if (currentPlant.currentStage == Plant.GrowthStage.mature) ripeParticles.SetActive(true);
        else ripeParticles.SetActive(false);
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
        ripeParticles.SetActive(false);
        if (currentPlant.currentStage == Plant.GrowthStage.wilted) {
            currentPlant = null;
            return;
        }
        if (currentPlant.currentStage == Plant.GrowthStage.mature) {
            watered = false;
            AudioManager.instance.PlayHere(1, source);
            GameManager.instance.inventory.AddItem(currentPlant.plantItem);
            currentPlant = null;
            return;
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.invParent.activeInHierarchy) return;

        if (Vector2.Distance(transform.position, GameManager.instance.player.transform.position) > GameManager.instance.playerReach) return;
        if (gardener.HoldingWateringCan()) {
            AudioManager.instance.PlayHere(2, source);
            StartCoroutine(waitThenWater());
        }
        Interact(gardener.GetSelectedSeedPlant());
    }

    IEnumerator waitThenWater()
    {
        var controller = GameManager.instance.player.GetComponent<PlayerController>();
        var inv = GameManager.instance.inventory;

        controller.enabled = inv.enabled = false;
        GameManager.instance.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(GameManager.instance.wateringTime);
        watered = controller.enabled = inv.enabled = true;
    }
}
