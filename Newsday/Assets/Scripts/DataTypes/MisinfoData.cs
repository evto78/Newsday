using UnityEngine;

[System.Serializable]
public class MisinfoData
{
    public enum Location { Headline, Author, Date, Image, Text }
    public Location location = Location.Text;
    public enum Type { truthful, parody, misleading, falseContext, stolen, manipulated, fabricated }
    public Type type = Type.truthful;
    [Header("If Location is Text:")]
    public string inArticleText = "NA";
}
