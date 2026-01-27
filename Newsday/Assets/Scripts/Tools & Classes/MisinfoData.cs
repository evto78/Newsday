using UnityEngine;

[System.Serializable]
public class MisinfoData
{
    //article variables
    public enum Location { Headline, Author, Date, Image, Text }
    public Location location = Location.Text;
    [Header("If Location is Text:")]
    public string inArticleText = "NA";
    public string linkID;

    //confirmation
    public enum Type { truthful, parody, misleading, falseContext, stolen, manipulated, fabricated }
    public Type type = Type.truthful;
    public bool[] checklist = new bool[5];

    public enum RejectionReason {Skipped, Confirmed, Missing, Manipulated, Fabricated, FakeAuthor}
    public RejectionReason [] checkListItems = new RejectionReason[5];
    public RejectionReason[] answerKey = new RejectionReason[5];

    public MisinfoData()
    {
        //initalize the list
        for(int i = 0; i< checkListItems.Length; i++)
        {
            checkListItems[i] = RejectionReason.Skipped;
        }

        if (type == Type.truthful)
        {
            for (int i = 0; i < checkListItems.Length; i++)
            {
                answerKey[i] = RejectionReason.Confirmed;
            }
        }
    }

    //needs to know what is wrong with the document
    public bool checkValidity()
    {
        for (int i = 0; i < checklist.Length; i++)
        {
            if (checkListItems[i] == answerKey[i]) continue;//if they match then we continue on

            //TO BE IMPLEMENTED
            // - creating a citation aid that adds the reason code

            return false;
        }
        return true;
    }
}
