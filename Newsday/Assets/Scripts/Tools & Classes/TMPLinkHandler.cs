using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class TMPLinkHandler : MonoBehaviour, IPointerClickHandler
{
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
    string lastFrameHover; bool hovering;
    private void Update()
    {
        hovering = false;

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(textBox, mousePos, activeCamera);

        
        if (linkTaggedText != -1)
        {
            hovering = true;
            TMP_LinkInfo linkInfo = textBox.textInfo.linkInfo[linkTaggedText];
            lastFrameHover = linkInfo.GetLinkText();

            OnHoverOverLinkEvent.Invoke(linkInfo.GetLinkText(), textBox);
        }
        else { lastFrameHover = ""; }
        if(lastFrameHover != "" && !hovering) { OnStopHoverOverLinkEvent.Invoke(lastFrameHover, textBox); }
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
