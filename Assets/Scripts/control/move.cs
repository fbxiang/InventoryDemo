using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {

    public float accelerationMultiplier = 1;
    public float maxForwardSpeed = 1;
    float currentSpeed = 0;

    public float angulawrAccelerationMultiplier = 1;
    public float maxAngularSpeed = 1;
    float currentAngularSpeed = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("w"))
        {
            currentSpeed += accelerationMultiplier * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxForwardSpeed);
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed);
        }
        else
        {
            currentSpeed = 0f;
        }

        if (Input.GetKey("a")) {
            currentAngularSpeed -= angulawrAccelerationMultiplier * Time.deltaTime;
            currentAngularSpeed = Mathf.Clamp(currentAngularSpeed, -maxAngularSpeed, maxAngularSpeed);
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * currentAngularSpeed);
        }
        else if (Input.GetKey("d"))
        {
            currentAngularSpeed += angulawrAccelerationMultiplier * Time.deltaTime;
            currentAngularSpeed = Mathf.Clamp(currentAngularSpeed, -maxAngularSpeed, maxAngularSpeed);
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * currentAngularSpeed);
        }
        else
        {
            currentAngularSpeed = 0f;
        }

	}
}
