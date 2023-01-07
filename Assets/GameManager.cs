using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] int frameTarget = 60;

    [Header("Text Display")]
    [SerializeField] float textDisplaySpeed = 10;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] GameObject dialogueParent;
    bool displayingText;
    string currentLine;

    private void Start()
    {
        SetFrameRate();
    }

    private void OnValidate()
    {
        if (Application.isPlaying) SetFrameRate();
    }

    void SetFrameRate()
    {
        Application.targetFrameRate = frameTarget;
    }

    public void SayLine(string line)
    {
        dialogueParent.gameObject.SetActive(true);
        mainText.text = "";
        StopAllCoroutines();
        StartCoroutine(DisplayLine(line));
    }

    void completeLine()
    {
        StopAllCoroutines();
        mainText.text = currentLine;
        displayingText = false;
    }

    public void EndConversation() 
    {
        completeLine();
        dialogueParent.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) 
            completeLine();
    }

    IEnumerator DisplayLine(string line)
    {
        displayingText = true;
        for (int i = 0; i < line.Length; i++) {
            mainText.text += line[i];
            yield return new WaitForSeconds(textDisplaySpeed * Time.deltaTime);
        }
        displayingText = false;
    }
}
