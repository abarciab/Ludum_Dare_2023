using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] AudioSource buttonAudioSource;
    [SerializeField] AudioClip hoverSound;
    [SerializeField] AudioClip clickSound;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
        SceneManager.LoadScene(4, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(0);
    }

    public void ViewCredits()
    {
        SceneManager.LoadScene(5);
    }


    public void PlayHoverSound()
    {
        buttonAudioSource.clip = hoverSound;
        buttonAudioSource.Play();
    }
    public void PlayClickSound()
    {
        buttonAudioSource.clip = clickSound;
        buttonAudioSource.Play();
    }
}
