using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class beam : MonoBehaviour {
    public Vector3 start, end;

    public float size = 5f;

    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        Vector3 rayPosition = start;

        if (Input.GetMouseButton(0))
        {
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            line.SetWidth(size, size);
            line.enabled = true;
        }
    }
}
