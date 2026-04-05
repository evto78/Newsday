using UnityEngine;

public class TutorialPageSystem : MonoBehaviour
{
    [System.Serializable]
    public class TutorialPage
    {
        public GameObject page;
        public GameObject[] arrows; // arrows, hand icons, highlight boxes, etc.
    }

    public TutorialPage[] pages;
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
            bool isCurrentPage = (i == index);

            if (pages[i].page != null)
                pages[i].page.SetActive(isCurrentPage);

            if (pages[i].arrows != null)
            {
                foreach (GameObject arrow in pages[i].arrows)
                {
                    if (arrow != null)
                        arrow.SetActive(isCurrentPage);
                }
            }
        }
    }
}