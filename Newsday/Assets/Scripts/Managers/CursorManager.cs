using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CursorManager : MonoBehaviour
{
    [System.Serializable]
    public class CursorData { public Texture2D cursorSprite; public Vector2 hotspot; }
    public List<CursorData> cursors;
    int currentCursor = -1;
    void Start() { Cursor.SetCursor(cursors[0].cursorSprite, cursors[0].hotspot, CursorMode.ForceSoftware); }
    public void ChangeCursor(int id)
    {
        if(id == currentCursor) { return; }
        currentCursor = id;
        Cursor.SetCursor(cursors[id].cursorSprite, cursors[id].hotspot, CursorMode.ForceSoftware);
    }
    public void ResetCursor()
    {
        currentCursor = -1;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    bool hovering;
    private void Update()
    {
        if (IsPointerOverUIElement(GetEventSystemRaycastResults()))
        {
            ChangeCursor(1);
            hovering = true; 
        }
        else if (hovering)
        {
            ChangeCursor(0);
            hovering = false;
        }
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.tag == "Clickable")
            { return true; }
        }
        return false;
    }
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
