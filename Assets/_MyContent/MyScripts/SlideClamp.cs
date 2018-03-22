using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideClamp : MonoBehaviour {

	public float offsetA = -0.5f;
	public float offsetB = 1f;

	public Transform initTransform;
	public Vector3 initPosition;

	// Use this for initialization
	void Start () {
		initPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		var pos = transform.position;
		pos.x =  Mathf.Clamp(transform.position.x, initPosition.x + offsetA, initPosition.x + offsetB);
		Debug.Log(Mathf.Clamp(transform.position.x, initPosition.x + offsetA, initPosition.x + offsetB) + " Clamped x transform?");
		pos.y = initPosition.y;
		pos.z = initPosition.z;
		transform.position = pos;
	}
}
