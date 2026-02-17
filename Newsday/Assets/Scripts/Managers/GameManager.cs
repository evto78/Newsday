using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Checklist checklist;
    [SerializeField] private ArticleManager articleManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void validateResponse()
    {
        if (!compareArticle())
        {
            Debug.Log("You did something wrong");
        }
        else
        {
            Debug.Log("You  got it right!!!!");
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
