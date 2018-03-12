using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.InventoryPro;

[System.Serializable]
public class SuitHolder : MonoBehaviour {

	public GameObject SuitSpawnPoint;
	public GameObject CurrentSuitObject;
	public uint CurrentID;
	public InventoryItemBase Suit;

	public SuitHelper[] Suits;

	public void SuitObject(uint id){
		if(CurrentSuitObject != null){
			if(CurrentID != id){
				Destroy(CurrentSuitObject);
				MakeSuitObject(id);
			}
		} else {
			MakeSuitObject(id);
		}
	}

	public void MakeSuitObject(uint id){
		GameObject g = null;
		uint u = 0;
		foreach(var i in Suits){
			if(id == Suits[u].ID){
				g = Suits[u].suit;
			}
			u++;
		}
		if(g != null){
			CurrentSuitObject = GameObject.Instantiate(g, SuitSpawnPoint.transform.position, SuitSpawnPoint.transform.rotation);
			CurrentID = id;
		}
	}
}
