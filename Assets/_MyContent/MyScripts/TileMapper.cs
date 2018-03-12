using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapper : MonoBehaviour {

	public GameObject[] MapTiles;
	public bool MapShown = false;

	// Use this for initialization
	void Awake () {
		HideMap();
	}
		
	public void ShowMap(){
		if(!MapShown){
			foreach(var i in MapTiles){
				i.SetActive(true);
			}
			MapShown = true;
		}
	}

	public void HideMap(){
		foreach(var i in MapTiles){
			i.SetActive(false);
		}
		MapShown = false;
	}
}
