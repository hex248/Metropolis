using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D pointerCursor;
    [SerializeField] Texture2D handCursor;

    public void Pointer()
    {
        Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
    }
    public void Hand()
    {
        Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
    }
}
