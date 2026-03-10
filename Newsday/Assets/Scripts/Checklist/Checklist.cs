using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
/***********************************
* Class Name: Checklist
* Last Person Edited: Ryan McBride
* Last Date Edited:Feb 7, 2026
* Description: Checklist that will return the results of the 
************************************/
public class Checklist : MonoBehaviour
{
    [SerializeField] private CheckListItemToggle [] items;
    private int [] result;
    private void Start()
    {
        result = new int[items.Length];
    }
    public int[] getItemValues()
    {
        for(int i = 0; i < result.Length; i++)
        {
            result[i] = items[i].getToggleStatus();
        }
        return result;
    }

    public void displayItemValues()
    {
        string output = "";
        for (int i = 0; i < result.Length; i++)
        {
            output += "item " + i + ": " + items[i].getToggleStatus() + "\n";
        }
        Debug.Log(output);
    }

    public void clearChecklist()
    {

        Transform checklist = transform.Find("Checklist").Find("Elements");
        if (checklist == null) { Debug.LogError("Could not find Checklist child"); return; }
        
        for(int i = 1; i < 7; i++)
        {
            //get the item transform
            Transform item = checklist.Find("Item (" + i + ")");
            if (item == null) { Debug.LogError("Could not find Item (" + i + ")"); continue; }

            //get the yes no compmonents of the item
            Transform yesTransform = item.Find("Yes");
            Transform noTransform = item.Find("No");
            if (yesTransform == null || noTransform == null) { Debug.LogError("Incomplete item in checklist"); continue; }

            //grab the scripts for the corresponding toggles
            Toggle yes = yesTransform.gameObject.GetComponent<Toggle>();
            Toggle no = noTransform.gameObject.GetComponent<Toggle>();

            //set to deactive
            yes.isOn = false;
            no.isOn = false;
        }


    }

}
