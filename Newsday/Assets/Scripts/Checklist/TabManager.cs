using UnityEngine;
/***********************************
* Class Name: Checklist
* Last Person Edited: Ryan McBride
* Last Date Edited:Feb 8, 2026
* Description: switches the tabs of the UI guide 
************************************/
public class TabManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    public int activeTab = 0;
    void Start()
    {
        setActiveTab(activeTab);
    }
    private void setActiveTab(int tab)
    {
        switch (tab)
        {
            case 0:
                checkListTab(); break;  
                case 1:
                tutorialTab(); break;
        }
    }

    public void checkListTab()
    {
        turnOffAllTabs();
        tabs[0].SetActive(true);
        activeTab = 0;
    }

    public void tutorialTab()
    {
        turnOffAllTabs();
        tabs[1].SetActive(true);
        activeTab = 1;
    }

    private void turnOffAllTabs()
    {
        for(int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(false);
        }
    }
}
