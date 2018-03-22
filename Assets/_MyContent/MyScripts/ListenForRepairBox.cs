﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenForRepairBox : MonoBehaviour {

	public RepairableObject[] RepairBox;
	public bool Repaired = false;
	public GameObject Light;

	// Use this for initialization
	void Start () {
		
	}
	
	void Update(){
		int i = 0;
		int o = 0;
		foreach(RepairableObject r in RepairBox){
			i++;
			if(r.ObjectRepaired){
				o++;
			}
		}
		if(i == o){
			Repaired = true;
			Light.SetActive(true);
			Debug.Log("Object Fully Repaired!");
		} else {
			Repaired = false;
			Light.SetActive(false);
		}
	}
}
