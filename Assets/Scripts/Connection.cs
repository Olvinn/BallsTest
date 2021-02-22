using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    public ConnectableElement begin, end;

    LineRenderer rend;

    void Start()
    {
        rend = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (begin)
            rend.SetPosition(0, begin.transform.position);

        if (end)
            rend.SetPosition(1, end.transform.position);
        else
        {
            Vector3 dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
            float camMod = Mathf.Abs(Camera.main.transform.position.y / dir.y);
            Vector3 curPos = Camera.main.transform.position + dir * camMod;
            rend.SetPosition(1, curPos);
        }
    }
}
