using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DunGen;

public class MyRuntimeDungeon : RuntimeDungeon {



	void Awake(){
		Generator.OnGenerationStatusChanged += OnChanged;
	}

	void OnChanged(DungeonGenerator dg, GenerationStatus gs){
		Debug.Log(gs);
	}
}
