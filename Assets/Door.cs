using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class Door : MonoBehaviour
{
    [SerializeField] bool setOpen;
    [SerializeField] bool setClosed;
    [SerializeField] Quaternion openRot;
    [SerializeField] Vector3 openPos;
    [SerializeField] Quaternion closeRot;
    [SerializeField] Vector3 closePos;
    bool open;
    GameObject player;

    [SerializeField] bool openTest;
    [SerializeField] bool closeTest;

    AudioSource source;

    private void Start()
    {
        Setup();   
    }

    void Setup()
    {
        if (GetComponent<AudioManager>() == null) gameObject.AddComponent<AudioSource>();
        source = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        
        if (openTest) {
            openTest = false;
            Open();
        }
        if (closeTest) {
            closeTest = false;
            Close();
        }

        if (setOpen) {
            setOpen = false;
            openRot = transform.rotation;
            openPos = transform.position;
        }
        if (setClosed) {
            setClosed = false;
            closeRot = transform.rotation;
            closePos = transform.position;
        }

        if (open && player != null && Vector2.Distance(transform.position, player.transform.position) > 3.5f) Close();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() || collision.GetComponent<Resident>()) {
            player = collision.gameObject;
            Open();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>()) {
            //Close();
        }
    }

    void Close()
    {
        if (source == null) Setup();
        if (open) AudioManager.instance.PlayHere(5, source);

        transform.rotation = closeRot;
        transform.position = closePos;
        open = false;
    }

    void Open()
    {
        if (source == null) Setup();
        if (!open) AudioManager.instance.PlayHere(5, source);

        transform.rotation = openRot;
        transform.position = openPos;
        open = true;
    }

}

