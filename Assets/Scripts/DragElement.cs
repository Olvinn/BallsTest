using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragElement : MonoBehaviour, IRaycastable
{
    public bool drag { get; private set; } = false;

    MeshRenderer rend;
    Color startColor;
    Transform parent;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
        startColor = rend.material.color;
        parent = transform.GetParent();
    }

    private void Update()
    {
        if (drag)
        {
            Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
            float camMod = Mathf.Abs(Camera.main.transform.position.y / dir.y);
            float trMod = Mathf.Abs(parent.position.y / dir.y);
            Vector3 curPos = Camera.main.transform.position + dir * (camMod - trMod);
            parent.position = curPos;
        }
    }

    public void OnBeginDrag()
    {
        drag = true;
    }

    public void OnEndDrag()
    {
        drag = false;
    }

    public void OnCursorEnter()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeColor(Color.green));
    }

    public void OnCursorExit()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeColor(startColor));
    }

    public void OnSelect()
    {

    }

    public void OnDeselect()
    {

    }

    IEnumerator ChangeColor(Color color)
    {
        while (rend.material.color != color)
        {
            rend.material.color = Vector4.MoveTowards(rend.material.color, color, Time.deltaTime * 2);
            yield return new WaitForEndOfFrame();
        }
    }
}
