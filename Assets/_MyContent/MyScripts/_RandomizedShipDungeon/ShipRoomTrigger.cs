using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class ShipRoomTrigger : MonoBehaviour {

	public GameObject[] RoomsToShow;
	public GameObject[] RoomsToHide;


	void OnTriggerEnter(Collider other) {
		
		foreach(var s in RoomsToShow){
			if(s.activeSelf == false){
				s.SetActive(true);
			}
		}
		foreach(var h in RoomsToHide){
			if(h.activeSelf == true){
				h.SetActive(false);
			}
		}

	}
}
