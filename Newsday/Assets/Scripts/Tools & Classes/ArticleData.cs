using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "New Article", menuName = "Article/Create New Article")]
[System.Serializable]
public class ArticleData : ScriptableObject
{
    [Header("Content")]
    public string headline = "HEADLINE";
    public Sprite image;
    public string author = "AUTHOR";
    public string date = "2026/01/01";
    [TextArea(6, 10)]
    public string bodyText = "";

    [Header("DATA")]
    public int id = -1;
    public List<MisinfoData> misinfo = new List<MisinfoData>();

    [Header("Apparance")]
    public Color hue = Color.red;

    [Header("Boogle Search Responses")]
    public string BS_headline;
    public Sprite BS_image;
    public string BS_author;
    public string BS_date;
    public string bodyFact;
    public string BS_bodyText;


    public Sprite getBoogleImage()
    {
        return BS_image;
    }
    public string boogleSearchReturn(string search) {
        if (date == search) return BS_date;
        if(headline == search) return BS_headline;
        if(image.name == search) return "-1";
        if (author == search) return BS_author;
        if (bodyFact == search) return BS_bodyText;
        return "";
    }
}
