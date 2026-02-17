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
    //Small Image is 300x300 ratio, Large Image is 650x300 ratio
    //Small can hold ~1300 characters in body text
    //Large can hold ~1100 characters in body text
    public enum Layout { SmallImage, LargeImageLeft, LargeImageRight}
    public Layout articleLayout = Layout.SmallImage;

    public int[] answer = {1, 1, 1, 1, 1, 1};

    
}
