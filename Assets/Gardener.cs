using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gardener : MonoBehaviour
{
    [SerializeField] Plot nearbyCrop;
    [SerializeField] Plant toPlant;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Plot>()) {
            nearbyCrop = collision.GetComponent<Plot>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Plot>()) {
            nearbyCrop = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearbyCrop != null)
            nearbyCrop.Interact(toPlant);
    }
}
