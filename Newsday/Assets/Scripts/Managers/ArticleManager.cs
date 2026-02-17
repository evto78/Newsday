using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArticleManager : MonoBehaviour
{
    [Header("References")]
    public List<ArticleDisplay> articleWindows; //All layouts of the articles, 0 is small, 1 is large left, 2 is large right
    [Header("Data")]
    public List<ArticleData> allArticles; //A list of all of the article objects in the resources folder
    public List<ArticleData> allValidArticles; //All articles with an id above -1, sorted by id
    public ArticleData currentArticle;
    [Header("Debug / Testing")]
    public ArticleData overrideArticle; //Ignore proper behavior and instead load this article
    public Image imageClicked; //Image to change to the last image clicked
    public TextMeshProUGUI textClicked; //Text to change to the last text clicked
    private void Awake() { SetUp(); }
    void SetUp()
    {
        //Grab all articles from the resources folder
        allArticles = new List<ArticleData>();
        allArticles.AddRange(Resources.LoadAll<ArticleData>("Articles"));

        //Parse all articles to find all valid articles
        allValidArticles = new List<ArticleData>();
        foreach(ArticleData article in allArticles)
        {
            if (article.id > -1) { allValidArticles.Add(article); }
        }

        //Sort all valid articles to be in order of id
        List<int> indexList = new List<int>();
        List<ArticleData> sortedList = new List<ArticleData>();
        foreach (ArticleData article in allValidArticles) { indexList.Add(article.id); }
        int index = 0; foreach (ArticleData article in allValidArticles) { sortedList.Add(allValidArticles[indexList.IndexOf(index)]); index++; }
        allValidArticles = sortedList;

        //If there is a override article, make the current article that override, otherwise select the first article
        if(overrideArticle == null) { currentArticle = allValidArticles[0]; }
        else { currentArticle = overrideArticle; }

        //Set the article layout to be the correct shape according to the current article
        foreach (ArticleDisplay display in articleWindows) { display.gameObject.SetActive(false); }
        switch (currentArticle.articleLayout)
        {
            case ArticleData.Layout.SmallImage: articleWindows[0].gameObject.SetActive(true); articleWindows[0].LoadArticle(currentArticle, this); break;
            case ArticleData.Layout.LargeImageLeft: articleWindows[1].gameObject.SetActive(true); articleWindows[1].LoadArticle(currentArticle, this); break;
            case ArticleData.Layout.LargeImageRight: articleWindows[2].gameObject.SetActive(true); articleWindows[2].LoadArticle(currentArticle, this); break;
        }
        
    }
    public void NextArticle()
    {
        if (currentArticle.id == allValidArticles.Count - 1) { return; }
        currentArticle = allValidArticles[currentArticle.id + 1];

        //Set the article layout to be the correct shape according to the current article
        foreach (ArticleDisplay display in articleWindows) { display.gameObject.SetActive(false); }
        switch (currentArticle.articleLayout)
        {
            case ArticleData.Layout.SmallImage: articleWindows[0].gameObject.SetActive(true); articleWindows[0].LoadArticle(currentArticle, this); break;
            case ArticleData.Layout.LargeImageLeft: articleWindows[1].gameObject.SetActive(true); articleWindows[1].LoadArticle(currentArticle, this); break;
            case ArticleData.Layout.LargeImageRight: articleWindows[2].gameObject.SetActive(true); articleWindows[2].LoadArticle(currentArticle, this); break;
        }
    }
    public void PreviousArticle()
    {
        if (currentArticle.id == 0) { return; }
        currentArticle = allValidArticles[currentArticle.id - 1];

        //Set the article layout to be the correct shape according to the current article
        foreach (ArticleDisplay display in articleWindows) { display.gameObject.SetActive(false); }
        switch (currentArticle.articleLayout)
        {
            case ArticleData.Layout.SmallImage: articleWindows[0].gameObject.SetActive(true); articleWindows[0].LoadArticle(currentArticle, this); break;
            case ArticleData.Layout.LargeImageLeft: articleWindows[1].gameObject.SetActive(true); articleWindows[1].LoadArticle(currentArticle, this); break;
            case ArticleData.Layout.LargeImageRight: articleWindows[2].gameObject.SetActive(true); articleWindows[2].LoadArticle(currentArticle, this); break;
        }
    }
    public void SetArticle(int id)
    {
        currentArticle = allValidArticles[id];

        //Set the article layout to be the correct shape according to the current article
        foreach (ArticleDisplay display in articleWindows) { display.gameObject.SetActive(false); }
        switch (currentArticle.articleLayout)
        {
            case ArticleData.Layout.SmallImage: articleWindows[0].gameObject.SetActive(true); articleWindows[0].LoadArticle(currentArticle, this); break;
            case ArticleData.Layout.LargeImageLeft: articleWindows[1].gameObject.SetActive(true); articleWindows[1].LoadArticle(currentArticle, this); break;
            case ArticleData.Layout.LargeImageRight: articleWindows[2].gameObject.SetActive(true); articleWindows[2].LoadArticle(currentArticle, this); break;
        }
    }
    GameObject clickedElement;
    private void Update()
    {
        //If a clickable component of the article is clicked...
        if (Input.GetMouseButtonDown(0) && IsPointerOverUIElement(GetEventSystemRaycastResults()))
        {
            ElementClicked(clickedElement, "");
        }
    }
    public void ElementClicked(GameObject element, string text)
    {
        //When something is clicked, set the image to the clicked image, and set the text to the clicked text
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
    //Check if the cursor is over a ui element with the clickable tag
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

    public int[] getCurrentArticleValues()
    {
        return currentArticle.answer;
    }

}
