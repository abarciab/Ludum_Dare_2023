using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

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
    public float wateringTime = 2f;
    int convoIndex;
    List<string> lines;
    Resident final;

    [Header("Daily Cycle")]
    [SerializeField] GameObject nightSky;
    [HideInInspector] public List<Resident> residents = new List<Resident>();
    public static Action DiningEvent;
    [SerializeField] float diningInterval;
    float diningCountdown;
    [SerializeField] GameObject winScreen;
    
    bool readyToAdvanceDay;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI hungryResidentsDisplay;
    [SerializeField] TextMeshProUGUI gainMoney;
    [SerializeField] TextMeshProUGUI loseMoney;

    [HideInInspector] public Van vanScript;

    //dependencies
    public InventoryScript inventory;
    public GameObject player, invParent;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AudioManager.instance.PlayMusic(6);
        diningCountdown = diningInterval;
        DiningEvent = null;
        EndConversation();
    }

    public int NumHungryResidents()
    {
        int num = 0;
        for (int i = 0; i < residents.Count; i++) {
            if (residents[i].hungry) num ++;
        }
        return num;
    }

    public void StartConvo(List<string> _lines, Resident speaker = null)
    {
        if (speaker != null) final = speaker;
        lines = _lines;
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

    public void DeliverItem(ItemObject item)
    {
        if (vanScript != null) vanScript.DropOffItem(item);
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
        if (final != null) final.Die();
        else if (readyToAdvanceDay) AdvanceDay();
    }

    public void AdvanceIfReadty()
    {
        if (readyToAdvanceDay) AdvanceDay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.E)) invParent.SetActive(!invParent.activeInHierarchy);
        if (Input.GetKeyDown(KeyCode.R)) RestartScene();

        ProcessDialogue();
        UpdateHungryDisplay();
        ProcesResidentHunger();

        diningCountdown -= Time.deltaTime;
        if (diningCountdown <= 0 & DiningEvent != null) DiningEvent.Invoke();
    }

    void ProcesResidentHunger()
    {
        if (NumHungryResidents() == 0 && !nightSky.activeInHierarchy) {
            if (lines.Count > 0)
                readyToAdvanceDay = true;
            else AdvanceDay();
        }
        if (residents.Count == 0) EndGame();
    }

    public void GainMoney(int amount)
    {
        AudioManager.instance.PlayGlobal(3);
        inventory.money += amount;
        gainMoney.text = "+ " + amount;
        gainMoney.gameObject.SetActive(true);
    }

    public void LoseMoney(int amount)
    {
        inventory.money -= Mathf.Abs(amount);
        loseMoney.text = "- " + amount;
        loseMoney.gameObject.SetActive(true);
    }

    void UpdateHungryDisplay()
    {
        if (nightSky.activeInHierarchy) {
            hungryResidentsDisplay.text = "Residents to Feed: 0";
            return;
        }
        hungryResidentsDisplay.text = "Residents to Feed: " + NumHungryResidents();
    }

    void EndGame()
    {
        winScreen.SetActive(true);
        player.gameObject.SetActive(false);
    }

    void AdvanceDay()
    {
        AudioManager.instance.PlayGlobal(7);
        readyToAdvanceDay = false;
        nightSky.SetActive(true);
        for (int i = 0; i < residents.Count; i++) {
            residents[i].hungry = true;
        }
        diningCountdown = diningInterval;
    }

    void ProcessDialogue()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) && !displayingText && lines.Count > 0) {
            NextLine();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
            completeLine();
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(4);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(0);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
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
