using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    [SerializeField] GameObject pattern;
    public int count = 10;
    public float radius = 10;

    void Start()
    {
        transform.localScale = new Vector3(radius * 2, 1, radius * 2);
        for(int i = 0; i < count; i++)
        {
            GameObject temp = Instantiate(pattern);
            Vector2 pos = Random.insideUnitCircle * radius;
            temp.transform.position = transform.position + new Vector3(pos.x, 1.5f, pos.y);
        }
    }

    private void OnValidate()
    {
        transform.localScale = new Vector3(radius * 2, 1, radius * 2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
