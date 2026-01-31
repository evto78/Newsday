using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
}
