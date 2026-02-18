using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ArticleDisplay : MonoBehaviour
{
    //The saturation and value of various color objects in the articles
    List<Vector2> articleSV = new List<Vector2>();
    [Header("References")]
    //All text elements of the article
    public List<TextMeshProUGUI> textElements;
    //All image elements of the article
    public List<Image> imageElements;
    public TextMeshProUGUI headline;
    public TextMeshProUGUI date;
    public TextMeshProUGUI author;
    public Image thumbnail;
    public TextMeshProUGUI bodyText;
    public Image bg;
    CursorManager cursorManager;
    ArticleManager articleManager;

    //Used to highlight the text when it is being hovered over
    string baseBodyText;
    List<string> bodyTextHighlightVariations;
    List<string> bodyTextHighlightVariationsTargetStringIndex;
    string currentBodyTextVariation;
    string selectedText;
    bool hovering;
    string highlightHexColor = "ffff00bb"; // The color of the highlight, written in hex. This is the value for yellow RGBA=(255,255,0,255)
    string semiHighlightHexColor = "ffff0033"; // A lower aplha of the previous highlight.

    private void Awake()
    {
        SetUp();
    }
    void SetUp()
    {
        //Get initial color values of the article parts so it can be changed later
        articleSV = new List<Vector2>();
        float H; float S; float V;
        for (int i = 0; i < textElements.Count + imageElements.Count; i++)
        {
            if (i < textElements.Count)
            {
                Color.RGBToHSV(textElements[i].color, out H, out S, out V);
            }
            else
            {
                Color.RGBToHSV(imageElements[i - textElements.Count].color, out H, out S, out V);
            }
            articleSV.Add(new Vector2(S, V));
        }

        cursorManager = GameObject.Find("CursorManager").GetComponent<CursorManager>();
    }
    public void LoadArticle(ArticleData inputArticle, ArticleManager manager)
    {
        //Called by article manager to load up the next article
        if (articleSV.Count < 1) { SetUp(); }

        articleManager = manager;

        headline.text = inputArticle.headline;
        date.text = inputArticle.date;
        author.text = inputArticle.author;
        thumbnail.sprite = inputArticle.image;
        bodyText.text = inputArticle.bodyText;
        //Currently useless loop for processing misinfo data
        foreach(MisinfoData misinfo in inputArticle.misinfo)
        {
            switch (misinfo.location)
            {
                case MisinfoData.Location.Text:
                    break;
                default:
                    break;
            }
        }
        //Change the color of the article to match articles hue
        Color.RGBToHSV(inputArticle.hue, out float H, out float S, out float V);
        for (int i = 0; i < textElements.Count + imageElements.Count; i++)
        {
            if (i < textElements.Count)
            {
                textElements[i].color = Color.HSVToRGB(H, articleSV[i].x, articleSV[i].y);
            }
            else
            {
                imageElements[i-textElements.Count].color = Color.HSVToRGB(H, articleSV[i].x, articleSV[i].y);
            }
        }
        //Prep highlight data
        baseBodyText = inputArticle.bodyText;
        currentBodyTextVariation = baseBodyText;
        FindLinksAndGenerateVariations();
    }
    void FindLinksAndGenerateVariations()
    {
        //Scrubs the body text to find where the linked text is, and stores it in these lists as a variation with that section highlighted
        bodyTextHighlightVariations = new List<string>();
        bodyTextHighlightVariationsTargetStringIndex = new List<string>();
        foreach(MisinfoData misinfo in articleManager.currentArticle.misinfo)
        {
            if (misinfo.location == MisinfoData.Location.Text) 
            {
                string compareText = misinfo.inArticleText;
                int startIndex = 0; int endIndex = 0;
                int correctCount = 0;
                for(int i = 0; i < baseBodyText.Length; i++)
                {
                    if (correctCount == compareText.Length-1)
                    {
                        endIndex = i;
                        break;
                    }
                    else if (baseBodyText[i] == compareText[correctCount])
                    {
                        if (correctCount == 0) { startIndex = i; }
                        correctCount++;
                    }
                    else
                    {
                        correctCount = 0;
                    }
                }
                //SemiHighlight
                string semiVariation = baseBodyText;
                semiVariation = semiVariation.Insert(startIndex,"<mark=#" + semiHighlightHexColor + ">");
                semiVariation = semiVariation.Insert(endIndex + 17,"</mark>");
                bodyTextHighlightVariations.Add(semiVariation);
                bodyTextHighlightVariationsTargetStringIndex.Add("semi"+misinfo.inArticleText);
                //Highlight
                string fullVariation = baseBodyText;
                fullVariation = fullVariation.Insert(startIndex, "<mark=#" + highlightHexColor + ">");
                fullVariation = fullVariation.Insert(endIndex + 17, "</mark>");
                bodyTextHighlightVariations.Add(fullVariation);
                bodyTextHighlightVariationsTargetStringIndex.Add("full"+misinfo.inArticleText);
            }
        }
    }
    private void Update()
    {
        //Highlight clicked text
        if (!hovering)
        {
            if (selectedText != "")
            {
                int index = bodyTextHighlightVariationsTargetStringIndex.IndexOf("full"+selectedText);
                if (index != -1)
                {
                    currentBodyTextVariation = bodyTextHighlightVariations[index];
                }
                else
                {
                    currentBodyTextVariation = baseBodyText;
                }
            }
            else
            {
                currentBodyTextVariation = baseBodyText;
            }
        }

        bodyText.text = currentBodyTextVariation;
    }
    // varios methods and functions for handleing clickable text:
    private void OnEnable()
    {
        TMPLinkHandler.OnClickedOnLinkEvent += ClickInlineText;
        TMPLinkHandler.OnHoverOverLinkEvent += HoverInlineText;
        TMPLinkHandler.OnStopHoverOverLinkEvent += StopHoverInlineText;
    }
    private void OnDisable()
    {
        TMPLinkHandler.OnClickedOnLinkEvent -= ClickInlineText;
        TMPLinkHandler.OnHoverOverLinkEvent -= HoverInlineText;
        TMPLinkHandler.OnStopHoverOverLinkEvent -= StopHoverInlineText;
    }
    private void ClickInlineText(string keyword, TMP_Text tmp)
    {
        //Debug.Log("Clicked : " + keyword);
        articleManager.ElementClicked(tmp.gameObject, keyword);

        //Highlight clicked text
        int index = bodyTextHighlightVariationsTargetStringIndex.IndexOf("full"+keyword);
        if (index != -1)
        {
            currentBodyTextVariation = bodyTextHighlightVariations[index];
            selectedText = keyword;
        }
    }
    private void HoverInlineText(string keyword, TMP_Text tmp)
    {
        //Debug.Log("Hovering over : " + keyword);
        if(cursorManager == null) { return; }
        cursorManager.ChangeCursor(1);

        //Highlight clicked text
        int index = bodyTextHighlightVariationsTargetStringIndex.IndexOf("semi"+keyword);
        if (index != -1)
        {
            if (selectedText != keyword)
            {
                currentBodyTextVariation = bodyTextHighlightVariations[index];
            }
        }
        hovering = true;
    }
    private void StopHoverInlineText(string keyword, TMP_Text tmp)
    {
        //Debug.Log("Stopped hovering over : " + keyword);
        if(cursorManager == null) { return; }
        cursorManager.ChangeCursor(0);

        hovering = false;
    }
}
