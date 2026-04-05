using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int day = 1;

    [SerializeField] private Checklist checklist;
    [SerializeField] private GameObject nextDayMessage;
    [SerializeField] private GameObject subwayButton;

    [Header("Managers")]
    [SerializeField] private ArticleManager articleManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private CitationMachineManager citationMachineManager;
    [SerializeField] private SubwaySocialMediaManager socialMediaManager;
    
    private OfficeManager officeManager;
    private ClockTimer timer;

    [Header("Article Related")]
    [SerializeField] private int articlesPerDay = 5;
    private int articlesDoneToday = 0;

    private List<int> reasonCodeFound;
    private static string reasonCodeCSVpath = "/Resources/CSV/ReasonCodeCSV.csv";
    private static int [] reasonCodes;
    private static string [] reasonResponse;
    private void readCSVFile()
    {
        string [] allLines = File.ReadAllLines(Application.dataPath + reasonCodeCSVpath);

        //add all the reason codes to the array and turn them into int
        string[] temp = allLines[0].Split(',');
        reasonCodes = new int [temp.Length];
        int i = 0;
        foreach (string s in temp)
        {
            //Debug.Log(s);
            reasonCodes[i] = int.Parse(s);
            i++;
        }

        //sort all the responses to be the same
        reasonResponse = allLines[1].Split(',');
    }

    private int getReasonCodeIndex(int reasonCode)
    {
        for (int i = 0; i < reasonCodes.Length; i++)
        {
            if (reasonCodes[i] == reasonCode) return i;
        }
        return -1;
    }
    
    void Start()
    {
        nextDayMessage.SetActive(false);
        subwayButton.SetActive(false);
        readCSVFile();

        //get the office manager thats attached to this object
        officeManager = GetComponent<OfficeManager>();

        timer = GetComponent<ClockTimer>();
    }

    IEnumerator displayMessage(GameObject result)
    {
        result.SetActive(true);
        yield return new WaitForSeconds(3f);
        result.SetActive(false);
    }
    
    public void validateResponse(bool approved)
    {
        //jumps back to the office scene
        cameraManager.jumpToScene(0);
        
        if (!compareArticle(approved))
        {
            //Debug.Log("You did something wrong");
            //StartCoroutine(displayMessage(tempLose));
            printCitation();
        }
        else
        {
            //Debug.Log("You  got it right!!!!");
            //StartCoroutine(displayMessage(tempWin));
        }

        //public void reporter leaves
        officeManager.reporterLeave();

        //Social Media Reaction, que the appropiete messeges
        ArticleData publishedArticle = articleManager.currentArticle;
        if (approved && publishedArticle.messegesIfApproved.Count > 0) { socialMediaManager.quedMesseges.AddRange(publishedArticle.messegesIfApproved); }
        else if (!approved && publishedArticle.messegesIfDenied.Count > 0) { socialMediaManager.quedMesseges.AddRange(publishedArticle.messegesIfDenied); }
        
        //check to see if that was their last article
        articlesDoneToday++;
        if (articlesDoneToday == articlesPerDay) subwayButton.SetActive(true);
    }

    //shows the new day message and preps all the variables for the next day...
    public void nextDay()
    {

        //resets the articles so that the day get begin anew. 
        day++;
        articlesDoneToday = 0;
        //sets and displayes message
        nextDayMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Day " + day;
        StartCoroutine(displayMessage(nextDayMessage));
        
        //move back to the first scene
        cameraManager.jumpToScene(0);

        //turn off subway button
        subwayButton.SetActive(false);

        //turn off any citation that are active
        citationMachineManager.turnOffCitations();
    }


    private void printCitation()
    {
        string citationBody =  "";
        foreach(int code in reasonCodeFound)
        {
            citationBody += " - ";
            citationBody += reasonResponse[getReasonCodeIndex(code)];
            citationBody += "\n";
        }
        citationMachineManager.printCitation(citationBody);

        //clear the current checklist
        checklist.clearChecklist();
    }

    
    //this checks the player if they submitted a correct article
    public bool compareArticle(bool approved)
    {
        int[] ArticleReasonCodes = articleManager.getArticleReasonCodes();
        int[] checklistValues = checklist.getItemValues();
        int[] articleValues = articleManager.getCurrentArticleValues();
        reasonCodeFound = new List<int>();
        //check to see if they filled out the checklist correctly
        for(int i = 0; i < checklistValues.Length; i++)
        {
            int checkListValue = checklistValues[i];
            if (checkListValue == 2 || checkListValue == -1) { reasonCodeFound.Add(1); break; }
        }
        //BASE CASES
        //if the articleReasoncode is empty or the first element is 0 then there nothing wrong with the article.
        if (ArticleReasonCodes.Length == 0)//|| ArticleReasonCodes[0] == 0)
        {
            
            for(int i = 0; i < checklistValues.Length; i++)//checks to see if they filled out the checklist correctly
            {
                if (checklistValues[i] == 0) { reasonCodeFound.Add(3); break;}
            }
            if (!approved) reasonCodeFound.Add(2);//check to see if they rejected it.

            //all of the items are 1 which mean that they selected yes and is correct
            if (reasonCodeFound.Count == 0) return true;
            return false;
        }

        List<int> itemLeftToCheck = new List<int>{1, 2, 3, 4, 5};
        
        //if there is something wrong
        foreach (int reasonCode in ArticleReasonCodes)
        {
            int item, misinfo;
            item = (reasonCode / 100);//get the first digit
            misinfo = (reasonCode - (item * 100)) / 10;//get the second digit
                     
            if (misinfo == 1)//if the reason code is related to misinformation
            {
                if(checklistValues[checklistValues.Length - 1] == 1) //if they marked yes to misinformation (meaning that they missed it)
                reasonCodeFound.Add(reasonCode);
                continue;//move onto the next reason code
            }

            // we look at the item the reason code is associated with
            if (checklistValues[item - 1] == 1) //if the player marked it as a yes when it should be a no
            {//, add the reason code
                reasonCodeFound.Add(reasonCode);
                itemLeftToCheck.Remove(item);//removed from the items that have been check
            }
        }   

        for(int i = 0; i < itemLeftToCheck.Count; i++)
        {
             if (checklistValues[i] == 0) { reasonCodeFound.Add(3); break;}
        }

        //need to check the other items that weren't flagged
        //after all of that if we don't have any reason codes then they should have it done correctly which in this case is true
        if (reasonCodeFound.Count == 0) return true;
        return false;

    }
}
