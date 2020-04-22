using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	public float speed = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		Vector3 targetCamPos = transform.position + new Vector3(-h, 0, 0) + new Vector3(0, 0, -v);
		transform.position = Vector3.Lerp(transform.position, targetCamPos, speed * Time.deltaTime);
	}
}
