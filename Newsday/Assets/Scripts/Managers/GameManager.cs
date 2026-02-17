using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Checklist checklist;
    [SerializeField] private ArticleManager articleManager;
    [SerializeField] private GameObject tempWin, tempLose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tempWin.SetActive(false);
        tempLose.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator displayMessage(GameObject result)
    {
        result.SetActive(true);
        yield return new WaitForSeconds(3f);
        result.SetActive(false);
    }
    
    public void validateResponse()
    {
        if (!compareArticle())
        {
            //Debug.Log("You did something wrong");
            StartCoroutine(displayMessage(tempLose));
        }
        else
        {
            //Debug.Log("You  got it right!!!!");
            StartCoroutine(displayMessage(tempWin));
        }
    }

    public bool compareArticle()
    {
        int[] checklistValues = checklist.getItemValues();
        int[] articleValues = articleManager.getCurrentArticleValues();

        for(int i = 0; i < checklistValues.Length; i++)
        {
            if (checklistValues[i] != articleValues[i]) return false;
        }
        return true;
    }
}
