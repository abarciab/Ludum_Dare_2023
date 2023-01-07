using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(3);
    }

    public void ViewCredits()
    {
        SceneManager.LoadScene(4);
    }

    public void ViewSettings()
    {
        print("settigns???");
    }
}
