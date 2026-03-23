using UnityEngine;

public class SimplePageSystem : MonoBehaviour
{
    public GameObject[] pages;
    private int currentPage = 0;

    void Start()
    {
        ShowPage(0);
    }

    public void Next()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    public void Back()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }
    }
}