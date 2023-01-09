using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 10;
    [SerializeField] float friction = 0.025f;

    //depdendencies
    Rigidbody2D rb;
    AudioSource source;
    float originalVolume;
    bool fading;
    Animator animator;
    bool up;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();   
        source = GetComponent<AudioSource>();
        originalVolume = source.volume;
    }

    void Update()
    {
        animator.SetBool("Up", up);
        HandleInput();
    }

    void HandleInput()
    {
        var speed = new Vector2();

        if (Input.GetKey(KeyCode.W)) {
            speed += (Vector2.up * walkSpeed);
            up = true;
        }
        else if (Input.GetKey(KeyCode.S)) {
            speed += (Vector2.down * walkSpeed);
            up = false;
        }
        if (Input.GetKey(KeyCode.A))
            speed += (Vector2.left * walkSpeed);
        else if (Input.GetKey(KeyCode.D))
            speed += (Vector2.right * walkSpeed);   

        rb.velocity = Vector2.Lerp(rb.velocity, speed, friction);
        if (speed != new Vector2()) {
            PlayFootStepSound();
            animator.SetBool("walking", true);
        }
        else { 
            StopFoodSteps();
            animator.SetBool("walking", false);
        }
    }

    void StopFoodSteps()
    {
        if (fading) return;
        StartCoroutine(FadeFootSteps());
    }

    IEnumerator FadeFootSteps()
    {
        fading = true;
        for (int i = 0; i < 20; i++) {
            source.volume = Mathf.Lerp(source.volume, 0, 0.5f);
            yield return new WaitForEndOfFrame();
        }
        fading = false;
        source.Pause();
    }

    void PlayFootStepSound()
    {
        StopAllCoroutines();
        fading = false;
        if (source.isPlaying) return;
        source.volume = originalVolume;
        source.Play();
    }
}
