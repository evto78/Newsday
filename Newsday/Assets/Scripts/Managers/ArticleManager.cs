using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    [SerializeField] private BoogleManager boogleManager;   

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

        setupArticle();
    }

    public void NextArticle()
    {
        if (currentArticle.id == allValidArticles.Count - 1) { return; }
        currentArticle = allValidArticles[currentArticle.id + 1];

        setupArticle();
    }

    public void PreviousArticle()
    {
        if (currentArticle.id == 0) { return; }
        currentArticle = allValidArticles[currentArticle.id - 1];

        setupArticle();
        
    }

    public void setupArticle()
    {
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

        setupArticle();
    }

    GameObject clickedElement;
    private void Update()
    {
        //If a clickable component of the article is clicked...
        if (Input.GetMouseButtonDown(0) && IsPointerOverUIElement(GetEventSystemRaycastResults()))
        {
            ElementClicked(clickedElement, "");
            //gets the text to display in the boogle search bar
            boogleManager.updateSearchBarText(getLastClicked());

            //preps the results
            updateBoogleSearchResult();
            
        }
    }

    public void updateBoogleSearchResult()
    {

        if (currentArticle.boogleSearchReturn(getLastClicked()) == "-1")
        {//if its an image then select the image result
            boogleManager.imageResult(currentArticle.getBoogleImage());

        }
        else
        {
            boogleManager.textResult(currentArticle.boogleSearchReturn(getLastClicked()));
        }
    }

    public string getLastClicked()
    {
        if (textWasLastClicked) return textClicked.text;
        return imageClicked.sprite.name;
    }

    private bool textWasLastClicked = false;
    
    public void ElementClicked(GameObject element, string text)
    {
        imageClicked.sprite = null; textClicked.text = "";
        if(text != "")
        {
            textClicked.text = text;
            textWasLastClicked = true;
            boogleManager.updateSearchBarText(getLastClicked());

            //preps the results
            updateBoogleSearchResult();
            return;
        }
        switch (element.name)
            {
                case "Headline":
                    textClicked.text = currentArticle.headline;
                    textWasLastClicked = true;
                    break;
                case "Date":
                    textClicked.text = currentArticle.date;
                    textWasLastClicked = true;
                    break;
                case "Author":
                    textClicked.text = currentArticle.author;
                    textWasLastClicked = true;
                    break;
                case "Image":
                    imageClicked.sprite = currentArticle.image;
                    textWasLastClicked = false;
                    break;
                case "CloseTab":
                    textClicked.text = "Close Tab";
                    break;
            } 
    }
    //Check if the cursor is over a ui element with the clickable tag
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.tag == "Clickable")//checks to see if the tag is clickable
            { 
                clickedElement = curRaysastResult.gameObject; 
                return true; 
            }
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
