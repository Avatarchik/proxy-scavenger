using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DunGen;

public class DungeonGenerationStatus : MonoBehaviour {

	void Start(){
		DungeonGenerator d = GetComponent<DungeonGenerator>();
		d.OnGenerationStatusChanged += OnChanged;
	}

	void OnChanged(DungeonGenerator dg, GenerationStatus gs){
		Debug.Log(gs);
	}
}
