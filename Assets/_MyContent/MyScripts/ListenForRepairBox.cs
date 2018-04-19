using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenForRepairBox : MonoBehaviour {

	public RepairableObject[] RepairBox;
	public bool Repaired = false;
	public IndicatorObjectController indicator;
	//public GameObject Light;

	// Use this for initialization
	void Start () {
		Debug.Log("ListenForRepairBox on " + this.gameObject.name + " gameObject Name | " + this.gameObject.transform.parent.name + " parent name");
	}
	
	void Update(){
		int i = 0;
		int o = 0;
		foreach(RepairableObject r in RepairBox){
			i++;
			if(r != null){
				if(r.ObjectRepaired){
					o++;
				}
			}
		}
		if(i == o){
			Repaired = true;
			indicator.SetState(IndicatorState.Positive);
			//Light.SetActive(true);
			Debug.Log("Object Fully Repaired!");
		} else {
			Repaired = false;
			indicator.SetState(IndicatorState.Negative);
			//Light.SetActive(false);
		}
	}
}
