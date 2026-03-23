using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using JetBrains.Annotations;
/*************************************************
* Last edited: Ryan McBride
*
* Description: This class will only start typing when startConversation() is called
* while reads out a list of strings that it was given, from speech[]
* it pauses inbetween lines until the player left mouse clicks or hits enter
* once it reaches its last line, then it stops. 
**************************************************/
public class StringTyper : MonoBehaviour
{
    TextMeshProUGUI txt;
    public float typingSpeed = 10f;
    public bool isTyping;
    public bool isTalking = false;
    private int speechIndex = 0;
    public string[] speech;
    private void Start()
    {
        speechIndex = 0;
    }


    private void Update()
    {

        if (speech.Length-1 == speechIndex)
        {
            isTalking = false;
        }

        //if we in a conversation, and we are done typing out the message we start off at 0 thats why
        if (isTalking && !isTyping && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return)))
        {
            speechIndex++;
            //Debug.Log(speechIndex);
            StartTyping(speech[speechIndex]);
        }

        

        //we were talking but now we stopped, we reset
        if (speechIndex != 0 && !isTalking)
        {
            speechIndex = 0;
        }
    }

    //starts the conversation and writes the first 
    public void startConversation(string []convo)
    {
        speech = convo;
        isTalking = true;
        StartTyping(speech[0]);
    }


    public void StartTyping(string input)
    {
        isTalking = true;
        if (isTyping) { return; }
        StartCoroutine(TypingProgress(input));
    }
    IEnumerator TypingProgress(string input)
    {
        isTyping = true;

        string typedText = "";
        txt = gameObject.GetComponent<TextMeshProUGUI>();
        txt.text = "";


        yield return new WaitForEndOfFrame();

        bool quickType = false;
        foreach (char c in input)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return) || quickType) { quickType = true; }
            else { yield return new WaitForSeconds(1f / typingSpeed); }
            typedText += c;
            txt.text = typedText;
        }

        isTyping = false;
    }
}
