using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] int frameTarget = 60;
    public float playerReach = 10;

    [Header("Text Display")]
    [SerializeField] float textDisplaySpeed = 10;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] GameObject dialogueParent;
    bool displayingText;
    string currentLine;
    [SerializeField] float clickCooldown = 0.5f;
    float clickCountdown = 0.1f;
    int convoIndex;
    List<string> lines;

    //dependencies
    public InventoryScript inventory;
    public GameObject player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetFrameRate();
        EndConversation();
        clickCooldown = 0;
    }

    void SetFrameRate()
    {
        //Application.targetFrameRate = frameTarget;
    }

    public void StartConvo(List<string> _lines)
    {
        lines = _lines;
        //clickCountdown = clickCooldown;
        convoIndex = 0;
        NextLine();
    }

    void NextLine()
    {
        if (convoIndex < lines.Count) {
            SayLine(lines[convoIndex]);
            convoIndex += 1;
            return;
        }
        EndConversation();
    }

    void SayLine(string line)
    {
        dialogueParent.gameObject.SetActive(true);
        mainText.text = "";
        currentLine = line;
        if (displayingText) StopAllCoroutines();
        StartCoroutine(DisplayLine(line));
    }

    public void completeLine()
    {
        if (displayingText) StopAllCoroutines();
        mainText.text = currentLine;
        displayingText = false;
    }

    public void EndConversation() 
    {
        lines = new List<string>();
        completeLine();
        dialogueParent.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
            SceneManager.LoadScene(3, LoadSceneMode.Additive);

            /*for (int i = 0; i < SceneManager.sceneCount; i++) {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
                SceneManager.LoadScene(SceneManager.GetSceneAt(i).name, LoadSceneMode.Additive);
            }*/
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) && !displayingText && lines.Count > 0){
            NextLine();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
            completeLine();
    }

    IEnumerator DisplayLine(string line)
    {
        displayingText = true;
        for (int i = 0; i < line.Length; i++) {
            mainText.text += line[i];
            yield return new WaitForSeconds(textDisplaySpeed * Time.fixedDeltaTime);
        }
        displayingText = false;
    }
}
