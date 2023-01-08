using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    public ItemObject contents;

    private void Awake()
    {
        transform.position += new Vector3(Random.Range(-1,1), Random.Range(-1, 1), 0);
    }

    private void OnMouseDown()
    {
        if (Vector2.Distance(transform.position, GameManager.instance.player.transform.position) > GameManager.instance.playerReach) return;

        GameManager.instance.inventory.AddItem(contents);
        Destroy(gameObject);
    }
}
