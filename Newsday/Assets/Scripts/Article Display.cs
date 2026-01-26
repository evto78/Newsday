using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
    }
    public void LoadArticle(ArticleData inputArticle)
    {
        headline.text = inputArticle.headline;
        date.text = inputArticle.date;
        author.text = inputArticle.author;
        thumbnail.sprite = inputArticle.image;
        bodyText.text = inputArticle.bodyText;

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
}
