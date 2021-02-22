using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    IRaycastable underCursor = null, dragged = null, selected = null;

    Ray ray;
    RaycastHit hit;
    IRaycastable currentObj;
    bool select, drag, currentObjectNotNull, underCursorNotNull, draggedNotNull, selectedNotNull;
    Vector3 pDownPos;
    float selectTimer, selectTime = .25f;

    void Update()
    {
        currentObj = null;
        currentObjectNotNull = false;
        underCursorNotNull = underCursor != null && !underCursor.Equals(null);
        draggedNotNull = dragged != null && !dragged.Equals(null);
        selectedNotNull = selected != null && !selected.Equals(null);

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        currentObj = null;

        ClickProcess();

        if (Physics.Raycast(ray, out hit, 2000))
        {
            currentObj = hit.collider.gameObject.GetComponent(typeof(IRaycastable)) as IRaycastable;
            currentObjectNotNull = currentObj != null && !currentObj.Equals(null);

            if (currentObjectNotNull)
            {
                if (underCursorNotNull)
                {
                    if (underCursor != currentObj)
                    {
                        underCursor.OnCursorExit();
                        underCursor = currentObj;
                        currentObj.OnCursorEnter();
                    }
                }
                else
                {
                    underCursor = currentObj;
                    currentObj.OnCursorEnter();
                }
            }
            else
            {
                if (underCursorNotNull)
                {
                    underCursor.OnCursorExit();
                    underCursor = null;
                }
            }
        }
        else
        {
            if (underCursorNotNull)
            {
                underCursor.OnCursorExit();
                underCursor = null;
            }
        }

        if (select)
        {
            if (!selectedNotNull && currentObjectNotNull)
            {
                selected = currentObj;
                currentObj.OnSelect();
            }
            if (selectedNotNull)
            {
                selected.OnDeselect();
                selected = null;
            }
            select = false;
        } 

        if (drag && !draggedNotNull && currentObjectNotNull)
        {
            dragged = currentObj;
            currentObj.OnBeginDrag();
        }
        else if (!drag && draggedNotNull)
        {
            dragged.OnEndDrag();
            dragged = null;
        }
    }

    //Run from Update
    void ClickProcess()
    {
        if (Input.GetMouseButton(0))
            selectTimer += Time.deltaTime;

        bool pDown = Input.GetMouseButtonDown(0);
        bool pUp = Input.GetMouseButtonUp(0);

        if (pDown)
        {
            pDownPos = Input.mousePosition;
            selectTimer = 0;
        }

        Vector3 delta = pDownPos - Input.mousePosition;

        if (delta.magnitude > 2 && Input.GetMouseButton(0))
            drag = true;
        else
            drag = false;

        if (pUp && !drag && selectTimer < selectTime)
            select = true;
    }
}
