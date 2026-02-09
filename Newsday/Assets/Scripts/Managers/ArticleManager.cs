using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArticleManager : MonoBehaviour
{
    [Header("References")]
    public List<ArticleDisplay> articleWindows;
    [Header("Data")]
    public List<ArticleData> allArticles;
    public ArticleData currentArticle;
    [Header("Debug / Testing")]
    public ArticleData overrideArticle;
    public Image imageClicked;
    public TextMeshProUGUI textClicked;
    private void Awake() { SetUp(); }
    void SetUp()
    {
        allArticles = new List<ArticleData>();
        allArticles.AddRange(Resources.LoadAll<ArticleData>("Articles"));

        if(overrideArticle == null) { currentArticle = allArticles[0]; }
        else { currentArticle = overrideArticle; }

        foreach (ArticleDisplay display in articleWindows) { display.gameObject.SetActive(false); }
        switch (currentArticle.articleLayout)
        {
            case ArticleData.Layout.SmallImage: articleWindows[0].gameObject.SetActive(true); articleWindows[0].LoadArticle(currentArticle, this); break;
            case ArticleData.Layout.LargeImageLeft: articleWindows[1].gameObject.SetActive(true); articleWindows[1].LoadArticle(currentArticle, this); break;
            case ArticleData.Layout.LargeImageRight: articleWindows[2].gameObject.SetActive(true); articleWindows[2].LoadArticle(currentArticle, this); break;
        }
        
    }
    bool hovering; GameObject clickedElement;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsPointerOverUIElement(GetEventSystemRaycastResults()))
        {
            ElementClicked(clickedElement, "");
        }
    }
    public void ElementClicked(GameObject element, string text)
    {
        if (imageClicked == null || textClicked == null) { return; }

        imageClicked.sprite = null; textClicked.text = "";
        if (text != "")
        {
            textClicked.text = text;
        }
        else
        {
            switch (element.name)
            {
                case "Headline":
                    textClicked.text = currentArticle.headline;
                    break;
                case "Date":
                    textClicked.text = currentArticle.date;
                    break;
                case "Author":
                    textClicked.text = currentArticle.author;
                    break;
                case "Image":
                    imageClicked.sprite = currentArticle.image;
                    break;
                case "CloseTab":
                    textClicked.text = "Close Tab";
                    break;
            }
        }
    }
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.tag == "Clickable")
            { clickedElement = curRaysastResult.gameObject; return true; }
        }
        return false;
    }
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
