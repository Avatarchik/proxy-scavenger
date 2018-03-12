using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAtPoint : MonoBehaviour {

	public GameObject ObjectToCreate;

	// Use this for initialization
	void Awake () {
		CreateObject();
	}

	public void CreateObject(){
		Debug.Log("bullet : " + this.transform.position);
		GameObject.Instantiate(ObjectToCreate,this.transform.position,this.transform.rotation);
	}
}
