using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class beamcast : MonoBehaviour {

    public GameObject rayCaster;

    float beamTimer = 0;

    LineRenderer line;

    ParticleSystem particles;

	void Start () {
        line = GetComponent<LineRenderer>();
        particles = GetComponent<ParticleSystem>();
        line.SetVertexCount(2);
        line.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if (rayCaster == null)
            rayCaster = gameObject;

        Vector3 rayPosition = rayCaster.transform.position;
        Vector3 rayVector = rayCaster.transform.forward;

        if (Input.GetMouseButton(0))
        {
            line.SetPosition(0, rayPosition);
            line.SetPosition(1, rayPosition + rayVector * 100);
            line.SetWidth(5f, 5f);
            line.enabled = true;
        }
    }

}
