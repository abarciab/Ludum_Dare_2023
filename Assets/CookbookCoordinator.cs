using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookbookCoordinator : MonoBehaviour
{
    [SerializeField] GameObject nextPage, prevPage;
    [SerializeField] PageObject pageScript;

    [SerializeField] List<ItemObject> Dishes;
    int currentPage;

    private void Start()
    {
        DisplayPage();
    }

    void DisplayPage()
    {
        pageScript.Display(Dishes[currentPage]);
        nextPage.gameObject.SetActive(currentPage < Dishes.Count-1);
        prevPage.gameObject.SetActive(currentPage > 0);
    }

    public void NextPage()
    {
        currentPage += 1;
        DisplayPage();
    }

    public void PrevPage()
    {
        currentPage -= 1;
        DisplayPage();
    }
}
