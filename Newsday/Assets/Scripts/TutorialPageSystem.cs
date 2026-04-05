using UnityEngine;
using UnityEngine.UI;

public class TutorialPageSystem : MonoBehaviour
{
    [System.Serializable]
    public class TutorialPage
    {
        public GameObject page;
        public GameObject[] arrows;
    }

    public TutorialPage[] pages;
    public bool showArrows = true;
    public Toggle arrowToggle;

    private int currentPage = 0;

    void Start()
    {
        if (arrowToggle != null)
        {
            arrowToggle.isOn = showArrows;
            arrowToggle.onValueChanged.AddListener(SetArrows);
        }

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

    public void SetArrows(bool value)
    {
        showArrows = value;
        RefreshArrows();
    }

    void ShowPage(int index)
    {
        currentPage = index;

        for (int i = 0; i < pages.Length; i++)
        {
            bool isCurrent = (i == index);

            if (pages[i].page != null)
                pages[i].page.SetActive(isCurrent);
        }

        RefreshArrows();
    }

    void RefreshArrows()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            bool isCurrent = (i == currentPage);

            if (pages[i].arrows != null)
            {
                foreach (GameObject arrow in pages[i].arrows)
                {
                    if (arrow != null)
                        arrow.SetActive(isCurrent && showArrows);
                }
            }
        }
    }
}