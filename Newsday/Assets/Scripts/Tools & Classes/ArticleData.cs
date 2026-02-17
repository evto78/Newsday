using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public string BS_bodyText;


    public string boogleSearchReturn() {
        if (date == "") ;
        return "";
    }
}
