﻿using UnityEngine;
using System.Collections;

public class explode : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Explode();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Explode()
    {
        var exp = GetComponent<ParticleSystem>();
        exp.Play();
        Destroy(gameObject, exp.duration);
    }
}
