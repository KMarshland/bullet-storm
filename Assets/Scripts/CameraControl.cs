﻿using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3 (
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"),
			Input.GetAxis("Mouse ScrollWheel")
		);
	}
}
