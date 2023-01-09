using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialCoordinator : MonoBehaviour
{
    [System.Serializable]
    public class TutorialData
    {
        public string Main;
        public List<string> details;
    }

    [SerializeField] TextMeshProUGUI main, details1, details2, details3;
    [SerializeField] List<TutorialData> datas;
    [SerializeField] float textDelay = 2;
    [SerializeField] float lastInstructionTime = 10;
    int index;
    [SerializeField] ItemObject tomatoPacket, carrotPacket;

    private void Update()
    {
        if (index == 0 && GameManager.instance.inventory.HasItem("potato")) NextItem();
        if (index == 1 && GameManager.instance.inventory.HasItem("french fries")) NextItem();
        if (index == 2 && GameManager.instance.inventory.money > 0) NextItem();
        if (index >= 3) lastInstructionTime -= Time.deltaTime;
        if (index == 3 && lastInstructionTime <= 0) {
            GameManager.instance.inventory.AddItem(carrotPacket);
            GameManager.instance.inventory.AddItem(tomatoPacket);
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        index = -1;
        NextItem();
    }

    void NextItem()
    {
        index += 1;

        main.text = datas[index].Main;
        details1.text = datas[index].details[0];
        details2.text = datas[index].details[1];
        details3.text = datas[index].details[2];

        details1.gameObject.SetActive(false);
        details2.gameObject.SetActive(false);
        details3.gameObject.SetActive(false);

        ShowTextGradually();
    }

    void ShowTextGradually()
    {
        StartCoroutine(ShowText(details1.gameObject, textDelay));
        StartCoroutine(ShowText(details2.gameObject, textDelay*2));
        StartCoroutine(ShowText(details3.gameObject, textDelay*3));
    }

    IEnumerator ShowText(GameObject obj, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        obj.SetActive(true);
    }

}
