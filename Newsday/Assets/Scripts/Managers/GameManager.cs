using System.Collections;
using UnityEditor.VersionControl;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Checklist checklist;
    [SerializeField] private ArticleManager articleManager;
    [SerializeField] private GameObject tempWin, tempLose;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private CitationMachineManager citationMachineManager;

    private List<int> reasonCodeFound = new List<int>();
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
        tempWin.SetActive(false);
        tempLose.SetActive(false);
        readCSVFile();
    }


    IEnumerator displayMessage(GameObject result)
    {
        result.SetActive(true);
        yield return new WaitForSeconds(3f);
        result.SetActive(false);
    }
    
    public void validateResponse(bool approved)
    {
        if (!compareArticle(approved))
        {
            //Debug.Log("You did something wrong");
            //StartCoroutine(displayMessage(tempLose));
            printCitation();
        }
        else
        {
            //Debug.Log("You  got it right!!!!");
            StartCoroutine(displayMessage(tempWin));
        }
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
    }


    //this checks the player if they submitted a correct article
    public bool compareArticle(bool approved)
    {
        int[] ArticleReasonCodes = articleManager.getArticleReasonCodes();
        int[] checklistValues = checklist.getItemValues();
        int[] articleValues = articleManager.getCurrentArticleValues();

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
