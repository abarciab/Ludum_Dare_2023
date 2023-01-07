using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

[CreateAssetMenu(fileName = "Plant", menuName = "ScriptableObjects/Plant", order = 1)]
public class Plant : ScriptableObject
{
    [System.Serializable]
    public class plantSprites
    {
        public Sprite seed;
        public Sprite sprout;
        public Sprite mature;
        public Sprite wilted;
    }
    //public enum Type { carrot, potato, tomato}
    public enum GrowthStage { seed, sprout, mature, wilted}

    //[SerializeField] Type type;
    public ItemObject plantItem;
    public GrowthStage currentStage { get; private set; }
    [SerializeField] float stageTimeLength;
    bool growing = true;
    float currentStateCountdown;
    Plot currentPlot;
    [SerializeField] plantSprites sprites;


    public void PlantInPlot(Plot newPlot)
    {
        currentPlot = newPlot;
        growing = true;
        currentStage = GrowthStage.seed;
        currentStateCountdown = stageTimeLength;
    }
    public Sprite GetCurrentSprite()
    {
        switch (currentStage) {
            case GrowthStage.seed:
                return sprites.seed;
            case GrowthStage.sprout:
                return sprites.sprout;
            case GrowthStage.mature:
                return sprites.mature;
            case GrowthStage.wilted:
                return sprites.wilted;
        }
        Debug.Log("No growth stage");
        return null;
    }

    public void Countdown(float time)
    {
        if (!growing) return;
        currentStateCountdown -= time;
        if (currentStateCountdown <= 0) AdvanceToNextStage();
    }

    void AdvanceToNextStage()
    {
        currentStage += 1;
        if (currentStage == GrowthStage.wilted) 
        { 
            growing = false;
        }
        currentStateCountdown = currentStage == GrowthStage.mature ? stageTimeLength * 5 : stageTimeLength;
        currentPlot.PlantAdvance();
    }
}
