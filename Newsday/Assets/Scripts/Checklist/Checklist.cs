using JetBrains.Annotations;
using UnityEngine;
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

}
