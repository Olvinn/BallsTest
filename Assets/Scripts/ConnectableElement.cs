using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConnectableElement : MonoBehaviour, IRaycastable
{
    static ConnectableElement selected, underPointer;
    static UnityEvent onColorize, onDecolorize;
    static Connection currentLink;

    [SerializeField] Connection linkPattern;

    MeshRenderer rend;
    Color startColor;

    private void Start()
    {
        if (onColorize == null)
            onColorize = new UnityEvent();
        onColorize.AddListener(() =>
        {
            if (selected != this)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeColor(Color.blue));
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ChangeColor(Color.yellow));
            }
        });

        if (onDecolorize == null)
            onDecolorize = new UnityEvent();
        onDecolorize.AddListener(() =>
        {
            StopAllCoroutines();
            StartCoroutine(ChangeColor(startColor));
        });

        rend = GetComponent<MeshRenderer>();
        startColor = rend.material.color;
    }

    public void OnBeginDrag()
    {
        selected = this;
        onColorize?.Invoke();
        CreateLink();
    }

    public void OnEndDrag()
    {
        onDecolorize?.Invoke();
        selected = null;
        if (linkPattern)
            ReleaseLink();
    }

    public void OnCursorEnter()
    {
        underPointer = this;
        if (selected)
            return;
        StopAllCoroutines();
        StartCoroutine(ChangeColor(Color.yellow));
    }

    public void OnCursorExit()
    {
        underPointer = null;
        if (selected)
            return;
        StopAllCoroutines();
        StartCoroutine(ChangeColor(startColor));
    }

    public void OnSelect()
    {
        selected = this;
        onColorize?.Invoke();
        CreateLink();
    }

    public void OnDeselect()
    {
        onDecolorize?.Invoke();
        selected = null;
        ReleaseLink();
    }

    void CreateLink()
    {
        currentLink = Instantiate(linkPattern.gameObject).GetComponent<Connection>();
        currentLink.begin = this;
    }

    void ReleaseLink()
    {
        if (underPointer && currentLink)
        {
            currentLink.end = underPointer;
            currentLink = null;
        }
        if (!underPointer)
        {
            Destroy(currentLink.gameObject);
        }
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
