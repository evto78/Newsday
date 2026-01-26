using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArticleManager : MonoBehaviour
{
    [Header("References")]
    public ArticleDisplay articleWindow;
    [Header("Data")]
    public List<ArticleData> allArticles;
    public ArticleData currentArticle;
    [Header("Debug / Testing")]
    public ArticleData overrideArticle;
    private void Awake() { SetUp(); }
    void SetUp()
    {
        allArticles = new List<ArticleData>();
        allArticles.AddRange(Resources.LoadAll<ArticleData>("Articles"));

        if(overrideArticle == null) { currentArticle = allArticles[0]; }
        else { currentArticle = overrideArticle; }

        articleWindow.LoadArticle(currentArticle);
    }
}
