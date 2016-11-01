using UnityEngine;
using System.Collections;

public class rotation : MonoBehaviour {

    public float rotationSpeed = 1f;

    public bool flipXAxis = false;

    public bool flipYAxis = false;

    bool rotateMode = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(1))
        {
            rotateMode = !rotateMode;
            Cursor.visible = !rotateMode;
            Cursor.lockState = rotateMode ? CursorLockMode.Locked : CursorLockMode.None;
        }

        if (rotateMode)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            gameObject.transform.Rotate((flipXAxis?-1:1) * Vector3.up * Time.deltaTime * h * rotationSpeed, Space.World);
            gameObject.transform.Rotate((flipYAxis ? -1 : 1) * Vector3.right * Time.deltaTime * v * rotationSpeed);
        }
	}
}
