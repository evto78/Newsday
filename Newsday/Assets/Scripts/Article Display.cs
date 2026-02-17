using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArticleDisplay : MonoBehaviour
{
    List<Vector2> articleSV;
    [Header("References")]
    public List<TextMeshProUGUI> textElements;
    public List<Image> imageElements;
    public TextMeshProUGUI headline;
    public TextMeshProUGUI date;
    public TextMeshProUGUI author;
    public Image thumbnail;
    public TextMeshProUGUI bodyText;
    public Image bg;
    CursorManager cursorManager;
    ArticleManager articleManager;
    private void Awake()
    {
        articleSV = new List<Vector2>();
        float H; float S; float V;
        for(int i = 0; i < textElements.Count+imageElements.Count; i++)
        {
            if(i < textElements.Count)
            {
                Color.RGBToHSV(textElements[i].color, out H, out S, out V);
            }
            else
            {
                Color.RGBToHSV(imageElements[i-textElements.Count].color, out H, out S, out V);
            }
            articleSV.Add(new Vector2(S,V));
        }

        cursorManager = GameObject.Find("CursorManager").GetComponent<CursorManager>();
    }
    public void LoadArticle(ArticleData inputArticle, ArticleManager manager)
    {
        articleManager = manager;

        headline.text = inputArticle.headline;
        date.text = inputArticle.date;
        author.text = inputArticle.author;
        thumbnail.sprite = inputArticle.image;
        bodyText.text = inputArticle.bodyText;
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
    }
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
    }
    private void HoverInlineText(string keyword, TMP_Text tmp)
    {
        //Debug.Log("Hovering over : " + keyword);
        if(cursorManager == null) { return; }
        cursorManager.ChangeCursor(1);
    }
    private void StopHoverInlineText(string keyword, TMP_Text tmp)
    {
        //Debug.Log("Stopped hovering over : " + keyword);
        if(cursorManager == null) { return; }
        cursorManager.ChangeCursor(0);
    }
}
