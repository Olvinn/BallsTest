using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRaycastable
{
    void OnBeginDrag();

    void OnEndDrag();

    void OnCursorEnter();

    void OnCursorExit();

    void OnSelect();

    void OnDeselect();
}


public static class Utils
{
    public static Transform GetParent(this Transform cur)
    {
        if (cur.parent != null)
            return cur.parent.GetParent();
        else
            return cur;
    }
}
