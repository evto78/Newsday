using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ReporterScript : MonoBehaviour
{
    public OfficeManager officeManager;
    public GameObject usb;
    StringTyper speechBubble;
    public int dialogeIndex;
    public int articleNumber;
    public List<string> introDialoge;
    public List<articleDialoge> articleSpesificDialoge;
    [System.Serializable]
    public class articleDialoge
    {
        public List<string> dialoge;
    }
    void Start()
    {
        articleNumber = 0;
        dialogeIndex = 0;
        speechBubble = GetComponentInChildren<StringTyper>();
        speechBubble.StartTyping(introDialoge[0]);
    }
    private void OnEnable()
    {
        if (Application.isPlaying && speechBubble != null)
        {
            articleNumber++;
            dialogeIndex = 0;
            speechBubble.StartTyping(introDialoge[0]);
        }
    }
    void Update()
    {
        if (!speechBubble.isTyping && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return)))
        {
            switch (articleNumber)
            {
                case 0:
                    dialogeIndex++;
                    if (dialogeIndex > introDialoge.Count - 1) { return; }
                    if (introDialoge[dialogeIndex] == "EXIT") { gameObject.SetActive(false); return; }
                    speechBubble.StartTyping(introDialoge[dialogeIndex]);
                    if (dialogeIndex == 6) { usb.gameObject.SetActive(true); }
                    break;
            }
            
        }
    }
}
