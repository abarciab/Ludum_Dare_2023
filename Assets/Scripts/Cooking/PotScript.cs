using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotScript : EquipmentScript
{
    protected SpriteRenderer stoveRenderer;
    protected Sprite stoveSprite;
    [SerializeField] protected Sprite usedStoveSprite;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        stoveRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
        equipRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        defaultSprite = equipRenderer.sprite;
        stoveSprite = stoveRenderer.sprite;
    }

    void Update()
    {
        base.Update();
        UpdateSprite();
    }

    void UpdateSprite()
    {
        base.UpdateSprite();
        if (used && usedStoveSprite) {
            stoveRenderer.sprite = usedStoveSprite;
        }
        else if (!used) {
            stoveRenderer.sprite = stoveSprite;
        }
    }
}
