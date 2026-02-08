using UnityEngine;
/***********************************
* Class Name: Checklist Item Toggle
* Last Person Edited: Ryan McBride
* Last Date Edited:Feb 7, 2026
* Description: Toggle manager for the items in the checklist UI
* 0 if the player toggled no
* 1 if the player toggled yes
* 2 if player toggled both
* -1 if player hasn't toggled either one
************************************/

public class CheckListItemToggle : MonoBehaviour
{
    private bool yes, no;
    private void Start()
    {
        yes = false; no = false;
    }

    public void toggleYes()
    {
        yes = !yes;
    }
    public void toggleNo()
    {
        no = !no;
    }

    public int getToggleStatus()
    {
        if (yes && no) return 2;
        if (yes) return 1;
        if (no) return 0;
        return -1;
    }



}
