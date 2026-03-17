using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class StringTyper : MonoBehaviour
{
    TextMeshProUGUI txt;
    public float typingSpeed = 10f;
    public bool isTyping;
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }
    public void StartTyping(string input)
    {
        if (isTyping) { return; }
        StartCoroutine(TypingProgress(input));
    }
    IEnumerator TypingProgress(string input)
    {
        isTyping = true;

        string typedText = "";
        txt.text = typedText;

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
        yield return null;
    }
}
