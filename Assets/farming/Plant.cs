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
    public enum Type { carrot, potato, tomato}
    public enum growthStage { seed, sprout, mature, wilted}

    [SerializeField] Type type;
    [SerializeField] growthStage currentStage;
    [SerializeField] float stageTimeLength;
    bool growing;
    float currentStateCountdown;
    Plot currentPlot;
    [SerializeField] plantSprites sprites;


    public void PlantInPlot(Plot newPlot)
    {
        currentPlot = newPlot;
    }
    public Sprite GetCurrentSprite()
    {
        switch (currentStage) {
            case growthStage.seed:
                return sprites.seed;
            case growthStage.sprout:
                return sprites.sprout;
            case growthStage.mature:
                return sprites.mature;
            case growthStage.wilted:
                return sprites.wilted;
        }
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
        if (currentStage == growthStage.wilted) 
        { 
            growing = false;
            return; 
        }
        currentStateCountdown = currentStage == growthStage.wilted ? stageTimeLength * 5 : stageTimeLength;
        currentPlot.PlantAdvance();
    }
}
