using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class TMPLinkHandler : MonoBehaviour, IPointerClickHandler
{
    //A very magical script that allows linked text to be clickable within tmpro
    //must be added to the same gameobject as the tmpro for it to work

    private TMP_Text textBox;
    private Canvas activeCanvas;
    private Camera activeCamera;

    public delegate void ClickOnLinkEvent(string keyword, TMP_Text tmp);
    public static event ClickOnLinkEvent OnClickedOnLinkEvent;

    public delegate void HoverOverLinkEvent(string keyword, TMP_Text tmp);
    public static event HoverOverLinkEvent OnHoverOverLinkEvent;

    public delegate void StopHoverOverLinkEvent(string keyword, TMP_Text tmp);
    public static event StopHoverOverLinkEvent OnStopHoverOverLinkEvent;

    private void Awake()
    {
        activeCamera = Camera.main;

        textBox = GetComponent<TMP_Text>();
        activeCanvas = GetComponentInParent<Canvas>();

        if(activeCanvas.renderMode == RenderMode.ScreenSpaceOverlay) { activeCamera = null; }
        else { activeCamera = activeCanvas.worldCamera; }
    }
    bool hovering; TMP_LinkInfo prevLinkInfo;
    private void Update()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(textBox, mousePos, activeCamera);

        if (linkTaggedText != -1)
        {
            TMP_LinkInfo linkInfo = textBox.textInfo.linkInfo[linkTaggedText];

            OnHoverOverLinkEvent.Invoke(linkInfo.GetLinkText(), textBox);
            hovering = true;
            prevLinkInfo = linkInfo;
        }
        else if (hovering)
        {
            OnStopHoverOverLinkEvent.Invoke(prevLinkInfo.GetLinkText(), textBox);
            hovering = false;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousePos = new Vector3(eventData.position.x, eventData.position.y, 0f);

        var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(textBox, mousePos, activeCamera);

        if (linkTaggedText != -1)
        {
            TMP_LinkInfo linkInfo = textBox.textInfo.linkInfo[linkTaggedText];

            OnClickedOnLinkEvent.Invoke(linkInfo.GetLinkText(), textBox);
        }
    }
}
